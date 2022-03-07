using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.MainMenu
{
    public abstract class MainMenuManager<TManager, TMetadata> : AreaManager<TManager, TMetadata>,
                                                                 IMainMenuManager
        where TManager : MainMenuManager<TManager, TMetadata>
        where TMetadata : MainMenuMetadata<TManager, TMetadata>
    {
        public void LoadGame()
        {
            using (_PRF_LoadGame.Auto())
            {
                Context.Log.Info(nameof(LoadGame), this);
            }
        }

        public void NewGame()
        {
            using (_PRF_NewGame.Auto())
            {
                Context.Log.Info(nameof(NewGame), this);
            }
        }

        public void Quit()
        {
            using (_PRF_Quit.Auto())
            {
                Context.Log.Info(nameof(Quit), this);
            }
        }

        public void Settings()
        {
            using (_PRF_Settings.Auto())
            {
                Context.Log.Info(nameof(Settings), this);
            }
        }

        /// <inheritdoc />
        protected override void OnActivation()
        {
            using (_PRF_Activate.Auto())
            {
                Context.Log.Info(nameof(OnActivation), this);
            }
        }

        /// <inheritdoc />
        protected override void OnDeactivation()
        {
            using (_PRF_Deactivate.Auto())
            {
                Context.Log.Info(nameof(OnDeactivation), this);
            }
        }

        /// <inheritdoc />
        protected override void ResetArea()
        {
            using (_PRF_ResetArea.Auto())
            {
                Context.Log.Info(nameof(ResetArea), this);
            }
        }

        #region IMainMenuManager Members

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.MainMenu;

        /// <inheritdoc />
        public override ApplicationArea ParentArea => ApplicationArea.None;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker
            _PRF_LoadGame = new ProfilerMarker(_PRF_PFX + nameof(LoadGame));

        private static readonly ProfilerMarker _PRF_NewGame = new ProfilerMarker(_PRF_PFX + nameof(NewGame));

        private static readonly ProfilerMarker _PRF_Quit = new ProfilerMarker(_PRF_PFX + nameof(Quit));

        private static readonly ProfilerMarker
            _PRF_Settings = new ProfilerMarker(_PRF_PFX + nameof(Settings));

        #endregion
    }
}