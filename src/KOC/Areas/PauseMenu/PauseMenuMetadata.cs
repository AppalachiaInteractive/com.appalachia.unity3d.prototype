namespace Appalachia.Prototype.KOC.Areas.PauseMenu
{
    public abstract class PauseMenuMetadata<TManager, TMetadata> : AreaMetadata<TManager, TMetadata>,
                                                                   IPauseMenuMetadata
        where TManager : PauseMenuManager<TManager, TMetadata>
        where TMetadata : PauseMenuMetadata<TManager, TMetadata>
    {
        #region IPauseMenuMetadata Members

        /// <inheritdoc />
        public override ApplicationArea Area => ApplicationArea.PauseMenu;

        #endregion
    }
}
