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

        public QualitySettingsCollection QualitySettings { get; set; }
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

        protected override void OnInitialize()
        {
            using (_PRF_OnInitialize.Auto())
            {
                var qualitySettings = QualitySettings;
                var savedGames = SavedGames;
                var userSettings = UserSettings;

                RegisterCollection<QualitySettingLevel, QualitySettingsCollection>(ref qualitySettings);
                RegisterCollection<SavedGame, SavedGameCollection>(ref savedGames);
                RegisterCollection<UserSettings, UserSettingsCollection>(ref userSettings);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UserDatabase) + ".";

        private static readonly ProfilerMarker _PRF_OnInitialize =
            new ProfilerMarker(_PRF_PFX + nameof(OnInitialize));

        private static readonly ProfilerMarker _PRF_Dispose = new ProfilerMarker(_PRF_PFX + nameof(Dispose));

        #endregion
    }
}
