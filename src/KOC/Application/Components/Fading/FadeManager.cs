using System.Collections;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Interpolation;
using Appalachia.Utility.Interpolation.Modes;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components.Fading
{
    [ExecuteAlways, SmartLabelChildren]
    public abstract class FadeManager<T> : AppalachiaApplicationBehaviour
    {
        public delegate void FadeHandler(bool fadeIn);

        public delegate void FadeInHandler();

        public delegate void FadeOutHandler();

        #region Fields and Autoproperties

        [SerializeField] public FadeSettings fadeSettings;

        [Range(0f, 1f)] public float fadePercentage;

        private bool _isFading;

        private bool _forceActive;

        #endregion

        public bool IsFading => _isFading;

        private bool passiveMode => fadeSettings.passiveMode;
        public event FadeHandler FadeCompleted;

        public event FadeInHandler FadeInCompleted;
        public event FadeOutHandler FadeOutCompleted;
        public event FadeHandler FadeStarted;

        public event FadeInHandler FadeInStarted;
        public event FadeOutHandler FadeOutStarted;

        #region Event Functions

        protected override void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                base.Awake();

                Initialize();
            }
        }

        protected virtual void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (!fadeSettings.passiveMode)
                {
                    return;
                }

                if (_forceActive)
                {
                    return;
                }

                var interpolator = InterpolatorFactory.GetInterpolator(InterpolationMode.SmoothStep);

                var fadeValue = interpolator.Interpolate(
                    fadeSettings.minimumAlpha,
                    fadeSettings.maximumAlpha,
                    fadePercentage
                );

                ExecuteFade(fadeValue);
            }
        }

        protected override async AppaTask OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                await base.WhenEnabled();

                Initialize();

                if (fadeSettings.updateAtStart)
                {
                    ExecuteFade(
                        fadeSettings.startVisible ? fadeSettings.maximumAlpha : fadeSettings.minimumAlpha
                    );
                }
            }
        }

        #endregion

        [ButtonGroup("Fades")]
        [DisableIf(nameof(passiveMode))]
        public void FadeIn()
        {
            using (_PRF_FadeIn.Auto())
            {
                if (fadeSettings.passiveMode && !_forceActive)
                {
                    return;
                }

                if (_isFading)
                {
                    return;
                }

                Initialize();

                StartCoroutine(FadeScreenInCoroutine());
            }
        }

        [ButtonGroup("Fades")]
        [DisableIf(nameof(passiveMode))]
        public void FadeOut()
        {
            using (_PRF_FadeOut.Auto())
            {
                if (fadeSettings.passiveMode && !_forceActive)
                {
                    return;
                }

                if (_isFading)
                {
                    return;
                }

                Initialize();

                StartCoroutine(FadeScreenOutCoroutine());
            }
        }

        public void ForceActive()
        {
            _forceActive = true;
        }

        [ButtonGroup("Utilities")]
        [DisableIf(nameof(passiveMode))]
        public void Hide()
        {
            using (_PRF_Hide.Auto())
            {
                if (fadeSettings.passiveMode && !_forceActive)
                {
                    return;
                }

                ExecuteFade(fadeSettings.minimumAlpha);
            }
        }

        [ButtonGroup("Utilities")]
        [DisableIf(nameof(passiveMode))]
        public void Show()
        {
            using (_PRF_Show.Auto())
            {
                if (fadeSettings.passiveMode && !_forceActive)
                {
                    return;
                }

                ExecuteFade(fadeSettings.maximumAlpha);
            }
        }

        protected abstract void ExecuteFade(float time);

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

#if UNITY_EDITOR
                var go = gameObject;
                var sceneName = go.scene.name;

                var objectName = go.name;
                var assetName = ZString.Format("{0}_{1}_{2}", sceneName, objectName, nameof(FadeSettings));
#endif

                if (fadeSettings == null)
                {
#if UNITY_EDITOR
                    if (AppalachiaApplication.IsPlayingOrWillPlay)
                    {
#endif
                        fadeSettings = ScriptableObject.CreateInstance<FadeSettings>();
#if UNITY_EDITOR
                    }
                    else
                    {
                        fadeSettings = FadeSettings.LoadOrCreateNew(assetName);
                    }
#endif
                }

#if UNITY_EDITOR
                if ((fadeSettings.name != assetName) && !AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    fadeSettings.Rename(assetName);
                }
#endif
            }
        }

        private IEnumerator ExecuteInternal(
            IInterpolationMode interpolator,
            float startValue,
            float endValue,
            float duration)
        {
            if (fadeSettings.passiveMode && !_forceActive)
            {
                yield break;
            }

            var localFadePercentage = 0.0f;

            ExecuteFade(startValue);

            while (localFadePercentage <= 1.0f)
            {
                localFadePercentage += Time.unscaledDeltaTime / duration;

                var fadeValue = interpolator.Interpolate(startValue, endValue, localFadePercentage);

                ExecuteFade(fadeValue);

                yield return new WaitForEndOfFrame();
            }

            ExecuteFade(endValue);
        }

        private IEnumerator FadeScreenInCoroutine()
        {
            Initialize();
            _isFading = true;

            var interpolator = InterpolatorFactory.GetInterpolator(fadeSettings.fadeIn);

            FadeStarted?.Invoke(true);
            FadeInStarted?.Invoke();

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

            FadeInCompleted?.Invoke();
            FadeCompleted?.Invoke(true);
        }

        private IEnumerator FadeScreenOutCoroutine()
        {
            Initialize();
            _isFading = true;

            var interpolator = InterpolatorFactory.GetInterpolator(fadeSettings.fadeOut);

            FadeStarted?.Invoke(false);
            FadeOutStarted?.Invoke();

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

            FadeOutCompleted?.Invoke();
            FadeCompleted?.Invoke(false);
        }

        #region Profiling

        private const string _PRF_PFX = nameof(FadeManager<T>) + ".";
        private static readonly ProfilerMarker _PRF_FadeIn = new ProfilerMarker(_PRF_PFX + nameof(FadeIn));
        private static readonly ProfilerMarker _PRF_FadeOut = new ProfilerMarker(_PRF_PFX + nameof(FadeOut));
        private static readonly ProfilerMarker _PRF_Show = new ProfilerMarker(_PRF_PFX + nameof(Show));
        private static readonly ProfilerMarker _PRF_Hide = new ProfilerMarker(_PRF_PFX + nameof(Hide));
        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));

        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(Update));

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
