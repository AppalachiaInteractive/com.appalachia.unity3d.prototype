using System;
using Appalachia.CI.Integration.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Settings
{
    [DoNotReorderFields, Serializable]
    public class RuntimeGraphRamSettings : IRuntimeGraphSettings
    {
        #region Fields and Autoproperties

        public Color allocatedRamColor;
        public Color reservedRamColor;
        public Color monoRamColor;

        [PropertyRange(10, 300)] public int graphResolution;
        [PropertyRange(1,  200)] public int textUpdateRate;

        #endregion

        #region IRuntimeGraphSettings Members

        public int GraphResolution => graphResolution;
        public int TextUpdateRate => textUpdateRate;

        public void Reset()
        {
            allocatedRamColor = new Color32(255, 190, 60,  255);
            reservedRamColor = new Color32(205,  84,  229, 255);
            monoRamColor = new(0.3f, 0.65f, 1f, 1);
            graphResolution = 150;
            textUpdateRate = 3; // 3 updates per sec.
        }

        #endregion
    }
}
