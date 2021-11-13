using System;
using System.Collections;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Behaviours;
using Appalachia.Utility.Interpolation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Screens.Fading
{
    [ExecuteAlways, SmartLabelChildren]
    public abstract class UIFadeManager<T> : AppalachiaBehaviour
    {
        public UIFadeSettings uiFadeSettings;

        private bool _isFading;

        public bool IsFading => _isFading;

        protected abstract void ExecuteFade(float time);

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            Initialize();
            ExecuteFade(uiFadeSettings.startVisible ? uiFadeSettings.maximumAlpha : uiFadeSettings.minimumAlpha);
        }

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
        public void Show()
        {
            ExecuteFade(uiFadeSettings.maximumAlpha);
        }

        [ButtonGroup("Utilities")]
        public void Hide()
        {
            ExecuteFade(uiFadeSettings.minimumAlpha);
        }

        private void Initialize()
        {
#if UNITY_EDITOR
            if (uiFadeSettings == null)
            {
                var go = gameObject;
                var sceneName = go.scene.name;
                var objectName = go.name;
                
                var assetName = $"{sceneName}_{objectName}_{nameof(UIFadeSettings)}";
                
                uiFadeSettings = UIFadeSettings.LoadOrCreateNew(assetName);
            }
#endif
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

                var fadeValue = interpolator.Interpolate(fadePercentage, uiFadeSettings.maximumAlpha, uiFadeSettings.minimumAlpha);
                
                ExecuteFade(fadeValue);
                yield return new WaitForEndOfFrame();
            }

            _isFading = false;
            yield return null;
        }

        private IEnumerator FadeScreenInCoroutine()
        {
            Initialize();
            _isFading = true;
            
            var interpolator = InterpolatorFactory.GetInterpolator(uiFadeSettings.fadeIn);

            var fadePercentage = 0.0f;
            
            while (fadePercentage <= 1.0f)
            {
                fadePercentage += Time.unscaledDeltaTime / uiFadeSettings.fadeInDuration;

                var fadeValue = interpolator.Interpolate(fadePercentage, uiFadeSettings.maximumAlpha, uiFadeSettings.minimumAlpha);
                
                ExecuteFade(fadeValue);

                yield return new WaitForEndOfFrame();
            }

            _isFading = false;
            yield return null;
        }
    }
}
