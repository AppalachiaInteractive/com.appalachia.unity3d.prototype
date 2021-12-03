using Appalachia.Prototype.KOC.Application.Areas.SplashScreen.Base;

namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen.Appalachia
{
    public class AppalachiaSplashScreenManager : SplashScreenSubManager<AppalachiaSplashScreenManager,
        AppalachiaSplashScreenMetadata>
    {
        public override ApplicationArea Area => ApplicationArea.SplashScreen_Appalachia;
    }
}
