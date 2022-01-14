namespace Appalachia.Prototype.KOC.Areas.MainMenu
{
    public abstract class MainMenuMetadata<TManager, TMetadata> : AreaMetadata<TManager, TMetadata>,
                                                                  IMainMenuMetadata
        where TManager : MainMenuManager<TManager, TMetadata>
        where TMetadata : MainMenuMetadata<TManager, TMetadata>
    {
        #region IMainMenuMetadata Members

        public override ApplicationArea Area => ApplicationArea.MainMenu;

        #endregion
    }
}
