namespace Appalachia.Prototype.KOC.Areas.LoadingScreen
{
    public abstract class LoadingScreenManager<TManager, TMetadata> : AreaManager<TManager, TMetadata>,
                                                                      ILoadingScreenManager
        where TManager : LoadingScreenManager<TManager, TMetadata>
        where TMetadata : LoadingScreenMetadata<TManager, TMetadata>
    {
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

        #region ILoadingScreenManager Members

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.LoadingScreen;

        /// <inheritdoc />
        public override ApplicationArea ParentArea => ApplicationArea.None;

        #endregion
    }
}
