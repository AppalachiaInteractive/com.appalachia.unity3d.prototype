using System;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Model
{
    public readonly struct QueuedDebugLogEntry : IComparable<QueuedDebugLogEntry>, IComparable
    {
        public QueuedDebugLogEntry(string logString, string stackTrace, LogType logType)
        {
            this.logString = logString;
            this.stackTrace = stackTrace;
            this.logType = logType;
        }

        #region Fields and Autoproperties

        public readonly LogType logType;
        public readonly string logString;
        public readonly string stackTrace;

        #endregion

        public static bool operator >(QueuedDebugLogEntry left, QueuedDebugLogEntry right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(QueuedDebugLogEntry left, QueuedDebugLogEntry right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <(QueuedDebugLogEntry left, QueuedDebugLogEntry right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(QueuedDebugLogEntry left, QueuedDebugLogEntry right)
        {
            return left.CompareTo(right) <= 0;
        }

        // Checks if logString or stackTrace contains the search term
        public bool MatchesSearchTerm(string searchTerm)
        {
            return ((logString != null) &&
                    (logString.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)) ||
                   ((stackTrace != null) &&
                    (stackTrace.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0));
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return 1;
            }

            return obj is QueuedDebugLogEntry other
                ? CompareTo(other)
                : throw new ArgumentException($"Object must be of type {nameof(QueuedDebugLogEntry)}");
        }

        #endregion

        #region IComparable<QueuedDebugLogEntry> Members

        public int CompareTo(QueuedDebugLogEntry other)
        {
            var logTypeComparison = logType.CompareTo(other.logType);
            if (logTypeComparison != 0)
            {
                return logTypeComparison;
            }

            var logStringComparison = string.Compare(logString, other.logString, StringComparison.Ordinal);
            if (logStringComparison != 0)
            {
                return logStringComparison;
            }

            return string.Compare(stackTrace, other.stackTrace, StringComparison.Ordinal);
        }

        #endregion
    }
}
