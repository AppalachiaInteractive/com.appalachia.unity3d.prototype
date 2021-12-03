using Appalachia.Prototype.KOC.Application.Areas.SplashScreen.Base;

namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen.Disclaimer
{
    public class DisclaimerSplashScreenManager : SplashScreenSubManager<DisclaimerSplashScreenManager,
        DisclaimerSplashScreenMetadata>
    {
        public override ApplicationArea Area => ApplicationArea.SplashScreen_Disclaimer;
    }
}
