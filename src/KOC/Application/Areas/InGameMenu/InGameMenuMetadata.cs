namespace Appalachia.Prototype.KOC.Application.Areas.InGameMenu
{
    public abstract class InGameMenuMetadata<T, TM> : AreaMetadata<T, TM>, IInGameMenuMetadata
        where T : InGameMenuManager<T, TM>
        where TM : InGameMenuMetadata<T, TM>
    {
        #region IInGameMenuMetadata Members

        public override ApplicationArea Area => ApplicationArea.InGameMenu;

        #endregion
    }
}
