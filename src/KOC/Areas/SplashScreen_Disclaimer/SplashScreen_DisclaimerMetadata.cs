using Appalachia.Prototype.KOC.Areas.SplashScreen.Base;

namespace Appalachia.Prototype.KOC.Areas.SplashScreen_Disclaimer
{
    public abstract class
        SplashScreen_DisclaimerMetadata<TManager, TMetadata> : SplashScreenSubMetadata<TManager, TMetadata>,
                                                               ISplashScreen_DisclaimerMetadata
        where TManager : SplashScreen_DisclaimerManager<TManager, TMetadata>
        where TMetadata : SplashScreen_DisclaimerMetadata<TManager, TMetadata>
    {
        #region ISplashScreen_DisclaimerMetadata Members

        public override ApplicationArea Area => ApplicationArea.SplashScreen_Disclaimer;

        #endregion
    }
}
