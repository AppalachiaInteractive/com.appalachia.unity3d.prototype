using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Appalachia.Utility.Strings;
using Appalachia.Utility.Timing;
using Unity.Profiling;
using UnityEngine;

// ReSharper disable FormatStringProblem

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Services.
    Rendering
{
    [CallStaticConstructorInEditor]
    public sealed class RenderingProfilerService : DeveloperInterfaceManager_V01.Service<
        RenderingProfilerService, RenderingProfilerServiceMetadata, PerformanceProfiingFeature,
        PerformanceProfiingFeatureMetadata>
    {
        #region Constants and Static Readonly

        private const string STRING_SCENE_NAME_AND_COUNT = "{0} ({1})";

        private const ushort PERCENT_SAMPLE_COUNT = 10;
        private const ushort TOTAL_SAMPLE_COUNT = 1024;

        private static readonly Utf8PreparedFormat<string, int> FORMAT_SCENE_NAME_AND_COUNT =
            new(STRING_SCENE_NAME_AND_COUNT);

        #endregion

        #region Fields and Autoproperties

        [SerializeField] private ushort _currentFPS;
        [SerializeField] private ushort _averageFPS;
        [SerializeField] private ushort _onePercentFPS;
        [SerializeField] private ushort _zero1PercentFps;
        private ushort[] allFpsSampleValues;
        private ushort[] bottomPercentFpsSamples;
        private ushort currentSampleIndex;

        #endregion

        public ushort AverageFPS => _averageFPS;
        public ushort CurrentFPS => _currentFPS;
        public ushort OnePercentFPS => _onePercentFPS;
        public ushort Zero1PercentFps => _zero1PercentFps;

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
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                
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

                _averageFPS = (ushort)(sumFps / (float)fpsSampleCount);
                _onePercentFPS = (ushort)(sumFpsBottomPercentage / (float)fpsBottomPercentageSampleCount);
                _zero1PercentFps = minimumFps;
            }
        }

        private void UpdateCurrentFPS(float frameDeltaTime)
        {
            var realFps = 1f / frameDeltaTime;
            var roundedFps = Mathf.RoundToInt(realFps);
            _currentFPS = (ushort)Mathf.Max(1, roundedFps);

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
