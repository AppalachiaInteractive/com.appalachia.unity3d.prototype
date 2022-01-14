using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.StartScreen.Versions
{
    public class StartScreenManager_V01 : StartScreenManager<StartScreenManager_V01, StartScreenMetadata_V01>
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
