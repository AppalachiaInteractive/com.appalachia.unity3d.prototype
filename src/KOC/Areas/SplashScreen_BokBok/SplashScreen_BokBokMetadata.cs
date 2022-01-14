using Appalachia.Prototype.KOC.Areas.SplashScreen.Base;

namespace Appalachia.Prototype.KOC.Areas.SplashScreen_BokBok
{
    public abstract class
        SplashScreen_BokBokMetadata<TManager, TMetadata> : SplashScreenSubMetadata<TManager, TMetadata>,
                                                           ISplashScreen_BokBokMetadata
        where TManager : SplashScreen_BokBokManager<TManager, TMetadata>
        where TMetadata : SplashScreen_BokBokMetadata<TManager, TMetadata>
    {
        #region ISplashScreen_BokBokMetadata Members

        public override ApplicationArea Area => ApplicationArea.SplashScreen_BokBok;

        #endregion
    }
}
