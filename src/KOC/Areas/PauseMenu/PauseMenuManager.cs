namespace Appalachia.Prototype.KOC.Areas.PauseMenu
{
    public abstract class PauseMenuManager<TManager, TMetadata> : AreaManager<TManager, TMetadata>,
                                                                  IPauseMenuManager
        where TManager : PauseMenuManager<TManager, TMetadata>
        where TMetadata : PauseMenuMetadata<TManager, TMetadata>
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

        #region IPauseMenuManager Members

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.PauseMenu;

        /// <inheritdoc />
        public override ApplicationArea ParentArea => ApplicationArea.None;

        #endregion
    }
}
