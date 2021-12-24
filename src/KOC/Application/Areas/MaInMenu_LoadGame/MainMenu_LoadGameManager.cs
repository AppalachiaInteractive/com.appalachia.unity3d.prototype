using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Areas.MaInMenu_LoadGame
{
    public abstract class MainMenu_LoadGameManager<T, TM> : AreaManager<T, TM>, IMainMenu_LoadGameManager
        where T : MainMenu_LoadGameManager<T, TM>
        where TM : MainMenu_LoadGameMetadata<T, TM>
    {
        protected override void OnActivation()
        {
            using (_PRF_Activate.Auto())
            {
                Context.Log.Info(nameof(OnActivation), this);
            }
        }

        protected override void OnDeactivation()
        {
            using (_PRF_Deactivate.Auto())
            {
                Context.Log.Info(nameof(OnDeactivation), this);
            }
        }

        protected override void ResetArea()
        {
            using (_PRF_ResetArea.Auto())
            {
                Context.Log.Info(nameof(ResetArea), this);
            }
        }

        #region IMainMenu_LoadGameManager Members

        public override ApplicationArea Area => ApplicationArea.MainMenu_LoadGame;
        public override ApplicationArea ParentArea => ApplicationArea.MainMenu;

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(MainMenu_LoadGameManager<T, TM>) + ".";

        private static readonly ProfilerMarker _PRF_Activate =
            new ProfilerMarker(_PRF_PFX + nameof(OnActivation));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeactivation));

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        #endregion
    }
}
