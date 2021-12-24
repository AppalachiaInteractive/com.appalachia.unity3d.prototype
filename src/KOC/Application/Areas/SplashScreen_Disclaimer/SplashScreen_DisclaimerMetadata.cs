using Appalachia.Prototype.KOC.Application.Areas.SplashScreen.Base;

namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen_Disclaimer
{
    public abstract class SplashScreen_DisclaimerMetadata<T, TM> : SplashScreenSubMetadata<T, TM>,
                                                                   ISplashScreen_DisclaimerMetadata
        where T : SplashScreen_DisclaimerManager<T, TM>
        where TM : SplashScreen_DisclaimerMetadata<T, TM>
    {
        #region ISplashScreen_DisclaimerMetadata Members

        public override ApplicationArea Area => ApplicationArea.SplashScreen_Disclaimer;

        #endregion
    }
}
