namespace Appalachia.Prototype.KOC.Application.Areas.MainMenu_Settings
{
    public abstract class MainMenu_SettingsMetadata<T, TM> : AreaMetadata<T, TM>, IMainMenu_SettingsMetadata
        where T : MainMenu_SettingsManager<T, TM>
        where TM : MainMenu_SettingsMetadata<T, TM>
    {
        #region IMainMenu_SettingsMetadata Members

        public override ApplicationArea Area => ApplicationArea.MainMenu_Settings;

        #endregion
    }
}
