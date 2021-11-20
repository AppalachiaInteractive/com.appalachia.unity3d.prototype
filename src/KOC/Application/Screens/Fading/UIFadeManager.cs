using System.Collections;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Behaviours;
using Appalachia.Core.Scriptables;
using Appalachia.Utility.Interpolation;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Screens.Fading
{
    [ExecuteAlways, SmartLabelChildren]
    public abstract class UIFadeManager<T> : AppalachiaBehaviour
    {
        #region Fields and Autoproperties

        public UIFadeSettings uiFadeSettings;

        private bool _isFading;

        #endregion

        public bool IsFading => _isFading;

        #region Event Functions

        protected override void Awake()
        {
            base.Awake();

            Initialize();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Initialize();
            ExecuteFade(
                uiFadeSettings.startVisible ? uiFadeSettings.maximumAlpha : uiFadeSettings.minimumAlpha
            );
        }

        #endregion

        [ButtonGroup("Fades")]
        public void FadeScreenIn()
        {
            if (_isFading)
            {
                return;
            }

            StartCoroutine(FadeScreenInCoroutine());
        }

        [ButtonGroup("Fades")]
        public void FadeScreenOut()
        {
            if (_isFading)
            {
                return;
            }

            StartCoroutine(FadeScreenOutCoroutine());
        }

        [ButtonGroup("Utilities")]
        public void Hide()
        {
            ExecuteFade(uiFadeSettings.minimumAlpha);
        }

        [ButtonGroup("Utilities")]
        public void Show()
        {
            ExecuteFade(uiFadeSettings.maximumAlpha);
        }

        protected abstract void ExecuteFade(float time);

        private IEnumerator FadeScreenInCoroutine()
        {
            Initialize();
            _isFading = true;

            var interpolator = InterpolatorFactory.GetInterpolator(uiFadeSettings.fadeIn);

            var fadePercentage = 0.0f;

            while (fadePercentage <= 1.0f)
            {
                fadePercentage += Time.unscaledDeltaTime / uiFadeSettings.fadeInDuration;

                var fadeValue = interpolator.Interpolate(
                    fadePercentage,
                    uiFadeSettings.maximumAlpha,
                    uiFadeSettings.minimumAlpha
                );

                ExecuteFade(fadeValue);

                yield return new WaitForEndOfFrame();
            }

            _isFading = false;
            yield return null;
        }

        private IEnumerator FadeScreenOutCoroutine()
        {
            Initialize();
            _isFading = true;

            var interpolator = InterpolatorFactory.GetInterpolator(uiFadeSettings.fadeOut);

            var fadePercentage = 1.0f;

            while (fadePercentage >= 0.0f)
            {
                fadePercentage -= Time.unscaledDeltaTime / uiFadeSettings.fadeOutDuration;

                var fadeValue = interpolator.Interpolate(
                    fadePercentage,
                    uiFadeSettings.maximumAlpha,
                    uiFadeSettings.minimumAlpha
                );

                ExecuteFade(fadeValue);
                yield return new WaitForEndOfFrame();
            }

            _isFading = false;
            yield return null;
        }

        public override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

#if UNITY_EDITOR
                if (uiFadeSettings == null)
                {
                    var go = gameObject;
                    var sceneName = go.scene.name;
                    var objectName = go.name;

                    var assetName = $"{sceneName}_{objectName}_{nameof(UIFadeSettings)}";

                    uiFadeSettings = AppalachiaObject.LoadOrCreateNew<UIFadeSettings>(assetName);
                }
#endif
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(UIFadeManager<T>) + ".";
        
        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
