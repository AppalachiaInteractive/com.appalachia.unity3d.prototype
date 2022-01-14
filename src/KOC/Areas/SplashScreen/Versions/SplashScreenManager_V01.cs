using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.SplashScreen.Versions
{
    public class
        SplashScreenManager_V01 : SplashScreenManager<SplashScreenManager_V01, SplashScreenMetadata_V01>
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
