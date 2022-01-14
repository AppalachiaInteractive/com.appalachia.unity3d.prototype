namespace Appalachia.Prototype.KOC.Areas.InGameMenu
{
    public abstract class InGameMenuMetadata<TManager, TMetadata> : AreaMetadata<TManager, TMetadata>,
                                                                    IInGameMenuMetadata
        where TManager : InGameMenuManager<TManager, TMetadata>
        where TMetadata : InGameMenuMetadata<TManager, TMetadata>
    {
        #region IInGameMenuMetadata Members

        public override ApplicationArea Area => ApplicationArea.InGameMenu;

        #endregion
    }
}
