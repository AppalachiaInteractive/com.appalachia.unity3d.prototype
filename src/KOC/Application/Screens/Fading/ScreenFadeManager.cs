using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Screens.Fading
{
    public class ScreenFadeManager : FadeManager<ScreenFadeManager>
    {
        #region Fields and Autoproperties

        public Image FullScreenBlack;

        #endregion

        protected override void ExecuteFade(float time)
        {
            using (_PRF_ExecuteFade.Auto())
            {
                var color = FullScreenBlack.color;
                color.a = Mathf.Clamp01(1 - time);
                FullScreenBlack.color = color;
            }
        }

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                if (FullScreenBlack == null)
                {
                    gameObject.CreateOrGetComponentInChild(ref FullScreenBlack, "Image - FullScreenBlack");
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ScreenFadeManager) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_ExecuteFade =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteFade));

        #endregion
    }
}
