using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Appalachia.Prototype.KOC.Character.Settings
{
    [Serializable]
    public struct Breathing
    {
        

        [Range(0, 10)] public float breathingPeriod;
        [Range(0, 10)] public float breathingPeriodIntensityFactor;
        [Range(0, 1)] public float breathingPeriodVariance;

        [Range(0, 5)] public float exhalePeriod;
        [Range(0, 1)] public float exhalePeriodVariance;

        [Range(0, 5)] public float inhalePeriod;
        [Range(0, 1)] public float inhalePeriodPacingFactor;
        [Range(0, 1)] public float inhalePeriodVariance;
        [Range(0, 5)] public float initialDelay;
        [Range(0, 1)] public float initialDelayVariance;

        [Range(0, 1)] public float intensityDampening;
        [Range(0, 1)] public float intensityTransference;

        [Range(0, 1)] public float volumeOverPace;
        [Range(0, 1)] public float volumeOverPaceVariance;


        public float GetBreathingPeriod()
        {
            return breathingPeriod + Random.Range(-breathingPeriodVariance, breathingPeriodVariance);
        }

        public float GetExhalePeriod()
        {
            return exhalePeriod + Random.Range(-exhalePeriodVariance, exhalePeriodVariance);
        }

        public float GetInhalePeriod()
        {
            return inhalePeriod + Random.Range(-inhalePeriodVariance, inhalePeriodVariance);
        }

        public float GetInitialDelay()
        {
            return initialDelay + Random.Range(-initialDelayVariance, initialDelayVariance);
        }

        public float GetVolumeOverPace()
        {
            return volumeOverPace + Random.Range(-volumeOverPaceVariance, volumeOverPaceVariance);
        }

        #region IEquatable

        #endregion
    }
}
