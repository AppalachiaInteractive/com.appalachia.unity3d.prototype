namespace Appalachia.Prototype.KOC.Application.Areas.MaInMenu_LoadGame
{
    public abstract class MainMenu_LoadGameMetadata<T, TM> : AreaMetadata<T, TM>, IMainMenu_LoadGameMetadata
        where T : MainMenu_LoadGameManager<T, TM>
        where TM : MainMenu_LoadGameMetadata<T, TM>
    {
        #region IMainMenu_LoadGameMetadata Members

        public override ApplicationArea Area => ApplicationArea.MainMenu_LoadGame;

        #endregion
    }
}
