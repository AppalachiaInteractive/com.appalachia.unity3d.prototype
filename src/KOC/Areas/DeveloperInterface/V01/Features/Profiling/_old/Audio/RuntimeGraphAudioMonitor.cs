﻿using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Instance;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Settings;
using Appalachia.Utility.Async;
using Appalachia.Utility.Timing;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Audio
{
    /// <summary>
    ///     Note: this class only works with Unity's AudioListener.
    ///     If you're using a custom audio engine (like FMOD or WWise) it won't work,
    ///     although you can always adapt it.
    /// </summary>
    public class RuntimeGraphAudioMonitor : RuntimeGraphInstanceMonitor<RuntimeGraphAudioGraph,
        RuntimeGraphAudioManager, RuntimeGraphAudioMonitor, RuntimeGraphAudioText, RuntimeGraphAudioSettings>
    {
        #region Constants and Static Readonly

        private const float m_refValue = 1f;

        #endregion

        #region Fields and Autoproperties

        /// <summary>
        ///     Current audio spectrum from the specified AudioListener.
        /// </summary>
        public float[] Spectrum { get; private set; }

        /// <summary>
        ///     Highest audio spectrum from the specified AudioListener in the last few seconds.
        /// </summary>
        public float[] SpectrumHighestValues { get; private set; }

        /// <summary>
        ///     Maximum DB registered in the current spectrum.
        /// </summary>
        public float MaxDB { get; private set; }

        #endregion

        /// <summary>
        ///     Returns true if there is a reference to the audio listener.
        /// </summary>
        public bool SpectrumDataAvailable => RuntimeGraphManager.AudioListener != null;

        /// <inheritdoc />
        protected override RuntimeGraphAudioSettings settings => allSettings.audio;

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                if (RuntimeGraphManager.AudioListener != null)
                {
                    // Use this data to calculate the dB value

                    AudioListener.GetOutputData(Spectrum, 0);

                    float sum = 0;

                    for (var i = 0; i < Spectrum.Length; i++)
                    {
                        sum += Spectrum[i] * Spectrum[i]; // sum squared samples
                    }

                    var rmsValue = Mathf.Sqrt(sum / Spectrum.Length); // rms = square root of average

                    MaxDB = 20 * Mathf.Log10(rmsValue / m_refValue); // calculate dB

                    if (MaxDB < -80)
                    {
                        MaxDB = -80; // clamp it to -80dB min
                    }

                    // Use this data to draw the spectrum in the graphs

                    AudioListener.GetSpectrumData(Spectrum, 0, settings.FFTWindow);

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
                else if ((RuntimeGraphManager.AudioListener == null) &&
                         (settings.findListenerIfNull == LookForAudioListener.ALWAYS))
                {
                    RuntimeGraphManager.AudioListener = FindAudioListener();
                }
            }
        }

        #endregion

        /// <inheritdoc />
        public override void InitializeParameters()
        {
            using (_PRF_InitializeParameters.Auto())
            {
                base.InitializeParameters();

                if ((RuntimeGraphManager.AudioListener == null) &&
                    (settings.findListenerIfNull != LookForAudioListener.NEVER))
                {
                    RuntimeGraphManager.AudioListener = FindAudioListener();
                }

                if ((Spectrum == null) || (settings.spectrumSize != Spectrum.Length))
                {
                    Spectrum = new float[settings.spectrumSize];
                }

                if ((SpectrumHighestValues == null) ||
                    (settings.spectrumSize != SpectrumHighestValues.Length))
                {
                    SpectrumHighestValues = new float[settings.spectrumSize];
                }
            }
        }

        /// <inheritdoc />
        protected override void AfterInitialization()
        {
            using (_PRF_AfterInitialization.Auto())
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
                SceneManager.sceneLoaded += OnSceneLoaded;

                base.AfterInitialization();
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenDestroyed()
        {
            await base.WhenDestroyed();

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        ///     Tries to find an audio listener in the main camera.
        /// </summary>
        private AudioListener FindAudioListener()
        {
            var mainCamera = Camera.main;

            if ((mainCamera != null) && mainCamera.TryGetComponent(out AudioListener audioListener))
            {
                return audioListener;
            }

            return null;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            using (_PRF_OnSceneLoaded.Auto())
            {
                if (settings.findListenerIfNull == LookForAudioListener.ON_SCENE_LOAD)
                {
                    RuntimeGraphManager.AudioListener = FindAudioListener();
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_AfterInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(AfterInitialization));

        private static readonly ProfilerMarker _PRF_FindAudioListener =
            new ProfilerMarker(_PRF_PFX + nameof(FindAudioListener));

        private static readonly ProfilerMarker _PRF_InitializeParameters =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeParameters));

        private static readonly ProfilerMarker _PRF_OnSceneLoaded =
            new ProfilerMarker(_PRF_PFX + nameof(OnSceneLoaded));

        #endregion
    }
}