using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Screens.Fading
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasFadeManager : FadeManager<CanvasFadeManager>
    {
        #region Fields and Autoproperties

        public CanvasGroup menuCanvasGroup;

        #endregion

        #region Event Functions

        protected override void Awake()
        {
            base.Awake();

            this.GetOrCreateComponent(ref menuCanvasGroup);
        }

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                var alpha = menuCanvasGroup.alpha;

                if (alpha < fadeSettings.minimumAlpha)
                {
                    menuCanvasGroup.alpha = fadeSettings.minimumAlpha;
                }
                else if (alpha > fadeSettings.maximumAlpha)
                {
                    menuCanvasGroup.alpha = fadeSettings.maximumAlpha;
                }
            }
        }

        #endregion

        protected override void ExecuteFade(float time)
        {
            menuCanvasGroup.alpha = Mathf.Clamp01(time);
        }

        #region Profiling

        private const string _PRF_PFX = nameof(CanvasFadeManager) + ".";
        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(Update));

        #endregion
    }
}
