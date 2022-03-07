namespace Appalachia.Prototype.KOC.Areas.InGameMenu
{
    public abstract class InGameMenuManager<TManager, TMetadata> : AreaManager<TManager, TMetadata>,
                                                                   IInGameMenuManager
        where TManager : InGameMenuManager<TManager, TMetadata>
        where TMetadata : InGameMenuMetadata<TManager, TMetadata>
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

        #region IInGameMenuManager Members

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.InGameMenu;

        /// <inheritdoc />
        public override ApplicationArea ParentArea => ApplicationArea.None;

        #endregion
    }
}