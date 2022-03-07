﻿using System;
using System.Diagnostics;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Util
{
    internal static class G_IntString
    {
        #region Static Fields and Autoproperties

        /// <summary>
        ///     List of negative ints casted to strings.
        /// </summary>
        private static string[] m_negativeBuffer = Array.Empty<string>();

        /// <summary>
        ///     List of positive ints casted to strings.
        /// </summary>
        private static string[] m_positiveBuffer = Array.Empty<string>();

        #endregion

        /// <summary>
        ///     The highest int value of the existing number buffer.
        /// </summary>
        public static int MaxValue => m_positiveBuffer.Length;

        /// <summary>
        ///     The lowest int value of the existing number buffer.
        /// </summary>
        public static int MinValue => -(m_negativeBuffer.Length - 1);

        public static void Dispose()
        {
            m_negativeBuffer = Array.Empty<string>();
            m_positiveBuffer = Array.Empty<string>();
        }

        /// <summary>
        ///     Initialize the buffers.
        /// </summary>
        /// <param name="minNegativeValue">
        ///     Lowest negative value allowed.
        /// </param>
        /// <param name="maxPositiveValue">
        ///     Highest positive value allowed.
        /// </param>
        public static void Init(int minNegativeValue, int maxPositiveValue)
        {
            if ((MinValue > minNegativeValue) && (minNegativeValue <= 0))
            {
                var length = Mathf.Abs(minNegativeValue);

                m_negativeBuffer = new string[length];

                for (var i = 0; i < length; i++)
                {
                    m_negativeBuffer[i] = (-i - 1).ToString();
                }
            }

            if ((MaxValue < maxPositiveValue) && (maxPositiveValue >= 0))
            {
                m_positiveBuffer = new string[maxPositiveValue + 1];

                for (var i = 0; i < (maxPositiveValue + 1); i++)
                {
                    m_positiveBuffer[i] = i.ToString();
                }
            }
        }

        /// <summary>
        ///     Returns this int as a cached string.
        /// </summary>
        /// <param name="value">
        ///     The required int.
        /// </param>
        /// <returns>
        ///     A cached number string if within the buffer ranges.
        /// </returns>
        [DebuggerStepThrough]
        public static string ToStringNonAlloc(this int value)
        {
            if ((value < 0) && (-value <= m_negativeBuffer.Length))
            {
                return m_negativeBuffer[-value - 1];
            }

            if ((value >= 0) && (value < m_positiveBuffer.Length))
            {
                return m_positiveBuffer[value];
            }

            // If the value is not within the buffer ranges, just do a normal .ToString()
            return value.ToString();
        }
    }
}