using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.LoadingScreen.Versions
{
    public class
        LoadingScreenManager_V01 : LoadingScreenManager<LoadingScreenManager_V01, LoadingScreenMetadata_V01>
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
