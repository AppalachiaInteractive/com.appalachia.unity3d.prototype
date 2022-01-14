using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.InGameMenu.Versions
{
    public class InGameMenuManager_V01 : InGameMenuManager<InGameMenuManager_V01, InGameMenuMetadata_V01>
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
