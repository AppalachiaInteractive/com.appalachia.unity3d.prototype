using Appalachia.Prototype.KOC.Application.Areas.SplashScreen.Base;

namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen_BokBok
{
    public abstract class SplashScreen_BokBokManager<T, TM> : SplashScreenSubManager<T, TM>,
                                                              ISplashScreen_BokBokManager
        where T : SplashScreen_BokBokManager<T, TM>
        where TM : SplashScreen_BokBokMetadata<T, TM>
    {
        #region ISplashScreen_BokBokManager Members

        public override ApplicationArea Area => ApplicationArea.SplashScreen_BokBok;

        #endregion
    }
}
