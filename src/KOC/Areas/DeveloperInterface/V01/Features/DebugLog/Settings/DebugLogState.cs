using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Collections;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Components;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Model;
using Appalachia.Utility.DataStructures.Lists;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Settings
{
    [Serializable]
    public class DebugLogState : AppalachiaSimpleBase
    {
        #region Fields and Autoproperties

        [NonSerialized] private bool _initialized;

        public bool isCollapseOn;
        public bool isInSearchMode;
        public bool isLogWindowVisible;
        public bool screenDimensionsChanged;
        public bool shouldUpdateRecycledListView;
        public bool snapToBottom;
        public DebugLogFilter logFilter;
        public DebugLogIndexList indicesOfListEntriesToShow;
        public DebugLogIndexList uncollapsedLogEntriesIndices;
        public Dictionary<DebugLogEntry, int> collapsedLogEntriesMap;
        public Dictionary<LogType, Sprite> logSpriteRepresentations;
        public DynamicCircularBuffer<QueuedDebugLogEntry> queuedLogEntries;
        public float logWindowPreviousWidth;
        public int errorEntryCount;
        public int indexOfLogEntryToSelectAndFocus;
        public int infoEntryCount;
        public int newErrorEntryCount;
        public int newInfoEntryCount;
        public int newWarningEntryCount;
        public int pendingLogToAutoExpand;
        public int warningEntryCount;
        public List<DebugLogEntry> collapsedLogEntries;
        public List<DebugLogEntry> pooledLogEntries;
        public List<DebugLogItem> pooledLogItems;
        public object logEntriesLock;
        public PointerEventData nullPointerEventData;
        public string searchTerm;
        public Vector2 logItemsScrollRectOriginalSize;

        #endregion

        public bool initialized
        {
            get => _initialized;
            private set => _initialized = value;
        }

        public void InitializeState(DebugLogSettings settings)
        {
            using (_PRF_InitializeState.Auto())
            {
                collapsedLogEntries = new List<DebugLogEntry>(128);
                collapsedLogEntriesMap = new Dictionary<DebugLogEntry, int>(128);
                indexOfLogEntryToSelectAndFocus = -1;
                indicesOfListEntriesToShow = new DebugLogIndexList();
                isLogWindowVisible = true;
                logEntriesLock = new object();
                logFilter = DebugLogFilter.All;
                logSpriteRepresentations = new Dictionary<LogType, Sprite>();
                nullPointerEventData = new PointerEventData(null);
                pooledLogEntries = new List<DebugLogEntry>(16);
                pooledLogItems = new List<DebugLogItem>(16);
                queuedLogEntries = new DynamicCircularBuffer<QueuedDebugLogEntry>(16);
                screenDimensionsChanged = true;
                snapToBottom = true;
                uncollapsedLogEntriesIndices = new DebugLogIndexList();

                // Associate sprites with log types
                logSpriteRepresentations = new Dictionary<LogType, Sprite>
                {
                    { LogType.Log, settings.visuals.infoLog },
                    { LogType.Warning, settings.visuals.warningLog },
                    { LogType.Error, settings.visuals.errorLog },
                    { LogType.Exception, settings.visuals.errorLog },
                    { LogType.Assert, settings.visuals.errorLog }
                };

                initialized = true;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(DebugLogState) + ".";

        private static readonly ProfilerMarker _PRF_InitializeState =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeState));

        #endregion
    }
}
