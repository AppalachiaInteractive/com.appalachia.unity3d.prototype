using Appalachia.Prototype.KOC.Areas.SplashScreen.Base;

namespace Appalachia.Prototype.KOC.Areas.SplashScreen_Appalachia
{
    public abstract class
        SplashScreen_AppalachiaManager<TManager, TMetadata> : SplashScreenSubManager<TManager, TMetadata>,
                                                              ISplashScreen_AppalachiaManager
        where TManager : SplashScreen_AppalachiaManager<TManager, TMetadata>
        where TMetadata : SplashScreen_AppalachiaMetadata<TManager, TMetadata>
    {
        #region ISplashScreen_AppalachiaManager Members

        public override ApplicationArea Area => ApplicationArea.SplashScreen_Appalachia;

        #endregion
    }
}
