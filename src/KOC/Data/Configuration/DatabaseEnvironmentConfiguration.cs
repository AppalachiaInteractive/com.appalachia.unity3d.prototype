using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Root;
using Appalachia.Data.Core;
using Appalachia.Data.Core.Configuration;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Data.Configuration
{
    public class DatabaseEnvironmentConfiguration : AppalachiaObject<DatabaseEnvironmentConfiguration>
    {
        #region Fields and Autoproperties

        [InlineProperty, HideLabel, FoldoutGroup(APPASTR.Game_State)]
        public DatabaseConfigurationSettings gameState;

        [InlineProperty, HideLabel, FoldoutGroup(APPASTR.Metadata)]
        public DatabaseConfigurationSettings metadata;

        [InlineProperty, HideLabel, FoldoutGroup(APPASTR.User)]
        public DatabaseConfigurationSettings user;

        [InlineProperty, HideLabel, FoldoutGroup(APPASTR.Game_State)]
        public DatabaseLocationConfiguration gameStateSaveLocation;

        [InlineProperty, HideLabel, FoldoutGroup(APPASTR.Metadata)]
        public DatabaseLocationConfiguration metadataSaveLocation;

        [InlineProperty, HideLabel, FoldoutGroup(APPASTR.User)]
        public DatabaseLocationConfiguration userSaveLocation;

        #endregion

        public DatabaseLocationConfiguration GetLocation(DatabaseType type)
        {
            using (_PRF_GetLocation.Auto())
            {
                switch (type)
                {
                    case DatabaseType.Metadata:
                        return metadataSaveLocation;
                    case DatabaseType.User:
                        return userSaveLocation;
                    case DatabaseType.GameState:
                        return gameStateSaveLocation;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
        }

        public DatabaseConfigurationSettings GetSettings(DatabaseType type)
        {
            using (_PRF_GetSettings.Auto())
            {
                switch (type)
                {
                    case DatabaseType.Metadata:
                        return metadata;
                    case DatabaseType.User:
                        return user;
                    case DatabaseType.GameState:
                        return gameState;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetLocation =
            new ProfilerMarker(_PRF_PFX + nameof(GetLocation));

        private static readonly ProfilerMarker _PRF_GetSettings =
            new ProfilerMarker(_PRF_PFX + nameof(GetSettings));

        #endregion
    }
}
