using Appalachia.Prototype.KOC.Application.Areas.SplashScreen.Base;

namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen.BokBok
{
    public class
        BokBokSplashScreenManager : SplashScreenSubManager<BokBokSplashScreenManager,
            BokBokSplashScreenMetadata>
    {
        public override ApplicationArea Area => ApplicationArea.SplashScreen_BokBok;
    }
}
