using System;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Utilities
{
    [Serializable]
    public class CachedNumberString
    {
        public CachedNumberString(float minimum, float maximum, int decimalPlaces)
        {
            using (_PRF_CachedNumberString.Auto())
            {
                _decimalPlaces = decimalPlaces;
                _minimum = Math.Round(minimum, decimalPlaces);
                _maximum = Math.Round(maximum, decimalPlaces);

                Initialize();
            }
        }

        #region Fields and Autoproperties

        [SerializeField] private double _maximum;

        [SerializeField] private double _minimum;
        [SerializeField] private int _decimalPlaces;

        [NonSerialized] private string _formatString;

        [NonSerialized] private string[] _cachedValues;

        #endregion

        public double Maximum => _maximum;

        public double Minimum => _minimum;
        public int DecimalPlaces => _decimalPlaces;

        protected string[] CachedValues
        {
            get
            {
                if (_cachedValues == null)
                {
                    Initialize();
                }

                return _cachedValues;
            }
        }

        private double Range => Maximum - Minimum;
        private double Increment => Range / Length;
        private int Length => (int)(Range * DecimalPlaces);

        public string ToString(float f)
        {
            using (_PRF_ToString.Auto())
            {
                var index = ToIndex(f);

                return CachedValues[index];
            }
        }

        private double FromIndex(int index)
        {
            using (_PRF_FromIndex.Auto())
            {
                var starting = Minimum;

                var additive = Increment * index;

                var result = starting + additive;

                return result;
            }
        }

        private void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                _cachedValues = new string[Length];

                _formatString = "0";

                for (var i = 0; i < DecimalPlaces; i++)
                {
                    if (i == 0)
                    {
                        _formatString += ".";
                    }

                    _formatString += "0";
                }

                var currentValue = Minimum;

                for (var i = 0; i < _cachedValues.Length; i++)
                {
                    var resultingString = currentValue.ToString(_formatString);
                    _cachedValues[i] = resultingString;

                    currentValue += Increment;
                }
            }
        }

        private int ToIndex(double f)
        {
            using (_PRF_ToIndex.Auto())
            {
                var distanceFromMinimum = f - Minimum;

                var steps = distanceFromMinimum / Increment;

                var index = (int)steps;

                return index;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(CachedNumberString) + ".";

        private static readonly ProfilerMarker _PRF_FromIndex =
            new ProfilerMarker(_PRF_PFX + nameof(FromIndex));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_ToString =
            new ProfilerMarker(_PRF_PFX + nameof(ToString));

        private static readonly ProfilerMarker _PRF_ToIndex = new ProfilerMarker(_PRF_PFX + nameof(ToIndex));

        private static readonly ProfilerMarker _PRF_CachedNumberString =
            new ProfilerMarker(_PRF_PFX + nameof(CachedNumberString));

        #endregion
    }
}
