using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.HUD.Versions
{
    public class HUDManager_V01 : HUDManager<HUDManager_V01, HUDMetadata_V01>
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
