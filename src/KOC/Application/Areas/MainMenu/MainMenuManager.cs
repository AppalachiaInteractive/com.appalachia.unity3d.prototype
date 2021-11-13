using Appalachia.Utility.Logging;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Areas.MainMenu
{
    public class MainMenuManager : AreaManager<MainMenuManager, MainMenuMetadata>
    {
        #region Profiling

        private const string _PRF_PFX = nameof(MainMenuManager) + ".";

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        private static readonly ProfilerMarker _PRF_NewGame = new ProfilerMarker(_PRF_PFX + nameof(NewGame));

        private static readonly ProfilerMarker
            _PRF_LoadGame = new ProfilerMarker(_PRF_PFX + nameof(LoadGame));

        private static readonly ProfilerMarker
            _PRF_Settings = new ProfilerMarker(_PRF_PFX + nameof(Settings));

        private static readonly ProfilerMarker _PRF_Quit = new ProfilerMarker(_PRF_PFX + nameof(Quit));

        private static readonly ProfilerMarker
            _PRF_Activate = new ProfilerMarker(_PRF_PFX + nameof(Activate));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(Deactivate));

        #endregion

        public override ApplicationArea Area => ApplicationArea.MainMenu;

        public override void Activate()
        {
            using (_PRF_Activate.Auto())
            {
                AppaLog.Context.Area.Info(nameof(Activate));
            }
        }

        public override void Deactivate()
        {
            using (_PRF_Deactivate.Auto())
            {
                AppaLog.Context.Area.Info(nameof(Deactivate));
            }
        }

        public void LoadGame()
        {
            using (_PRF_LoadGame.Auto())
            {
                AppaLog.Context.Area.Info(nameof(LoadGame));
            }
        }

        public void NewGame()
        {
            using (_PRF_NewGame.Auto())
            {
                AppaLog.Context.Area.Info(nameof(NewGame));
            }
        }

        public void Quit()
        {
            using (_PRF_Quit.Auto())
            {
                AppaLog.Context.Area.Info(nameof(Quit));
            }
        }

        public void Settings()
        {
            using (_PRF_Settings.Auto())
            {
                AppaLog.Context.Area.Info(nameof(Settings));
            }
        }

        protected override void ResetArea()
        {
            using (_PRF_ResetArea.Auto())
            {
                AppaLog.Context.Area.Info(nameof(ResetArea));
            }
        }
    }
}
