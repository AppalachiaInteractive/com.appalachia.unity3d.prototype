using Appalachia.Prototype.KOC.Areas.SplashScreen.Base;

namespace Appalachia.Prototype.KOC.Areas.SplashScreen_Disclaimer
{
    public abstract class
        SplashScreen_DisclaimerManager<TManager, TMetadata> : SplashScreenSubManager<TManager, TMetadata>,
                                                              ISplashScreen_DisclaimerManager
        where TManager : SplashScreen_DisclaimerManager<TManager, TMetadata>
        where TMetadata : SplashScreen_DisclaimerMetadata<TManager, TMetadata>
    {
        #region ISplashScreen_DisclaimerManager Members

        public override ApplicationArea Area => ApplicationArea.SplashScreen_Disclaimer;

        #endregion
    }
}
