using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Areas.MainMenu_NewGame
{
    public abstract class MainMenu_NewGameManager<T, TM> : AreaManager<T, TM>, IMainMenu_NewGameManager
        where T : MainMenu_NewGameManager<T, TM>
        where TM : MainMenu_NewGameMetadata<T, TM>
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

        #region IMainMenu_NewGameManager Members

        public override ApplicationArea Area => ApplicationArea.MainMenu_NewGame;
        public override ApplicationArea ParentArea => ApplicationArea.MainMenu;

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(MainMenu_NewGameManager<T, TM>) + ".";

        private static readonly ProfilerMarker _PRF_Activate =
            new ProfilerMarker(_PRF_PFX + nameof(OnActivation));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeactivation));

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        #endregion
    }
}
