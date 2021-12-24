using Appalachia.Prototype.KOC.Application.Input;
using Unity.Profiling;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen_Disclaimer.Versions
{
    public class SplashScreen_DisclaimerManager_V01 : SplashScreen_DisclaimerManager<
                                                          SplashScreen_DisclaimerManager_V01,
                                                          SplashScreen_DisclaimerMetadata_V01>,
                                                      KOCInputActions.ISplashScreenActions
    {
        #region Fields and Autoproperties

        private bool _enableAcknowledgement;

        #endregion

        public override AreaVersion Version => AreaVersion.V01;

        public void OnTimelinePause()
        {
            using (_PRF_OnTimelinePause.Auto())
            {
                Context.Log.Info(nameof(OnTimelinePause), this);

                _enableAcknowledgement = true;
            }
        }

        #region ISplashScreenActions Members

        public override void OnContinue(InputAction.CallbackContext context)
        {
            using (_PRF_OnContinue.Auto())
            {
                base.OnContinue(context);

                if (_enableAcknowledgement)
                {
                    _playableDirector.Resume();
                }
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(SplashScreen_DisclaimerManager_V01) + ".";

        private static readonly ProfilerMarker _PRF_OnContinue =
            new ProfilerMarker(_PRF_PFX + nameof(OnContinue));

        private static readonly ProfilerMarker _PRF_OnTimelinePause =
            new ProfilerMarker(_PRF_PFX + nameof(OnTimelinePause));

        #endregion
    }
}
