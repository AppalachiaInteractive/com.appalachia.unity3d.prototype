namespace Appalachia.Prototype.KOC.Areas.HUD
{
    public abstract class HUDManager<TManager, TMetadata> : AreaManager<TManager, TMetadata>, IHUDManager
        where TManager : HUDManager<TManager, TMetadata>
        where TMetadata : HUDMetadata<TManager, TMetadata>
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

        #region IHUDManager Members

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.HUD;

        /// <inheritdoc />
        public override ApplicationArea ParentArea => ApplicationArea.None;

        #endregion
    }
}