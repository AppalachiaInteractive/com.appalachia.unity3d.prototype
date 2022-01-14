using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Util
{
    internal static class G_FloatString
    {
        #region Constants and Static Readonly

        /// <summary>
        ///     The currently defined, globally used decimal multiplier.
        /// </summary>
        private static readonly float m_decimalMultiplier = 10f;

        /// <summary>
        ///     Float represented as a string, formatted.
        /// </summary>
        private const string m_floatFormat = "0.0";

        #endregion

        #region Static Fields and Autoproperties

        /// <summary>
        ///     List of negative floats casted to strings.
        /// </summary>
        private static string[] m_negativeBuffer = new string[0];

        /// <summary>
        ///     List of positive floats casted to strings.
        /// </summary>
        private static string[] m_positiveBuffer = new string[0];

        #endregion

        public static float MaxValue => (m_positiveBuffer.Length - 1).FromIndex();

        public static float MinValue => -(m_negativeBuffer.Length - 1).FromIndex();

        internal static void Dispose()
        {
            m_negativeBuffer = new string[0];
            m_positiveBuffer = new string[0];
        }

        internal static void Init(float minNegativeValue, float maxPositiveValue)
        {
            var negativeLength = minNegativeValue.ToIndex();
            var positiveLength = maxPositiveValue.ToIndex();

            if ((MinValue > minNegativeValue) && (negativeLength >= 0))
            {
                m_negativeBuffer = new string[negativeLength];
                for (var i = 0; i < negativeLength; i++)
                {
                    m_negativeBuffer[i] = (-i - 1).FromIndex().ToString(m_floatFormat);
                }
            }

            if ((MaxValue < maxPositiveValue) && (positiveLength >= 0))
            {
                m_positiveBuffer = new string[positiveLength + 1];
                for (var i = 0; i < (positiveLength + 1); i++)
                {
                    m_positiveBuffer[i] = i.FromIndex().ToString(m_floatFormat);
                }
            }
        }

        internal static float ToFloat(this int i)
        {
            return i;
        }

        internal static int ToInt(this float f)
        {
            return (int)f;
        }

        [DebuggerStepThrough]
        internal static string ToStringNonAlloc(this float value, string format)
        {
            var valIndex = value.ToIndex();

            if ((value < 0) && (valIndex < m_negativeBuffer.Length))
            {
                return m_negativeBuffer[valIndex];
            }

            if ((value >= 0) && (valIndex < m_positiveBuffer.Length))
            {
                return m_positiveBuffer[valIndex];
            }

            return value.ToString(format);
        }

        private static float FromIndex(this int i)
        {
            return i.ToFloat() / m_decimalMultiplier;
        }

        private static int ToIndex(this float f)
        {
            return Mathf.Abs((f * m_decimalMultiplier).ToInt());
        }
    }
}
