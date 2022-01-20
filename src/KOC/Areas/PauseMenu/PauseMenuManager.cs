namespace Appalachia.Prototype.KOC.Areas.PauseMenu
{
    public abstract class PauseMenuManager<TManager, TMetadata> : AreaManager<TManager, TMetadata>,
                                                                  IPauseMenuManager
        where TManager : PauseMenuManager<TManager, TMetadata>
        where TMetadata : PauseMenuMetadata<TManager, TMetadata>
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

        #region IPauseMenuManager Members

        public override ApplicationArea Area => ApplicationArea.PauseMenu;
        public override ApplicationArea ParentArea => ApplicationArea.None;

        #endregion
    }
}
