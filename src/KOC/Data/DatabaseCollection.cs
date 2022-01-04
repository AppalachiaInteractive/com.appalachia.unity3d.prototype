using System;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Data.Core;
using Appalachia.Data.Core.AccessLayer;
using Appalachia.Prototype.KOC.Data.Configuration;
using Appalachia.Prototype.KOC.Data.Databases;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Data
{
    [Serializable]
    public class DatabaseCollection : IDisposable
    {
        #region Static Fields and Autoproperties

        private static DatabaseConfiguration _config;

        #endregion

        #region Fields and Autoproperties

        [HideLabel, InlineProperty, BoxGroup(APPASTR.Current_Game)]
        public GameStateDatabase currentGame;

        [HideLabel, InlineProperty, BoxGroup(APPASTR.Metadata)]
        public MetadataDatabase metadata;

        [ReadOnly] public string dataSetName;

        [HideLabel, InlineProperty, BoxGroup(APPASTR.User)]
        public UserDatabase user;

        #endregion

        public static DatabaseCollection CreateOrLoad(DatabaseConfiguration config, string dataSetName)
        {
            using (_PRF_CreateOrLoad.Auto())
            {
                _config = config;

                var userAccess = GetDatabaseAccess(DatabaseType.User,         dataSetName);
                var metadataAccess = GetDatabaseAccess(DatabaseType.Metadata, dataSetName);

                var instance = new DatabaseCollection
                {
                    user = UserDatabase.InitializeDatabase(_config, userAccess),
                    metadata = MetadataDatabase.InitializeDatabase(_config, metadataAccess),
                    dataSetName = dataSetName
                };

                return instance;
            }
        }

        public void LoadGame(DatabaseConfiguration config, long gameId)
        {
            using (_PRF_LoadGame.Auto())
            {
                _config = config;

                var postfix = gameId.ToString();
                var userAccess = GetDatabaseAccess(DatabaseType.GameState, dataSetName, postfix);

                currentGame = GameStateDatabase.InitializeDatabase(_config, userAccess, gameId);
            }
        }

        public void SaveGame()
        {
            using (_PRF_SaveGame.Auto())
            {
                currentGame.Save();
            }
        }

        private static DatabaseAccess GetDatabaseAccess(
            DatabaseType type,
            string dataSetName,
            string postfix = null)
        {
            using (_PRF_GetDatabaseAccess.Auto())
            {
                var environment = _config.GetEnvironment();
                var location = environment.GetLocation(type);
                var settings = environment.GetSettings(type);

                var storageDirectory = location.GetBasePath();

                var storageFileExtension = _config.GetDataStorageFileExtension(settings);
                var storageFileNameWithoutExtension =
                    _config.GetDataStorageFileNameWithoutExtension(settings, postfix);

                var fileName = ZString.Format(
                    "{0}{1}",
                    storageFileNameWithoutExtension,
                    storageFileExtension
                );

                var filePath = AppaPath.Combine(storageDirectory, dataSetName, fileName);

                var access = DatabaseAccess.GetDatabaseAccess(filePath, settings);

                return access;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            using (_PRF_Dispose.Auto())
            {
                user?.Dispose();
                metadata?.Dispose();
                currentGame?.Dispose();
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(Databases) + ".";

        private static readonly ProfilerMarker _PRF_GetDatabaseAccess =
            new ProfilerMarker(_PRF_PFX + nameof(GetDatabaseAccess));

        private static readonly ProfilerMarker
            _PRF_SaveGame = new ProfilerMarker(_PRF_PFX + nameof(SaveGame));

        private static readonly ProfilerMarker _PRF_Dispose = new ProfilerMarker(_PRF_PFX + nameof(Dispose));

        private static readonly ProfilerMarker _PRF_CreateOrLoad =
            new ProfilerMarker(_PRF_PFX + nameof(CreateOrLoad));

        private static readonly ProfilerMarker
            _PRF_LoadGame = new ProfilerMarker(_PRF_PFX + nameof(LoadGame));

        #endregion
    }
}
