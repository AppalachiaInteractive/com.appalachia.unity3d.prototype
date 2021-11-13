using Appalachia.Utility.Logging;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Areas.MainMenu.SubAreas.LoadGame
{
    public class LoadGameManager : SubAreaManager<LoadGameManager, LoadGameMetadata, MainMenuSubArea,
        MainMenuManager, MainMenuMetadata>
    {
        #region Profiling

        private const string _PRF_PFX = nameof(LoadGameManager) + ".";

        private static readonly ProfilerMarker _PRF_NewGame = new ProfilerMarker(_PRF_PFX + nameof(NewGame));

        private static readonly ProfilerMarker
            _PRF_LoadGame = new ProfilerMarker(_PRF_PFX + nameof(LoadGame));

        private static readonly ProfilerMarker
            _PRF_Activate = new ProfilerMarker(_PRF_PFX + nameof(Activate));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(Deactivate));

        private static readonly ProfilerMarker _PRF_OnReset = new ProfilerMarker(_PRF_PFX + nameof(OnReset));

        #endregion

        public override MainMenuSubArea SubArea => MainMenuSubArea.LoadGame;

        public override void Activate()
        {
            using (_PRF_Activate.Auto())
            {
                AppaLog.Context.SubArea.Info(nameof(Activate));
            }
        }

        public override void Deactivate()
        {
            using (_PRF_Deactivate.Auto())
            {
                AppaLog.Context.SubArea.Info(nameof(Deactivate));
            }
        }

        protected override void OnReset(bool resetting)
        {
            using (_PRF_OnReset.Auto())
            {
                AppaLog.Context.SubArea.Info(nameof(OnReset));
            }
        }
    }
}
