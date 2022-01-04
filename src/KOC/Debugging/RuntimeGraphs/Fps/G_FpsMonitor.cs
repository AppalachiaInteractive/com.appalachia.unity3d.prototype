using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Fps
{
    public class G_FpsMonitor : RuntimeGraphInstanceMonitor<G_FpsGraph, G_FpsManager, G_FpsMonitor, G_FpsText,
        RuntimeGraphFpsSettings>
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

        protected override RuntimeGraphFpsSettings settings => allSettings.fps;

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (!DependenciesAreReady || !FullyInitialized)
                {
                    return;
                }
                
                UpdateCurrentFPS(Time.unscaledDeltaTime);

                CalculateFPSTimings();
            }
        }

        #endregion

        protected override void BeforeInitialize()
        {
            using (_PRF_BeforeInitialize.Auto())
            {
                allFpsSampleValues ??= new ushort[TOTAL_SAMPLE_COUNT];
                bottomPercentFpsSamples ??= new ushort [PERCENT_SAMPLE_COUNT];
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

        private const string _PRF_PFX = nameof(G_FpsMonitor) + ".";

        private static readonly ProfilerMarker _PRF_GetMaximumSample =
            new ProfilerMarker(_PRF_PFX + nameof(GetMaximumSample));

        private static readonly ProfilerMarker _PRF_AnalyzeArray =
            new ProfilerMarker(_PRF_PFX + nameof(AnalyzeArray));

        private static readonly ProfilerMarker _PRF_CalculateAverageFPS =
            new ProfilerMarker(_PRF_PFX + nameof(CalculateFPSTimings));

        private static readonly ProfilerMarker _PRF_BeforeInitialize =
            new ProfilerMarker(_PRF_PFX + nameof(BeforeInitialize));

        private static readonly ProfilerMarker _PRF_UpdateParameters =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeParameters));

        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(Update));

        #endregion
    }
}
