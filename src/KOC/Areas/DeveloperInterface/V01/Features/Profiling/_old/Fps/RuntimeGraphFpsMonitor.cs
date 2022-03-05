using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Instance;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Settings;
using Appalachia.Utility.Timing;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Fps
{
    public class RuntimeGraphFpsMonitor : RuntimeGraphInstanceMonitor<RuntimeGraphFpsGraph,
        RuntimeGraphFpsManager, RuntimeGraphFpsMonitor, RuntimeGraphFpsText, RuntimeGraphFpsSettings>
    {
        #region Constants and Static Readonly

        private const ushort PERCENT_SAMPLE_COUNT = 10;
        private const ushort TOTAL_SAMPLE_COUNT = 1024;

        #endregion

        #region Fields and Autoproperties

        private ushort[] allFpsSampleValues;
        private ushort[] bottomPercentFpsSamples;
        private ushort currentSampleIndex;

        public ushort CurrentFPS { get; private set; }
        public ushort AverageFPS { get; private set; }
        public ushort OnePercentFPS { get; private set; }
        public ushort Zero1PercentFps { get; private set; }

        #endregion

        /// <inheritdoc />
        protected override RuntimeGraphFpsSettings settings => allSettings.fps;

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                UpdateCurrentFPS(CoreClock.Instance.UnscaledDeltaTime);

                CalculateFPSTimings();
            }
        }

        #endregion

        /// <inheritdoc />
        protected override void BeforeInitialization()
        {
            using (_PRF_BeforeInitialization.Auto())
            {
                base.BeforeInitialization();

                if ((allFpsSampleValues == null) || (allFpsSampleValues.Length == 0))
                {
                    allFpsSampleValues = new ushort[TOTAL_SAMPLE_COUNT];
                }

                if ((bottomPercentFpsSamples == null) || (bottomPercentFpsSamples.Length == 0))
                {
                    bottomPercentFpsSamples = new ushort [PERCENT_SAMPLE_COUNT];
                }
            }
        }

        private static void AnalyzeArray(
            ushort[] samples,
            ushort[] sampleMinimums,
            ushort discardValue,
            out ushort minimum,
            out uint sum,
            out ushort sampleCount)
        {
            using (_PRF_AnalyzeArray.Auto())
            {
                sampleCount = 0;
                minimum = ushort.MaxValue;
                sum = 0;

                ushort biggestSampleMinimum = 0;
                ushort biggestSampleMinimumIndex = 0;

                if (sampleMinimums != null)
                {
                    GetMaximumSample(sampleMinimums, out biggestSampleMinimum, out biggestSampleMinimumIndex);
                }

                for (var i = 0; i < samples.Length; i++)
                {
                    var currentSample = samples[i];

                    if (currentSample == discardValue)
                    {
                        continue;
                    }

                    sampleCount++;

                    if (currentSample < minimum)
                    {
                        minimum = currentSample;
                    }

                    if (sampleMinimums != null)
                    {
                        if (currentSample < biggestSampleMinimum)
                        {
                            sampleMinimums[biggestSampleMinimumIndex] = currentSample;

                            GetMaximumSample(
                                sampleMinimums,
                                out biggestSampleMinimum,
                                out biggestSampleMinimumIndex
                            );
                        }
                    }

                    sum += currentSample;
                }
            }
        }

        private static void GetMaximumSample(ushort[] samples, out ushort maximumSample, out ushort index)
        {
            using (_PRF_GetMaximumSample.Auto())
            {
                maximumSample = ushort.MinValue;
                index = 0;

                for (ushort i = 0; i < samples.Length; i++)
                {
                    var sample = samples[i];

                    if (sample > maximumSample)
                    {
                        maximumSample = sample;
                        index = i;
                    }
                }
            }
        }

        private void CalculateFPSTimings()
        {
            using (_PRF_CalculateAverageFPS.Auto())
            {
                for (var i = 0; i < bottomPercentFpsSamples.Length; i++)
                {
                    bottomPercentFpsSamples[i] = ushort.MaxValue;
                }

                AnalyzeArray(
                    allFpsSampleValues,
                    bottomPercentFpsSamples,
                    0,
                    out var minimumFps,
                    out var sumFps,
                    out var fpsSampleCount
                );

                AnalyzeArray(
                    bottomPercentFpsSamples,
                    null,
                    ushort.MaxValue,
                    out _,
                    out var sumFpsBottomPercentage,
                    out var fpsBottomPercentageSampleCount
                );

                AverageFPS = (ushort)(sumFps / (float)fpsSampleCount);
                OnePercentFPS = (ushort)(sumFpsBottomPercentage / (float)fpsBottomPercentageSampleCount);
                Zero1PercentFps = minimumFps;
            }
        }

        private void UpdateCurrentFPS(float frameDeltaTime)
        {
            var realFps = 1f / frameDeltaTime;
            var roundedFps = Mathf.RoundToInt(realFps);
            CurrentFPS = (ushort)Mathf.Max(1, roundedFps);

            currentSampleIndex++;

            if (currentSampleIndex >= allFpsSampleValues.Length)
            {
                currentSampleIndex = 0;
            }

            allFpsSampleValues[currentSampleIndex] = CurrentFPS;
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetMaximumSample =
            new ProfilerMarker(_PRF_PFX + nameof(GetMaximumSample));

        private static readonly ProfilerMarker _PRF_AnalyzeArray =
            new ProfilerMarker(_PRF_PFX + nameof(AnalyzeArray));

        private static readonly ProfilerMarker _PRF_BeforeInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(BeforeInitialization));

        private static readonly ProfilerMarker _PRF_CalculateAverageFPS =
            new ProfilerMarker(_PRF_PFX + nameof(CalculateFPSTimings));

        #endregion
    }
}
