namespace Appalachia.Prototype.KOC.Areas.MainMenu_Settings
{
    public abstract class MainMenu_SettingsMetadata<TManager, TMetadata> : AreaMetadata<TManager, TMetadata>,
        IMainMenu_SettingsMetadata
        where TManager : MainMenu_SettingsManager<TManager, TMetadata>
        where TMetadata : MainMenu_SettingsMetadata<TManager, TMetadata>
    {
        #region IMainMenu_SettingsMetadata Members

        public override ApplicationArea Area => ApplicationArea.MainMenu_Settings;

        #endregion
    }
}
