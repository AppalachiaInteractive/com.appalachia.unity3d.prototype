using System;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Scriptables;
using Appalachia.Data.Core;
using Appalachia.Data.Core.Configuration;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Data.Configuration
{
    public class DatabaseConfiguration : SingletonAppalachiaObject<DatabaseConfiguration>
    {
        #region Fields and Autoproperties

        public ActiveDatabaseConfiguration activeConfiguration;

        [SmartLabel, BoxGroup(APPASTR.Developer), ReadOnly, NonSerialized, ShowInInspector]
        public DatabaseEnvironmentConfiguration developer;

        [SmartLabel, BoxGroup(APPASTR.Developer), ReadOnly, NonSerialized, ShowInInspector]
        public DatabaseEnvironmentConfiguration developer2;

        [SmartLabel, BoxGroup(APPASTR.Editor), ReadOnly]
        public DatabaseEnvironmentConfiguration editor;

        [SmartLabel, BoxGroup(APPASTR.Runtime), ReadOnly]
        public DatabaseEnvironmentConfiguration runtime;

        #endregion

        #region Event Functions

        protected override void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                base.Awake();
                Initialize();
            }
        }

        protected override void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                Initialize();
            }
        }

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
                var fullPostfix = postfix == null ? string.Empty : $"-{postfix}";

                var fileName = $"{settings.name}{fullPostfix}";

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
                        return developer;
                    case ActiveDatabaseConfiguration.Developer2:
                        return developer2;
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

                var result =
                    $"{separator}{prefix}{(settings.isEncrypted ? encrypted : unencrypted)}{techChar}";

                return result;
            }
        }

        private static readonly ProfilerMarker _PRF_ConfigureDeveloper2Defaults = new ProfilerMarker(_PRF_PFX + nameof(ConfigureDeveloper2Defaults));
        private void ConfigureDeveloper2Defaults(string username)
        {
            using (_PRF_ConfigureDeveloper2Defaults.Auto())
            {
                var developer2Name = username + "2";

                if (developer2 == null)
                {
                    developer2 = LoadOrCreateNew<DatabaseEnvironmentConfiguration>(developer2Name);
                }

                developer2.userSaveLocation = new DatabaseLocationConfiguration(
                    DataLocation.ApplicationData,
                    true,
                    true,
                    AppaPath.Combine("Data", developer2Name)
                );

                developer2.metadataSaveLocation = new DatabaseLocationConfiguration(
                    DataLocation.LocalApplicationData,
                    true,
                    true,
                    AppaPath.Combine("Data", developer2Name)
                );

                developer2.gameStateSaveLocation = new DatabaseLocationConfiguration(
                    DataLocation.CommonApplicationData,
                    true,
                    true,
                    AppaPath.Combine("Data", developer2Name)
                );

                developer2.user.name = "User";
                developer2.user.technology = DatabaseTechnology.Json;
                developer2.user.isEncrypted = true;
                developer2.user.key = APPASTR.EncryptionKeys.DB_DEVELOPER2_USER;

                developer2.metadata.name = "Metadata";
                developer2.metadata.technology = DatabaseTechnology.Json;
                developer2.metadata.isEncrypted = true;
                developer2.metadata.key = APPASTR.EncryptionKeys.DB_DEVELOPER2_METADATA;

                developer2.gameState.name = "GameState";
                developer2.gameState.technology = DatabaseTechnology.Json;
                developer2.gameState.isEncrypted = true;
                developer2.gameState.key = APPASTR.EncryptionKeys.DB_DEVELOPER2_GAMESTATE;
            }
        }

        private static readonly ProfilerMarker _PRF_ConfigureDeveloperDefaults = new ProfilerMarker(_PRF_PFX + nameof(ConfigureDeveloperDefaults));
        private void ConfigureDeveloperDefaults(string username)
        {
            using (_PRF_ConfigureDeveloperDefaults.Auto())
            {
                if (developer == null)
                {
                    developer = LoadOrCreateNew<DatabaseEnvironmentConfiguration>(
                        username,
                        ownerType: typeof(DatabaseConfiguration)
                    );
                }

                developer.userSaveLocation = new DatabaseLocationConfiguration(
                    DataLocation.PrototypeHardcoded,
                    false,
                    false,
                    AppaPath.Combine("Data", username)
                );

                developer.metadataSaveLocation = new DatabaseLocationConfiguration(
                    DataLocation.PrototypeHardcoded,
                    false,
                    false,
                    AppaPath.Combine("Data", username)
                );

                developer.gameStateSaveLocation = new DatabaseLocationConfiguration(
                    DataLocation.PrototypeHardcoded,
                    true,
                    true,
                    AppaPath.Combine("Data", username)
                );

                developer.user.name = "User";
                developer.user.technology = DatabaseTechnology.ScriptableObject;
                developer.user.isEncrypted = false;
                developer.user.key = APPASTR.EncryptionKeys.DB_DEVELOPER_USER;

                developer.metadata.name = "Metadata";
                developer.metadata.technology = DatabaseTechnology.ScriptableObject;
                developer.metadata.isEncrypted = false;
                developer.metadata.key = APPASTR.EncryptionKeys.DB_DEVELOPER_METADATA;

                developer.gameState.name = "GameState";
                developer.gameState.technology = DatabaseTechnology.ScriptableObject;
                developer.gameState.isEncrypted = false;
                developer.gameState.key = APPASTR.EncryptionKeys.DB_DEVELOPER_GAMESTATE;
            }
        }

        private static readonly ProfilerMarker _PRF_ConfigureEditorDefaults = new ProfilerMarker(_PRF_PFX + nameof(ConfigureEditorDefaults));
        private void ConfigureEditorDefaults()
        {
            using (_PRF_ConfigureEditorDefaults.Auto())
            {
                if (editor == null)
                {
                    editor = LoadOrCreateNew<DatabaseEnvironmentConfiguration>(APPASTR.Editor);
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

        private static readonly ProfilerMarker _PRF_ConfigureRuntimeDefaults = new ProfilerMarker(_PRF_PFX + nameof(ConfigureRuntimeDefaults));
        private void ConfigureRuntimeDefaults()
        {
            using (_PRF_ConfigureRuntimeDefaults.Auto())
            {
                if (runtime == null)
                {
                    runtime = LoadOrCreateNew<DatabaseEnvironmentConfiguration>(APPASTR.Runtime);
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

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();
                
                initializer.Reset(this, "2021-11-19-01");

                initializer.Initialize(
                    this,
                    APPASTR.General,
                    (developer == null) || (developer2 == null) || (editor == null) || (runtime == null),
                    () =>
                    {
                        var username = Environment.UserName.ToLower();

                        if (username == "janic")
                        {
                            username = "janice";
                        }

                        activeConfiguration = ActiveDatabaseConfiguration.Developer;

                        ConfigureRuntimeDefaults();

                        ConfigureEditorDefaults();

                        ConfigureDeveloperDefaults(username);

                        ConfigureDeveloper2Defaults(username);
                    }
                );
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(DatabaseConfiguration) + ".";

        private static readonly ProfilerMarker _PRF_GetDataLocation =
            new ProfilerMarker(_PRF_PFX + nameof(GetEnvironment));

        private static readonly ProfilerMarker _PRF_GetFileExtension =
            new ProfilerMarker(_PRF_PFX + nameof(GetFileExtension));

        private static readonly ProfilerMarker _PRF_GetDataStorageFileExtension =
            new ProfilerMarker(_PRF_PFX + nameof(GetDataStorageFileExtension));

        private static readonly ProfilerMarker _PRF_GetDataStorageFileNameWithoutExtension =
            new ProfilerMarker(_PRF_PFX + nameof(GetDataStorageFileNameWithoutExtension));

        private static readonly ProfilerMarker _PRF_GetFileExtensionForTechnology =
            new ProfilerMarker(_PRF_PFX + nameof(GetFileExtension));

        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        #endregion
    }
}
