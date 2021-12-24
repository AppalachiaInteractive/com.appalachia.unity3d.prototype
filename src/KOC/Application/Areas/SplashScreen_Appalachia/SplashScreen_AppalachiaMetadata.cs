using Appalachia.Prototype.KOC.Application.Areas.SplashScreen.Base;

namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen_Appalachia
{
    public abstract class SplashScreen_AppalachiaMetadata<T, TM> : SplashScreenSubMetadata<T, TM>,
                                                                   ISplashScreen_AppalachiaMetadata
        where T : SplashScreen_AppalachiaManager<T, TM>
        where TM : SplashScreen_AppalachiaMetadata<T, TM>
    {
        #region ISplashScreen_AppalachiaMetadata Members

        public override ApplicationArea Area => ApplicationArea.SplashScreen_Appalachia;

        #endregion
    }
}
