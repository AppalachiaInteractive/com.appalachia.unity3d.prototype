using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.InGameMenu
{
    public abstract class InGameMenuManager<TManager, TMetadata> : AreaManager<TManager, TMetadata>,
                                                                   IInGameMenuManager
        where TManager : InGameMenuManager<TManager, TMetadata>
        where TMetadata : InGameMenuMetadata<TManager, TMetadata>
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

        #region IInGameMenuManager Members

        public override ApplicationArea Area => ApplicationArea.InGameMenu;
        public override ApplicationArea ParentArea => ApplicationArea.None;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_Activate =
            new ProfilerMarker(_PRF_PFX + nameof(OnActivation));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeactivation));

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        #endregion
    }
}
