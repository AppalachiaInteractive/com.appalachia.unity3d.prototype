using System.Collections;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Behaviours;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Interpolation;
using Appalachia.Utility.Interpolation.Modes;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.Fading
{
    [ExecuteAlways, SmartLabelChildren]
    public abstract class FadeManager<T> : AppalachiaApplicationBehaviour<T>
        where T : FadeManager<T>
    {
        public delegate void FadeHandler(bool fadeIn);

        public delegate void FadeInHandler();

        public delegate void FadeOutHandler();

        public event FadeHandler FadeCancelled;

        public event FadeHandler FadeCompleted;

        public event FadeInHandler FadeInCancelled;
        public event FadeInHandler FadeInCompleted;

        public event FadeInHandler FadeInStarted;
        public event FadeOutHandler FadeOutCancelled;
        public event FadeOutHandler FadeOutCompleted;
        public event FadeOutHandler FadeOutStarted;
        public event FadeHandler FadeStarted;

        #region Fields and Autoproperties

        [SerializeField] public FadeSettings fadeSettings;

        [Range(0f, 1f)] public float fadePercentage;

        private bool _isFadingIn;
        private bool _isFadingOut;

        private bool _forceActive;
        private bool _cancelActiveFade;

        #endregion

        public bool IsFading => _isFadingIn || _isFadingOut;
        public bool IsFadingIn => _isFadingIn;
        public bool IsFadingOut => _isFadingOut;

        private bool passiveMode => fadeSettings.passiveMode;

        #region Event Functions

        protected virtual void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

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

        #endregion

        public void CancelActiveFade()
        {
            using (_PRF_CancelActiveFade.Auto())
            {
                _cancelActiveFade = true;
            }
        }

        public void EnsureFadeIn()
        {
            using (_PRF_EnsureFadeIn.Auto())
            {
                if (IsFading)
                {
                    if (IsFadingIn)
                    {
                        return;
                    }

                    CancelActiveFade();
                }

                FadeIn();
            }
        }

        public void EnsureFadeOut()
        {
            using (_PRF_EnsureFadeOut.Auto())
            {
                if (IsFading)
                {
                    if (IsFadingOut)
                    {
                        return;
                    }

                    CancelActiveFade();
                }

                FadeOut();
            }
        }

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

                if (IsFading)
                {
                    return;
                }

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

                if (IsFading)
                {
                    return;
                }

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

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
#if UNITY_EDITOR

                var go = gameObject;
                var sceneName = go.scene.name;

                var objectName = go.name;
                var assetName = ZString.Format("{0}_{1}_{2}", sceneName, objectName, nameof(FadeSettings));

                using (_PRF_Initialize.Suspend())
                {
                    initializer.Do(
                        this,
                        nameof(FadeSettings),
                        fadeSettings == null,
                        () =>
                        {
                            using (_PRF_Initialize.Auto())
                            {
                                if (fadeSettings == null)
                                {
                                    fadeSettings = AppalachiaApplication.IsPlayingOrWillPlay
                                        ? ScriptableObject.CreateInstance<FadeSettings>()
                                        : FadeSettings.LoadOrCreateNew(assetName);
                                }
                            }
                        }
                    );
                }

                if ((fadeSettings.name != assetName) && !AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    fadeSettings.Rename(assetName);
                }
#else
           using(_PRF_Initialize.Auto())
{
         if (fadeSettings == null)
                {                    
                    fadeSettings = ScriptableObject.CreateInstance<FadeSettings>();
                }
}
#endif
            }
        }

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                if (fadeSettings.updateAtStart)
                {
                    ExecuteFade(
                        fadeSettings.startVisible ? fadeSettings.maximumAlpha : fadeSettings.minimumAlpha
                    );
                }
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
            _isFadingIn = true;

            var interpolator = InterpolatorFactory.GetInterpolator(fadeSettings.fadeIn);

            FadeStarted?.Invoke(true);
            FadeInStarted?.Invoke();

            var enumerator = ExecuteInternal(
                interpolator,
                fadeSettings.minimumAlpha,
                fadeSettings.maximumAlpha,
                fadeSettings.fadeInDuration
            );

            while (!_cancelActiveFade && enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }

            _isFadingIn = false;

            if (_cancelActiveFade)
            {
                FadeInCancelled?.Invoke();
                FadeCancelled?.Invoke(true);
                _cancelActiveFade = false;
            }
            else
            {
                FadeInCompleted?.Invoke();
                FadeCompleted?.Invoke(true);
            }
        }

        private IEnumerator FadeScreenOutCoroutine()
        {
            _isFadingOut = true;

            var interpolator = InterpolatorFactory.GetInterpolator(fadeSettings.fadeOut);

            FadeStarted?.Invoke(false);
            FadeOutStarted?.Invoke();

            var enumerator = ExecuteInternal(
                interpolator,
                fadeSettings.maximumAlpha,
                fadeSettings.minimumAlpha,
                fadeSettings.fadeOutDuration
            );

            while (!_cancelActiveFade && enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }

            _isFadingOut = false;

            if (_cancelActiveFade)
            {
                FadeOutCancelled?.Invoke();
                FadeCancelled?.Invoke(false);
                _cancelActiveFade = false;
            }
            else
            {
                FadeOutCompleted?.Invoke();
                FadeCompleted?.Invoke(false);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_CancelActiveFade =
            new ProfilerMarker(_PRF_PFX + nameof(CancelActiveFade));

        private static readonly ProfilerMarker _PRF_EnsureFadeIn =
            new ProfilerMarker(_PRF_PFX + nameof(EnsureFadeIn));

        private static readonly ProfilerMarker _PRF_EnsureFadeOut =
            new ProfilerMarker(_PRF_PFX + nameof(EnsureFadeOut));

        private static readonly ProfilerMarker _PRF_FadeIn = new ProfilerMarker(_PRF_PFX + nameof(FadeIn));
        private static readonly ProfilerMarker _PRF_FadeOut = new ProfilerMarker(_PRF_PFX + nameof(FadeOut));
        private static readonly ProfilerMarker _PRF_Hide = new ProfilerMarker(_PRF_PFX + nameof(Hide));

        private static readonly ProfilerMarker _PRF_Show = new ProfilerMarker(_PRF_PFX + nameof(Show));

        #endregion
    }
}
