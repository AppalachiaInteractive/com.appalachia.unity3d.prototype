using System;
using Appalachia.CI.Integration.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Settings
{
    [DoNotReorderFields, Serializable]
    public class RuntimeGraphFpsSettings : IRuntimeGraphSettings
    {
        #region Fields and Autoproperties

        public Color goodFpsColor;
        public int goodFpsThreshold;
        public Color cautionFpsColor;
        public int cautionFpsThreshold;
        public Color criticalFpsColor;
        [PropertyRange(10, 300)] public int graphResolution;
        [PropertyRange(1,  200)] public int textUpdateRate;

        #endregion

        #region IRuntimeGraphSettings Members

        public int GraphResolution => graphResolution;
        public int TextUpdateRate => textUpdateRate;

        public void Reset()
        {
            goodFpsColor = new Color32(118, 212, 58, 255);
            goodFpsThreshold = 60;
            cautionFpsColor = new Color32(243, 232, 0, 255);
            cautionFpsThreshold = 30;
            criticalFpsColor = new Color32(220, 41, 30, 255);
            graphResolution = 150;
            textUpdateRate = 3;
        }

        #endregion
    }
}
