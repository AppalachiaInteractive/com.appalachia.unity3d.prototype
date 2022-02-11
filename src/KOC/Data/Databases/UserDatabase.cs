using System;
using Appalachia.Data.Core;
using Appalachia.Prototype.KOC.Data.Collections.User;
using Appalachia.Prototype.KOC.Data.Documents.User;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Data.Databases
{
    [Serializable]
    public class UserDatabase : KOCDatabase<UserDatabase>
    {
        #region Fields and Autoproperties

        public QualitySettingLevelCollection QualitySettingLevel { get; set; }
        public SavedGameCollection SavedGames { get; set; }
        public UserSettingsCollection UserSettings { get; set; }

        #endregion

        public override DatabaseType Type => DatabaseType.User;

        protected override void Dispose(bool disposing)
        {
            using (_PRF_Dispose.Auto())
            {
                base.Dispose(true);

                if (disposing)
                {
                }
            }
        }

        protected override void RegisterCollections()
        {
            using (_PRF_RegisterCollections.Auto())
            {
                var qualitySettings = QualitySettingLevel;
                var savedGames = SavedGames;
                var userSettings = UserSettings;

                RegisterCollection<QualitySettingLevel, QualitySettingLevelCollection>(ref qualitySettings);
                RegisterCollection<SavedGame, SavedGameCollection>(ref savedGames);
                RegisterCollection<UserSettings, UserSettingsCollection>(ref userSettings);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_Dispose = new ProfilerMarker(_PRF_PFX + nameof(Dispose));

        private static readonly ProfilerMarker _PRF_RegisterCollections =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterCollections));

        #endregion
    }
}
