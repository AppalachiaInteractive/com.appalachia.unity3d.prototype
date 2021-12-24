using Appalachia.Prototype.KOC.Application.Areas.SplashScreen.Base;

namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen_BokBok
{
    public abstract class SplashScreen_BokBokMetadata<T, TM> : SplashScreenSubMetadata<T, TM>,
                                                               ISplashScreen_BokBokMetadata
        where T : SplashScreen_BokBokManager<T, TM>
        where TM : SplashScreen_BokBokMetadata<T, TM>
    {
        #region ISplashScreen_BokBokMetadata Members

        public override ApplicationArea Area => ApplicationArea.SplashScreen_BokBok;

        #endregion
    }
}
