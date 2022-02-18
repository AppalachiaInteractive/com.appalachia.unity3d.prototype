namespace Appalachia.Prototype.KOC.Areas.StartEnvironment
{
    public abstract class StartEnvironmentManager<TManager, TMetadata> : AreaManager<TManager, TMetadata>,
        IStartEnvironmentManager
        where TManager : StartEnvironmentManager<TManager, TMetadata>
        where TMetadata : StartEnvironmentMetadata<TManager, TMetadata>
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

        #region IStartEnvironmentManager Members

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.StartEnvironment;

        /// <inheritdoc />
        public override ApplicationArea ParentArea => ApplicationArea.None;

        #endregion
    }
}
