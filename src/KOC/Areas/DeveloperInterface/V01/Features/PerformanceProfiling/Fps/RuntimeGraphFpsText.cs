using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Instance;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Settings;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Util;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Fps
{
    public class RuntimeGraphFpsText : RuntimeGraphInstanceText<RuntimeGraphFpsGraph, RuntimeGraphFpsManager,
        RuntimeGraphFpsMonitor, RuntimeGraphFpsText, RuntimeGraphFpsSettings>
    {
        #region Constants and Static Readonly

        private const string m_msStringFormat = "0.0";

        #endregion

        #region Fields and Autoproperties

        [SerializeField] private Text m_fpsText;
        [SerializeField] private Text m_msText;

        [SerializeField] private Text m_avgFpsText;
        [SerializeField] private Text m_onePercentFpsText;
        [SerializeField] private Text m_zero1PercentFpsText;

        private float m_fps;

        private float m_ms;

        #endregion

        /// <inheritdoc />
        protected override bool ShouldUpdate => true;

        /// <inheritdoc />
        protected override RuntimeGraphFpsSettings settings => allSettings.fps;

        /// <inheritdoc />
        protected override void AfterInitialization()
        {
            using (_PRF_AfterInitialization.Auto())
            {
                base.AfterInitialization();

                G_IntString.Init(0, 2000);  // Max fps expected
                G_FloatString.Init(0, 100); // Max ms expected per frame
            }
        }

        /// <inheritdoc />
        protected override void UpdateText()
        {
            using (_PRF_UpdateText.Auto())
            {
                m_fps = frameCount / deltaTime;
                m_ms = (deltaTime / frameCount) * 1000f;

                // Update fps
                m_fpsText.text = Mathf.RoundToInt(m_fps).ToStringNonAlloc();
                SetFpsRelatedTextColor(m_fpsText, m_fps);

                // Update ms
                m_msText.text = m_ms.ToStringNonAlloc(m_msStringFormat);
                SetFpsRelatedTextColor(m_msText, m_fps);

                // Update 1% fps
                m_onePercentFpsText.text = ((int)monitor.OnePercentFPS).ToStringNonAlloc();
                SetFpsRelatedTextColor(m_onePercentFpsText, monitor.OnePercentFPS);

                // Update 0.1% fps
                m_zero1PercentFpsText.text = ((int)monitor.Zero1PercentFps).ToStringNonAlloc();
                SetFpsRelatedTextColor(m_zero1PercentFpsText, monitor.Zero1PercentFps);

                // Update avg fps
                m_avgFpsText.text = ((int)monitor.AverageFPS).ToStringNonAlloc();
                SetFpsRelatedTextColor(m_avgFpsText, monitor.AverageFPS);
            }
        }

        /// <summary>
        ///     Assigns color to a text according to their fps numeric value and
        ///     the colors specified in the 3 categories (Good, Caution, Critical).
        /// </summary>
        /// <param name="text">
        ///     UI Text component to change its color
        /// </param>
        /// <param name="fps">
        ///     Numeric fps value
        /// </param>
        private void SetFpsRelatedTextColor(Text text, float fps)
        {
            using (_PRF_SetFpsRelatedTextColor.Auto())
            {
                if (fps > settings.goodFpsThreshold)
                {
                    text.color = settings.goodFpsColor;
                }
                else if (fps > settings.cautionFpsThreshold)
                {
                    text.color = settings.cautionFpsColor;
                }
                else
                {
                    text.color = settings.criticalFpsColor;
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_AfterInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(AfterInitialization));

        private static readonly ProfilerMarker _PRF_SetFpsRelatedTextColor =
            new ProfilerMarker(_PRF_PFX + nameof(SetFpsRelatedTextColor));

        private static readonly ProfilerMarker _PRF_UpdateText =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateText));

        #endregion
    }
}
