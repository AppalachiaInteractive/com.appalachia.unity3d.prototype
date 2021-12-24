namespace Appalachia.Prototype.KOC.Application.Areas.MainMenu_NewGame
{
    public abstract class MainMenu_NewGameMetadata<T, TM> : AreaMetadata<T, TM>, IMainMenu_NewGameMetadata
        where T : MainMenu_NewGameManager<T, TM>
        where TM : MainMenu_NewGameMetadata<T, TM>
    {
        #region IMainMenu_NewGameMetadata Members

        public override ApplicationArea Area => ApplicationArea.MainMenu_NewGame;

        #endregion
    }
}
