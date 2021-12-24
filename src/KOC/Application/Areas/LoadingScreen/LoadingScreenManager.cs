using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Areas.LoadingScreen
{
    public abstract class LoadingScreenManager<T, TM> : AreaManager<T, TM>, ILoadingScreenManager
        where T : LoadingScreenManager<T, TM>
        where TM : LoadingScreenMetadata<T, TM>
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

        #region ILoadingScreenManager Members

        public override ApplicationArea Area => ApplicationArea.LoadingScreen;
        public override ApplicationArea ParentArea => ApplicationArea.None;

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(LoadingScreenManager<T, TM>) + ".";

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        private static readonly ProfilerMarker _PRF_Activate =
            new ProfilerMarker(_PRF_PFX + nameof(OnActivation));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeactivation));

        #endregion
    }
}
