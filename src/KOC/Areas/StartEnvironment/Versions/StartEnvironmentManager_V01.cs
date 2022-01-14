using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.StartEnvironment.Versions
{
    public class StartEnvironmentManager_V01 : StartEnvironmentManager<StartEnvironmentManager_V01,
        StartEnvironmentMetadata_V01>
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
