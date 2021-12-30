using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Application.Components.Fading
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Canvas))]
    public class CanvasFadeManager : FadeManager<CanvasFadeManager>
    {
        #region Fields and Autoproperties

        public Canvas canvas;

        [FormerlySerializedAs("menuCanvasGroup")]
        public CanvasGroup canvasGroup;

        private RectTransform m_RectTransform;

        private bool _isCanvasGroupInteractable;

        #endregion

        /// <summary>
        ///     The RectTransform component used by the Graphic. Cached for speed.
        /// </summary>
        public RectTransform rectTransform
        {
            get
            {
                gameObject.GetOrCreateComponent(ref m_RectTransform);

                return m_RectTransform;
            }
        }

        #region Event Functions

        protected override void Update()
        {
            using (_PRF_Update.Auto())
            {
                base.Update();

                try
                {
                    var alpha = canvasGroup.alpha;

                    if (alpha < fadeSettings.minimumAlpha)
                    {
                        canvasGroup.alpha = fadeSettings.minimumAlpha;
                    }
                    else if (alpha > fadeSettings.maximumAlpha)
                    {
                        canvasGroup.alpha = fadeSettings.maximumAlpha;
                    }
                }
                catch (UnassignedReferenceException)
                {
                    gameObject.GetOrCreateComponent(ref canvasGroup);
                }
            }
        }

        #endregion

        protected override void ExecuteFade(float time)
        {
            using (_PRF_ExecuteFade.Auto())
            {
                canvasGroup.alpha = Mathf.Clamp01(time);
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                gameObject.GetOrCreateComponent(ref canvasGroup);
                gameObject.GetOrCreateComponent(ref canvas);

                _isCanvasGroupInteractable = canvasGroup.interactable;

                FadeOutStarted += OnFadeOutStarted;
                FadeOutCompleted += OnFadeOutCompleted;

                FadeInStarted += OnFadeInStarted;
                FadeInCompleted += OnFadeInCompleted;
            }
        }

        private void OnFadeInCompleted()
        {
            using (_PRF_OnFadeInCompleted.Auto())
            {
            }
        }

        private void OnFadeInStarted()
        {
            using (_PRF_OnFadeInStarted.Auto())
            {
                canvas.enabled = true;
                canvasGroup.interactable = _isCanvasGroupInteractable;
            }
        }

        private void OnFadeOutCompleted()
        {
            using (_PRF_OnFadeOutCompleted.Auto())
            {
                canvas.enabled = false;
                canvasGroup.interactable = false;
            }
        }

        private void OnFadeOutStarted()
        {
            using (_PRF_OnFadeOutStarted.Auto())
            {
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(CanvasFadeManager) + ".";

        private static readonly ProfilerMarker _PRF_OnFadeInStarted =
            new ProfilerMarker(_PRF_PFX + nameof(OnFadeInStarted));

        private static readonly ProfilerMarker _PRF_OnFadeInCompleted =
            new ProfilerMarker(_PRF_PFX + nameof(OnFadeInCompleted));

        private static readonly ProfilerMarker _PRF_OnFadeOutStarted =
            new ProfilerMarker(_PRF_PFX + nameof(OnFadeOutStarted));

        private static readonly ProfilerMarker _PRF_OnFadeOutCompleted =
            new ProfilerMarker(_PRF_PFX + nameof(OnFadeOutCompleted));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_ExecuteFade =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteFade));

        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(Update));

        #endregion
    }
}
