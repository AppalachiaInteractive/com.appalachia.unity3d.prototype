using System;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

namespace Appalachia.Prototype.KOC.Features.Character.Settings
{
    [Serializable]
    public struct Breathing
    {
        #region Fields and Autoproperties

        [PropertyRange(0, 10)] public float breathingPeriod;
        [PropertyRange(0, 10)] public float breathingPeriodIntensityFactor;
        [PropertyRange(0, 1)] public float breathingPeriodVariance;

        [PropertyRange(0, 5)] public float exhalePeriod;
        [PropertyRange(0, 1)] public float exhalePeriodVariance;

        [PropertyRange(0, 5)] public float inhalePeriod;
        [PropertyRange(0, 1)] public float inhalePeriodPacingFactor;
        [PropertyRange(0, 1)] public float inhalePeriodVariance;
        [PropertyRange(0, 5)] public float initialDelay;
        [PropertyRange(0, 1)] public float initialDelayVariance;

        [PropertyRange(0, 1)] public float intensityDampening;
        [PropertyRange(0, 1)] public float intensityTransference;

        [PropertyRange(0, 1)] public float volumeOverPace;
        [PropertyRange(0, 1)] public float volumeOverPaceVariance;

        #endregion

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
