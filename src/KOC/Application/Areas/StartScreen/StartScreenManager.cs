using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Areas.StartScreen
{
    public abstract class StartScreenManager<T, TM> : AreaManager<T, TM>, IStartScreenManager
        where T : StartScreenManager<T, TM>
        where TM : StartScreenMetadata<T, TM>
    {
        public void Continue()
        {
            using (_PRF_Continue.Auto())
            {
                Context.Log.Info(nameof(Continue), this);
            }
        }

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

        #region IStartScreenManager Members

        public override ApplicationArea Area => ApplicationArea.StartScreen;
        public override ApplicationArea ParentArea => ApplicationArea.None;

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(StartScreenManager<T, TM>) + ".";

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        private static readonly ProfilerMarker _PRF_Activate =
            new ProfilerMarker(_PRF_PFX + nameof(OnActivation));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeactivation));

        private static readonly ProfilerMarker
            _PRF_Continue = new ProfilerMarker(_PRF_PFX + nameof(Continue));

        #endregion
    }
}
