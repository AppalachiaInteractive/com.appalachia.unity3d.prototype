using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.MainMenu.Versions
{
    public class MainMenuManager_V01 : MainMenuManager<MainMenuManager_V01, MainMenuMetadata_V01>
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
