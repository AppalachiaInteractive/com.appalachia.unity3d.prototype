using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Appalachia.Utility.Strings;
using Appalachia.Utility.Timing;
using Unity.Profiling;
using UnityEngine;

// ReSharper disable FormatStringProblem

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Services.FPS
{
    public sealed class FPSProfilerService : DeveloperInterfaceManager_V01.Service<FPSProfilerService,
        FPSProfilerServiceMetadata, PerformanceProfilingFeature, PerformanceProfilingFeatureMetadata>
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
        [SerializeField] private ushort _zero1PercentFPS;
        private ushort[] allFPSSampleValues;
        private ushort[] bottomPercentFPSSamples;
        private ushort currentSampleIndex;

        private ushort _minimumFPS;
        private ushort _maximumFPS;
        private float _lastFrame;

        #endregion

        public float LastFrame => _lastFrame;
        public ushort AverageFPS => _averageFPS;
        public ushort CurrentFPS => _currentFPS;
        public ushort MaximumFPS => _maximumFPS;
        public ushort MinimumFPS => _minimumFPS;
        public ushort OnePercentFPS => _onePercentFPS;
        public ushort Zero1PercentFPS => _zero1PercentFPS;

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                _lastFrame = CoreClock.Instance.UnscaledDeltaTime;
                
                UpdateCurrentFPS(_lastFrame);

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
                if ((allFPSSampleValues == null) || (allFPSSampleValues.Length == 0))
                {
                    allFPSSampleValues = new ushort[TOTAL_SAMPLE_COUNT];
                }

                if ((bottomPercentFPSSamples == null) || (bottomPercentFPSSamples.Length == 0))
                {
                    bottomPercentFPSSamples = new ushort [PERCENT_SAMPLE_COUNT];
                }
            }
        }

        private static void AnalyzeArray(
            ushort[] samples,
            ushort[] sampleMinimums,
            ushort discardValue,
            out ushort minimum,
            out ushort maximum,
            out uint sum,
            out ushort sampleCount)
        {
            using (_PRF_AnalyzeArray.Auto())
            {
                sampleCount = 0;
                minimum = ushort.MaxValue;
                maximum = ushort.MinValue;
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

                    if (currentSample > maximum)
                    {
                        maximum = currentSample;
                    }

                    if (sampleMinimums != null)
                    {
                        if (currentSample < biggestSampleMinimum)
                        {
                            sampleMinimums[biggestSampleMinimumIndex] = currentSample;

                            GetMaximumSample(sampleMinimums, out biggestSampleMinimum, out biggestSampleMinimumIndex);
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
                for (var i = 0; i < bottomPercentFPSSamples.Length; i++)
                {
                    bottomPercentFPSSamples[i] = ushort.MaxValue;
                }

                AnalyzeArray(
                    allFPSSampleValues,
                    bottomPercentFPSSamples,
                    0,
                    out var minimumFPS,
                    out var maximumFPS,
                    out var sumFPS,
                    out var fpsSampleCount
                );

                AnalyzeArray(
                    bottomPercentFPSSamples,
                    null,
                    ushort.MaxValue,
                    out _,
                    out _,
                    out var sumFPSBottomPercentage,
                    out var fpsBottomPercentageSampleCount
                );

                _averageFPS = (ushort)(sumFPS / (float)fpsSampleCount);
                _onePercentFPS = (ushort)(sumFPSBottomPercentage / (float)fpsBottomPercentageSampleCount);
                _zero1PercentFPS = minimumFPS;
                _minimumFPS = minimumFPS;
                _maximumFPS = maximumFPS;
            }
        }

        private void UpdateCurrentFPS(float frameDeltaTime)
        {
            var realFPS = 1f / frameDeltaTime;
            var roundedFPS = Mathf.RoundToInt(realFPS);
            _currentFPS = (ushort)Mathf.Max(1, roundedFPS);

            currentSampleIndex++;

            if (currentSampleIndex >= allFPSSampleValues.Length)
            {
                currentSampleIndex = 0;
            }

            allFPSSampleValues[currentSampleIndex] = CurrentFPS;
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetMaximumSample =
            new ProfilerMarker(_PRF_PFX + nameof(GetMaximumSample));

        private static readonly ProfilerMarker _PRF_AnalyzeArray = new ProfilerMarker(_PRF_PFX + nameof(AnalyzeArray));

        private static readonly ProfilerMarker _PRF_BeforeInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(BeforeInitialization));

        private static readonly ProfilerMarker _PRF_CalculateAverageFPS =
            new ProfilerMarker(_PRF_PFX + nameof(CalculateFPSTimings));

        #endregion
    }
}
