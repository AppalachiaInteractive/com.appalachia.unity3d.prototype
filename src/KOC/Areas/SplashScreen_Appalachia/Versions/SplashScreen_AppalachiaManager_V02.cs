using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.SplashScreen_Appalachia.Versions
{
    public class SplashScreen_AppalachiaManager_V02 : SplashScreen_AppalachiaManager<
        SplashScreen_AppalachiaManager_V02, SplashScreen_AppalachiaMetadata_V02>
    {
        public override AreaVersion Version => AreaVersion.V02;

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
