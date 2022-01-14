using Appalachia.Prototype.KOC.Areas.SplashScreen.Base;

namespace Appalachia.Prototype.KOC.Areas.SplashScreen_Appalachia
{
    public abstract class
        SplashScreen_AppalachiaMetadata<TManager, TMetadata> : SplashScreenSubMetadata<TManager, TMetadata>,
                                                               ISplashScreen_AppalachiaMetadata
        where TManager : SplashScreen_AppalachiaManager<TManager, TMetadata>
        where TMetadata : SplashScreen_AppalachiaMetadata<TManager, TMetadata>
    {
        #region ISplashScreen_AppalachiaMetadata Members

        public override ApplicationArea Area => ApplicationArea.SplashScreen_Appalachia;

        #endregion
    }
}
