using Appalachia.Prototype.KOC.Areas.SplashScreen.Base;

namespace Appalachia.Prototype.KOC.Areas.SplashScreen_BokBok
{
    public abstract class
        SplashScreen_BokBokManager<TManager, TMetadata> : SplashScreenSubManager<TManager, TMetadata>,
                                                          ISplashScreen_BokBokManager
        where TManager : SplashScreen_BokBokManager<TManager, TMetadata>
        where TMetadata : SplashScreen_BokBokMetadata<TManager, TMetadata>
    {
        #region ISplashScreen_BokBokManager Members

        public override ApplicationArea Area => ApplicationArea.SplashScreen_BokBok;

        #endregion
    }
}
