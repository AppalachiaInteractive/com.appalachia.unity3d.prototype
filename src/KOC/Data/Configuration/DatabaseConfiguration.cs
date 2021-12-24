using System;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Data.Core;
using Appalachia.Data.Core.Configuration;
using Appalachia.Prototype.KOC.Data.Collections;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Data.Configuration
{
    public class DatabaseConfiguration : SingletonAppalachiaObject<DatabaseConfiguration>
    {
        #region Fields and Autoproperties

        public ActiveDatabaseConfiguration activeConfiguration;

        [SmartLabel, BoxGroup(APPASTR.Developer), ReadOnly]
        public UserSpecificDatabaseEnvironmentConfiguration developer;

        [SmartLabel, BoxGroup(APPASTR.Developer), ReadOnly]
        public UserSpecificDatabaseEnvironmentConfiguration developer2;

        [SmartLabel, BoxGroup(APPASTR.Editor), ReadOnly]
        public DatabaseEnvironmentConfiguration editor;

        [SmartLabel, BoxGroup(APPASTR.Runtime), ReadOnly]
        public DatabaseEnvironmentConfiguration runtime;

        #endregion


        public string GetDataStorageFileExtension(DatabaseConfigurationSettings settings)
        {
            using (_PRF_GetDataStorageFileExtension.Auto())
            {
                var extension = GetFileExtension(settings);
                return extension;
            }
        }

        public string GetDataStorageFileNameWithoutExtension(
            DatabaseConfigurationSettings settings,
            string postfix = null)
        {
            using (_PRF_GetDataStorageFileNameWithoutExtension.Auto())
            {
                var fullPostfix = postfix == null ? string.Empty : ZString.Format("-{0}", postfix);

                var fileName = ZString.Format("{0}{1}", settings.name, fullPostfix);

                return fileName;
            }
        }

        public DatabaseEnvironmentConfiguration GetEnvironment()
        {
            using (_PRF_GetDataLocation.Auto())
            {
#if UNITY_EDITOR
                switch (activeConfiguration)
                {
                    case ActiveDatabaseConfiguration.Editor:
                        return editor;
                    case ActiveDatabaseConfiguration.Developer:
                        return developer.GetContext();
                    case ActiveDatabaseConfiguration.Developer2:
                        return developer2.GetContext();
                    case ActiveDatabaseConfiguration.Runtime:
#endif
                        return runtime;
#if UNITY_EDITOR
                    default:
                        throw new ArgumentOutOfRangeException();
                }
#endif
            }
        }

        public string GetFileExtension(DatabaseConfigurationSettings settings)
        {
            using (_PRF_GetFileExtension.Auto())
            {
                if (settings.technology == DatabaseTechnology.ScriptableObject)
                {
                    return ".asset";
                }

                const string prefix = "appa";
                const char separator = '.';

                // ReSharper disable once UnusedVariable
                const char postfixq = 'q';
                const char encrypted = 'e';
                const char unencrypted = 'u';
                const char postfixj = 'j';
                const char postfixu = 'u';
                const char postfixl = 'l';

                var techChar = settings.technology switch
                {
                    DatabaseTechnology.Json        => postfixj,
                    DatabaseTechnology.LiteDB      => postfixl,
                    DatabaseTechnology.UltraLiteDb => postfixu,
                    _                              => throw new ArgumentOutOfRangeException()
                };

                var result = ZString.Format(
                    "{0}{1}{2}{3}",
                    separator,
                    prefix,
                    settings.isEncrypted ? encrypted : unencrypted,
                    techChar
                );

                return result;
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

#if UNITY_EDITOR
                initializer.Reset(this, "2021-11-19-01");

                await initializer.Do(this, nameof(runtime), runtime == null, ConfigureRuntimeDefaults);

                await initializer.Do(this, nameof(editor), editor == null, ConfigureEditorDefaults);

                if (developer != null)
                {
                    developer.SetObjectOwnership(this);
                }

                await initializer.Do(
                    this,
                    nameof(developer),
                    (developer == null) || !developer.HasContext(),
                    () =>
                    {
                        activeConfiguration = ActiveDatabaseConfiguration.Developer;

                        ConfigureDeveloperDefaults();
                    }
                );

                if (developer2 != null)
                {
                    developer2.SetObjectOwnership(this);
                }

                await initializer.Do(
                    this,
                    nameof(developer2),
                    (developer2 == null) || !developer2.HasContext(),
                    ConfigureDeveloper2Defaults
                );

                developer.SetObjectOwnership(this);
                developer2.SetObjectOwnership(this);
#endif
            }
        }

#if UNITY_EDITOR
        private static void ConfigureDefaultsInternal(
            string contextName,
            ref UserSpecificDatabaseEnvironmentConfiguration config,
            bool isEncrypted,
            (DataLocation loc, DatabaseTechnology tech, string key) user,
            (DataLocation loc, DatabaseTechnology tech, string key) metadata,
            (DataLocation loc, DatabaseTechnology tech, string key) gameState)
        {
            using (_PRF_ConfigureDeveloperDefaultsInternal.Auto())
            {
                if (config == null)
                {
                    config = new UserSpecificDatabaseEnvironmentConfiguration();
                }

                DatabaseEnvironmentConfiguration context;

                if (config.HasContext())
                {
                    context = config.GetContext();
                }
                else
                {
                    context = config.CreateNew(contextName);
                }

                var contextKey = config.GetContextKey();

                if (context.userSaveLocation == null)
                {
                    context.userSaveLocation = new DatabaseLocationConfiguration(
                        user.loc,
                        true,
                        true,
                        AppaPath.Combine("Data", contextKey)
                    );

                    context.user.name = "User";
                    context.user.technology = user.tech;
                    context.user.isEncrypted = isEncrypted;
                    context.user.key = user.key;
                    Modifications.MarkAsModified(context);
                }

                if (context.metadataSaveLocation == null)
                {
                    context.metadataSaveLocation = new DatabaseLocationConfiguration(
                        metadata.loc,
                        true,
                        true,
                        AppaPath.Combine("Data", contextKey)
                    );

                    context.metadata.name = "Metadata";
                    context.metadata.technology = metadata.tech;
                    context.metadata.isEncrypted = isEncrypted;
                    context.metadata.key = metadata.key;
                    Modifications.MarkAsModified(context);
                }

                if (context.gameStateSaveLocation == null)
                {
                    context.gameStateSaveLocation = new DatabaseLocationConfiguration(
                        gameState.loc,
                        true,
                        true,
                        AppaPath.Combine("Data", contextKey)
                    );

                    context.gameState.name = "GameState";
                    context.gameState.technology = gameState.tech;
                    context.gameState.isEncrypted = isEncrypted;
                    context.gameState.key = gameState.key;
                    Modifications.MarkAsModified(context);
                }
            }
        }

        private void ConfigureDeveloper2Defaults()
        {
            using (_PRF_ConfigureDeveloper2Defaults.Auto())
            {
                ConfigureDefaultsInternal(
                    nameof(developer2),
                    ref developer2,
                    true,
                    (DataLocation.ApplicationData, DatabaseTechnology.Json,
                        APPASTR.EncryptionKeys.DB_DEVELOPER2_USER),
                    (DataLocation.LocalApplicationData, DatabaseTechnology.Json,
                        APPASTR.EncryptionKeys.DB_DEVELOPER2_METADATA),
                    (DataLocation.CommonApplicationData, DatabaseTechnology.Json,
                        APPASTR.EncryptionKeys.DB_DEVELOPER2_GAMESTATE)
                );
            }
        }

        private void ConfigureDeveloperDefaults()
        {
            using (_PRF_ConfigureDeveloperDefaults.Auto())
            {
                ConfigureDefaultsInternal(
                    nameof(developer),
                    ref developer,
                    false,
                    (DataLocation.PrototypeHardcoded, DatabaseTechnology.ScriptableObject,
                        APPASTR.EncryptionKeys.DB_DEVELOPER_USER),
                    (DataLocation.PrototypeHardcoded, DatabaseTechnology.ScriptableObject,
                        APPASTR.EncryptionKeys.DB_DEVELOPER_METADATA),
                    (DataLocation.PrototypeHardcoded, DatabaseTechnology.ScriptableObject,
                        APPASTR.EncryptionKeys.DB_DEVELOPER_GAMESTATE)
                );
            }
        }

        private void ConfigureEditorDefaults()
        {
            using (_PRF_ConfigureEditorDefaults.Auto())
            {
                if (editor == null)
                {
                    editor = DatabaseEnvironmentConfiguration.LoadOrCreateNew(APPASTR.Editor);
                }

                editor.userSaveLocation = new DatabaseLocationConfiguration(
                    DataLocation.LocalApplicationData,
                    true,
                    true,
                    "Data"
                );

                editor.metadataSaveLocation = new DatabaseLocationConfiguration(
                    DataLocation.LocalApplicationData,
                    true,
                    true,
                    "Data"
                );

                editor.gameStateSaveLocation = new DatabaseLocationConfiguration(
                    DataLocation.LocalApplicationData,
                    true,
                    true,
                    "Data"
                );

                editor.user.name = "U";
                editor.user.technology = DatabaseTechnology.UltraLiteDb;
                editor.user.isEncrypted = true;
                editor.user.key = APPASTR.EncryptionKeys.DB_EDITOR_USER;

                editor.metadata.name = "M";
                editor.metadata.technology = DatabaseTechnology.UltraLiteDb;
                editor.metadata.isEncrypted = false;
                editor.metadata.key = APPASTR.EncryptionKeys.DB_EDITOR_METADATA;

                editor.gameState.name = "G";
                editor.gameState.technology = DatabaseTechnology.UltraLiteDb;
                editor.gameState.isEncrypted = false;
                editor.gameState.key = APPASTR.EncryptionKeys.DB_EDITOR_GAMESTATE;
            }
        }

        private void ConfigureRuntimeDefaults()
        {
            using (_PRF_ConfigureRuntimeDefaults.Auto())
            {
                if (runtime == null)
                {
                    runtime = DatabaseEnvironmentConfiguration.LoadOrCreateNew(APPASTR.Runtime);
                }

                runtime.userSaveLocation = new DatabaseLocationConfiguration(
                    DataLocation.LocalApplicationData,
                    true,
                    true,
                    "Data"
                );

                runtime.metadataSaveLocation = new DatabaseLocationConfiguration(
                    DataLocation.LocalApplicationData,
                    true,
                    true,
                    "Data"
                );

                runtime.gameStateSaveLocation = new DatabaseLocationConfiguration(
                    DataLocation.LocalApplicationData,
                    true,
                    true,
                    "Data"
                );

                runtime.user.name = "U";
                runtime.user.technology = DatabaseTechnology.UltraLiteDb;
                runtime.user.isEncrypted = true;
                runtime.user.key = APPASTR.EncryptionKeys.DB_RUNTIME_USER;

                runtime.metadata.name = "M";
                runtime.metadata.technology = DatabaseTechnology.UltraLiteDb;
                runtime.metadata.isEncrypted = true;
                runtime.metadata.key = APPASTR.EncryptionKeys.DB_RUNTIME_METADATA;

                runtime.gameState.name = "G";
                runtime.gameState.technology = DatabaseTechnology.UltraLiteDb;
                runtime.gameState.isEncrypted = true;
                runtime.gameState.key = APPASTR.EncryptionKeys.DB_RUNTIME_GAMESTATE;
            }
        }
#endif

        #region Profiling

        private const string _PRF_PFX = nameof(DatabaseConfiguration) + ".";

#if UNITY_EDITOR
        private static readonly ProfilerMarker _PRF_ConfigureDeveloperDefaultsInternal =
            new ProfilerMarker(_PRF_PFX + nameof(ConfigureDefaultsInternal));

        private static readonly ProfilerMarker _PRF_ConfigureDeveloper2Defaults =
            new ProfilerMarker(_PRF_PFX + nameof(ConfigureDeveloper2Defaults));

        private static readonly ProfilerMarker _PRF_ConfigureDeveloperDefaults =
            new ProfilerMarker(_PRF_PFX + nameof(ConfigureDeveloperDefaults));

        private static readonly ProfilerMarker _PRF_ConfigureEditorDefaults =
            new ProfilerMarker(_PRF_PFX + nameof(ConfigureEditorDefaults));

        private static readonly ProfilerMarker _PRF_ConfigureRuntimeDefaults =
            new ProfilerMarker(_PRF_PFX + nameof(ConfigureRuntimeDefaults));
#endif

        private static readonly ProfilerMarker _PRF_GetDataLocation =
            new ProfilerMarker(_PRF_PFX + nameof(GetEnvironment));

        private static readonly ProfilerMarker _PRF_GetFileExtension =
            new ProfilerMarker(_PRF_PFX + nameof(GetFileExtension));

        private static readonly ProfilerMarker _PRF_GetDataStorageFileExtension =
            new ProfilerMarker(_PRF_PFX + nameof(GetDataStorageFileExtension));

        private static readonly ProfilerMarker _PRF_GetDataStorageFileNameWithoutExtension =
            new ProfilerMarker(_PRF_PFX + nameof(GetDataStorageFileNameWithoutExtension));

        

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        #endregion
    }
}
