using System.Collections;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Behaviours;
using Appalachia.Core.Scriptables;
using Appalachia.Utility.Interpolation;
using Appalachia.Utility.Interpolation.Modes;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Application.Screens.Fading
{
    [ExecuteAlways, SmartLabelChildren]
    public abstract class FadeManager<T> : AppalachiaBehaviour
    {
        public delegate void FadeInHandler();
        public delegate void FadeOutHandler();
        public delegate void FadeHandler(bool fadeIn);

        #region Fields and Autoproperties

        [FormerlySerializedAs("uiFadeSettings")] public FadeSettings fadeSettings;

        private bool _isFading;

        #endregion

        public bool IsFading => _isFading;
        public event FadeHandler OnFadeCompleted;

        public event FadeInHandler OnFadeInCompleted;
        public event FadeOutHandler OnFadeOutCompleted;

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
                fadeSettings.startVisible ? fadeSettings.maximumAlpha : fadeSettings.minimumAlpha
            );
        }

        #endregion

        public override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

#if UNITY_EDITOR
                if (fadeSettings == null)
                {
                    var go = gameObject;
                    var sceneName = go.scene.name;
                    var objectName = go.name;

                    var assetName = $"{sceneName}_{objectName}_{nameof(FadeSettings)}";

                    fadeSettings = AppalachiaObject.LoadOrCreateNew<FadeSettings>(assetName);
                }
#endif
            }
        }

        [ButtonGroup("Fades")]
        public void FadeIn()
        {
            if (_isFading)
            {
                return;
            }

            StartCoroutine(FadeScreenInCoroutine());
        }

        [ButtonGroup("Fades")]
        public void FadeOut()
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
            ExecuteFade(fadeSettings.minimumAlpha);
        }

        [ButtonGroup("Utilities")]
        public void Show()
        {
            ExecuteFade(fadeSettings.maximumAlpha);
        }

        protected abstract void ExecuteFade(float time);

        private IEnumerator ExecuteInternal(
            IInterpolationMode interpolator,
            float startValue,
            float endValue,
            float duration)
        {
            var fadePercentage = 0.0f;

            while (fadePercentage <= 1.0f)
            {
                fadePercentage += Time.unscaledDeltaTime / duration;

                var fadeValue = interpolator.Interpolate(startValue, endValue, fadePercentage);

                ExecuteFade(fadeValue);

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator FadeScreenInCoroutine()
        {
            Initialize();
            _isFading = true;

            var interpolator = InterpolatorFactory.GetInterpolator(fadeSettings.fadeIn);

            var enumerator = ExecuteInternal(
                interpolator,
                fadeSettings.minimumAlpha,
                fadeSettings.maximumAlpha,
                fadeSettings.fadeInDuration
            );

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }

            _isFading = false;

            OnFadeInCompleted?.Invoke();
            OnFadeCompleted?.Invoke(true);
        }

        private IEnumerator FadeScreenOutCoroutine()
        {
            Initialize();
            _isFading = true;

            var interpolator = InterpolatorFactory.GetInterpolator(fadeSettings.fadeOut);

            var enumerator = ExecuteInternal(
                interpolator,
                fadeSettings.maximumAlpha,
                fadeSettings.minimumAlpha,
                fadeSettings.fadeOutDuration
            );

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }

            _isFading = false;

            OnFadeOutCompleted?.Invoke();
            OnFadeCompleted?.Invoke(false);
        }

        #region Profiling

        private const string _PRF_PFX = nameof(FadeManager<T>) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
