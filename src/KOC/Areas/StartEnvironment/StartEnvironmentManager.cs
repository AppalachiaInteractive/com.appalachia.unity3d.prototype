namespace Appalachia.Prototype.KOC.Areas.StartEnvironment
{
    public abstract class StartEnvironmentManager<TManager, TMetadata> : AreaManager<TManager, TMetadata>,
        IStartEnvironmentManager
        where TManager : StartEnvironmentManager<TManager, TMetadata>
        where TMetadata : StartEnvironmentMetadata<TManager, TMetadata>
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
    }
}
