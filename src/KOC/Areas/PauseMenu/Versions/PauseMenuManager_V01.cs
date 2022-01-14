using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.PauseMenu.Versions
{
    public class PauseMenuManager_V01 : PauseMenuManager<PauseMenuManager_V01, PauseMenuMetadata_V01>
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
