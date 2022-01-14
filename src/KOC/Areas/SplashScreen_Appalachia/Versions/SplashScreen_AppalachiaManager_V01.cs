using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.SplashScreen_Appalachia.Versions
{
    public class SplashScreen_AppalachiaManager_V01 : SplashScreen_AppalachiaManager<
        SplashScreen_AppalachiaManager_V01, SplashScreen_AppalachiaMetadata_V01>
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
