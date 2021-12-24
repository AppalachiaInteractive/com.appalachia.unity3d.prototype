using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Areas.StartEnvironment
{
    public abstract class StartEnvironmentManager<T, TM> : AreaManager<T, TM>, IStartEnvironmentManager
        where T : StartEnvironmentManager<T, TM>
        where TM : StartEnvironmentMetadata<T, TM>
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

        #region IStartEnvironmentManager Members

        public override ApplicationArea Area => ApplicationArea.StartEnvironment;
        public override ApplicationArea ParentArea => ApplicationArea.None;

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(StartEnvironmentManager<T, TM>) + ".";

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        private static readonly ProfilerMarker _PRF_Activate =
            new ProfilerMarker(_PRF_PFX + nameof(OnActivation));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeactivation));

        #endregion
    }
}
