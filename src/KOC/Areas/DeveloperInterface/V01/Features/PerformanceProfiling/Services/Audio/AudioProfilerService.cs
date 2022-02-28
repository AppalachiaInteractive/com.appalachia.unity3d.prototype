using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Features.Availability.Extensions;
using Appalachia.Utility.Async;
using Appalachia.Utility.Strings;
using Appalachia.Utility.Timing;
using UnityEngine;

// ReSharper disable FormatStringProblem

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Services.Audio
{
    [CallStaticConstructorInEditor]
    public sealed class AudioProfilerService : DeveloperInterfaceManager_V01.Service<AudioProfilerService,
        AudioProfilerServiceMetadata, PerformanceProfiingFeature, PerformanceProfiingFeatureMetadata>
    {
        #region Constants and Static Readonly

        private const float DB_REF_VALUE = 1f;

        private const string STRING_SCENE_NAME_AND_COUNT = "{0} ({1})";

        private static AudioListener _audioListener;

        private static readonly Utf8PreparedFormat<string, int> FORMAT_SCENE_NAME_AND_COUNT =
            new(STRING_SCENE_NAME_AND_COUNT);

        #endregion

        static AudioProfilerService()
        {
            When.LifetimeComponentManager()
                .IsAvailableThen(
                     lifetimeComponentManager =>
                     {
                         lifetimeComponentManager.AudioListenerReady.Event += args =>
                         {
                             _audioListener = args.value;
                         };
                     }
                 );
        }

        #region Static Fields and Autoproperties

        #endregion

        #region Fields and Autoproperties

        [SerializeField] private float[] _spectrum;
        [SerializeField] private float[] _spectrumHighestValues;
        [SerializeField] private float _maxDB;

        #endregion

        /// <summary>
        ///     Returns true if there is a reference to the audio listener.
        /// </summary>
        public bool SpectrumDataAvailable => _audioListener != null;

        /// <summary>
        ///     Maximum DB registered in the current spectrum.
        /// </summary>
        public float MaxDB => _maxDB;

        /// <summary>
        ///     Current audio spectrum from the specified AudioListener.
        /// </summary>
        public float[] Spectrum => _spectrum;

        /// <summary>
        ///     Highest audio spectrum from the specified AudioListener in the last few seconds.
        /// </summary>
        public float[] SpectrumHighestValues => _spectrumHighestValues;

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                if (_audioListener != null)
                {
                    // Use this data to calculate the dB value

                    AudioListener.GetOutputData(Spectrum, 0);

                    float sum = 0;

                    for (var i = 0; i < Spectrum.Length; i++)
                    {
                        sum += Spectrum[i] * Spectrum[i]; // sum squared samples
                    }

                    var rmsValue = Mathf.Sqrt(sum / Spectrum.Length); // rms = square root of average

                    _maxDB = 20 * Mathf.Log10(rmsValue / DB_REF_VALUE); // calculate dB

                    if (_maxDB < -80)
                    {
                        _maxDB = -80; // clamp it to -80dB min
                    }

                    // Use this data to draw the spectrum in the graphs

                    AudioListener.GetSpectrumData(Spectrum, 0, Metadata.FFTWindow);

                    for (var i = 0; i < Spectrum.Length; i++)
                    {
                        // Update the highest value if its lower than the current one
                        if (Spectrum[i] > SpectrumHighestValues[i])
                        {
                            SpectrumHighestValues[i] = Spectrum[i];
                        }

                        // Slowly lower the value 
                        else
                        {
                            SpectrumHighestValues[i] = Mathf.Clamp(
                                SpectrumHighestValues[i] -
                                (SpectrumHighestValues[i] * CoreClock.Instance.DeltaTime * 2),
                                0,
                                1
                            );
                        }
                    }
                }
            }
        }

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                if ((Spectrum == null) || (Metadata.spectrumSize != Spectrum.Length))
                {
                    _spectrum = new float[Metadata.spectrumSize];
                }

                if ((SpectrumHighestValues == null) ||
                    (Metadata.spectrumSize != SpectrumHighestValues.Length))
                {
                    _spectrumHighestValues = new float[Metadata.spectrumSize];
                }
            }
        }
    }
}
