namespace Appalachia.Prototype.KOC.Application.Areas.MainMenu
{
    public abstract class MainMenuMetadata<T, TM> : AreaMetadata<T, TM>, IMainMenuMetadata
        where T : MainMenuManager<T, TM>
        where TM : MainMenuMetadata<T, TM>
    {
        #region IMainMenuMetadata Members

        public override ApplicationArea Area => ApplicationArea.MainMenu;

        #endregion
    }
}
