using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.SplashScreen_BokBok.Versions
{
    public class SplashScreen_BokBokManager_V02 : SplashScreen_BokBokManager<SplashScreen_BokBokManager_V02,
        SplashScreen_BokBokMetadata_V02>
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
