namespace Appalachia.Prototype.KOC.Areas.MainMenu_NewGame
{
    public abstract class MainMenu_NewGameMetadata<TManager, TMetadata> : AreaMetadata<TManager, TMetadata>,
        IMainMenu_NewGameMetadata
        where TManager : MainMenu_NewGameManager<TManager, TMetadata>
        where TMetadata : MainMenu_NewGameMetadata<TManager, TMetadata>
    {
        #region IMainMenu_NewGameMetadata Members

        public override ApplicationArea Area => ApplicationArea.MainMenu_NewGame;

        #endregion
    }
}
