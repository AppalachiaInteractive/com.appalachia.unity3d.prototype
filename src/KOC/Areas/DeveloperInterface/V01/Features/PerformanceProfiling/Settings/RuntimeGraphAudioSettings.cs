using System;
using Appalachia.CI.Integration.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Settings
{
    [DoNotReorderFields, Serializable]
    public class RuntimeGraphAudioSettings : IRuntimeGraphSettings
    {
        #region Constants and Static Readonly

        private static readonly int[] spectrumSizes = { 64, 128, 256, 512, 1024, 2048, 4096, 8192 };

        #endregion

        #region Fields and Autoproperties

        [FormerlySerializedAs("findAudioListenerInCameraIfNull")]
        public LookForAudioListener findListenerIfNull;

        public Color audioGraphColor;

        [PropertyRange(10, 300)] public int graphResolution;
        [PropertyRange(1,  200)] public int textUpdateRate;

        public FFTWindow FFTWindow;

        [ValueDropdown(nameof(spectrumSizes))]
        [PropertyTooltip("Must be a power of 2 and between 64-8192")]
        public int spectrumSize;

        #endregion

        #region IRuntimeGraphSettings Members

        public int GraphResolution => graphResolution;
        public int TextUpdateRate => textUpdateRate;

        public void Reset()
        {
            findListenerIfNull = LookForAudioListener.ON_SCENE_LOAD;
            audioGraphColor = Color.white;
            graphResolution = 81;
            textUpdateRate = 3;
            FFTWindow = FFTWindow.Blackman;
            spectrumSize = 512;
        }

        #endregion
    }
}
