using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.StartScreen
{
    public abstract class StartScreenManager<TManager, TMetadata> : AreaManager<TManager, TMetadata>,
                                                                    IStartScreenManager
        where TManager : StartScreenManager<TManager, TMetadata>
        where TMetadata : StartScreenMetadata<TManager, TMetadata>
    {
        public void Continue()
        {
            using (_PRF_Continue.Auto())
            {
                Context.Log.Info(nameof(Continue), this);
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

        #region IStartScreenManager Members

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.StartScreen;

        /// <inheritdoc />
        public override ApplicationArea ParentArea => ApplicationArea.None;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker
            _PRF_Continue = new ProfilerMarker(_PRF_PFX + nameof(Continue));

        #endregion
    }
}