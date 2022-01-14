using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.MainMenu_Settings.Versions
{
    public class MainMenu_SettingsManager_V01 : MainMenu_SettingsManager<MainMenu_SettingsManager_V01,
        MainMenu_SettingsMetadata_V01>
    {
        public override AreaVersion Version => AreaVersion.V01;

        protected override async AppaTask SetFeaturesToInitialState()
        {
            await AppaTask.CompletedTask;
        }

        protected override async AppaTask SetServicesToInitialState()
        {
            await AppaTask.CompletedTask;
        }

        protected override async AppaTask SetWidgetsToInitialState()
        {
            await AppaTask.CompletedTask;
        }
    }
}
