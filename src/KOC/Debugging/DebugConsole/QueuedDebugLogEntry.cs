using System;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Debugging.DebugConsole
{
    public readonly struct QueuedDebugLogEntry
    {
        public QueuedDebugLogEntry(string logString, string stackTrace, LogType logType)
        {
            this.logString = logString;
            this.stackTrace = stackTrace;
            this.logType = logType;
        }

        public readonly LogType logType;
        public readonly string logString;
        public readonly string stackTrace;

        // Checks if logString or stackTrace contains the search term
        public bool MatchesSearchTerm(string searchTerm)
        {
            return ((logString != null) &&
                    (logString.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)) ||
                   ((stackTrace != null) &&
                    (stackTrace.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0));
        }
    }
}
