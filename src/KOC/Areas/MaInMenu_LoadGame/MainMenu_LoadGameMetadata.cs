namespace Appalachia.Prototype.KOC.Areas.MaInMenu_LoadGame
{
    public abstract class MainMenu_LoadGameMetadata<TManager, TMetadata> : AreaMetadata<TManager, TMetadata>,
        IMainMenu_LoadGameMetadata
        where TManager : MainMenu_LoadGameManager<TManager, TMetadata>
        where TMetadata : MainMenu_LoadGameMetadata<TManager, TMetadata>
    {
        #region IMainMenu_LoadGameMetadata Members

        public override ApplicationArea Area => ApplicationArea.MainMenu_LoadGame;

        #endregion
    }
}
