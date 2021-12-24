namespace Appalachia.Prototype.KOC.Application.Areas.PauseMenu
{
    public abstract class PauseMenuMetadata<T, TM> : AreaMetadata<T, TM>, IPauseMenuMetadata
        where T : PauseMenuManager<T, TM>
        where TM : PauseMenuMetadata<T, TM>
    {
        #region IPauseMenuMetadata Members

        public override ApplicationArea Area => ApplicationArea.PauseMenu;

        #endregion
    }
}
