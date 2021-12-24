using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Debugging.DebugConsole.Collections;
using Appalachia.Prototype.KOC.Debugging.DebugConsole.Reflection;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Debugging.DebugConsole.Settings
{
    [Serializable]
    public class DebugLogManagerState : AppalachiaSimpleBase
    {
        #region Fields and Autoproperties

        public bool isCollapseOn;
        public bool isInSearchMode;
        public bool isLogWindowVisible;
        public bool screenDimensionsChanged;
        public bool shouldUpdateRecycledListView;
        public bool snapToBottom;
        public CircularBuffer<string> commandHistory;
        public DebugLogFilter logFilter;
        public DebugLogIndexList indicesOfListEntriesToShow;
        public DebugLogIndexList uncollapsedLogEntriesIndices;
        public Dictionary<DebugLogEntry, int> collapsedLogEntriesMap;
        public Dictionary<LogType, Sprite> logSpriteRepresentations;
        public DynamicCircularBuffer<QueuedDebugLogEntry> queuedLogEntries;
        public float logWindowPreviousWidth;
        public int commandHistoryIndex;
        public int commandInputFieldPrevCaretArgumentIndex;
        public int commandInputFieldPrevCaretPos;
        public int commandInputFieldPrevParamCount;
        public int errorEntryCount;
        public int indexOfLogEntryToSelectAndFocus;
        public int infoEntryCount;
        public int newErrorEntryCount;
        public int newInfoEntryCount;
        public int newWarningEntryCount;
        public int pendingLogToAutoExpand;
        public int visibleCommandSuggestionInstances;
        public int warningEntryCount;
        public List<ConsoleMethodInfo> matchingCommandSuggestions;
        public List<DebugLogEntry> collapsedLogEntries;
        public List<DebugLogEntry> pooledLogEntries;
        public List<DebugLogItem> pooledLogItems;
        public List<int> commandCaretIndexIncrements;
        public List<Text> commandSuggestionInstances;
        public object logEntriesLock;
        public PointerEventData nullPointerEventData;
        public string commandInputFieldPrevCommand;
        public string commandInputFieldPrevCommandName;
        public string searchTerm;
        public string unfinishedCommand;
        public Utf16ValueStringBuilder commandSuggestionsStringBuilder;
        public Vector2 logItemsScrollRectOriginalSize;

        #endregion

        public void InitializeState(DebugLogManagerSettings settings)
        {
            using (_PRF_InitializeState.Auto())
            {
                collapsedLogEntries ??= new List<DebugLogEntry>(128);
                collapsedLogEntriesMap ??= new Dictionary<DebugLogEntry, int>(128);
                commandCaretIndexIncrements ??= new List<int>(8);
                commandHistory ??= new CircularBuffer<string>(settings.command.commandHistorySize);
                commandHistoryIndex = -1;
                commandInputFieldPrevCaretArgumentIndex = -1;
                commandInputFieldPrevCaretPos = -1;
                commandInputFieldPrevParamCount = -1;
                commandSuggestionInstances ??= new List<Text>(8);
                commandSuggestionsStringBuilder = new Utf16ValueStringBuilder(false);
                commandSuggestionsStringBuilder = new Utf16ValueStringBuilder(false);
                indexOfLogEntryToSelectAndFocus = -1;
                indicesOfListEntriesToShow ??= new DebugLogIndexList();
                isLogWindowVisible = true;
                logEntriesLock ??= new object();
                logFilter = DebugLogFilter.All;
                logSpriteRepresentations = new Dictionary<LogType, Sprite>();
                matchingCommandSuggestions ??= new List<ConsoleMethodInfo>(8);
                nullPointerEventData ??= new PointerEventData(null);
                pooledLogEntries ??= new List<DebugLogEntry>(16);
                pooledLogItems ??= new List<DebugLogItem>(16);
                queuedLogEntries ??= new DynamicCircularBuffer<QueuedDebugLogEntry>(16);
                screenDimensionsChanged = true;
                snapToBottom = true;
                uncollapsedLogEntriesIndices ??= new DebugLogIndexList();

                // Associate sprites with log types
                logSpriteRepresentations = new Dictionary<LogType, Sprite>
                {
                    { LogType.Log, settings.visuals.infoLog },
                    { LogType.Warning, settings.visuals.warningLog },
                    { LogType.Error, settings.visuals.errorLog },
                    { LogType.Exception, settings.visuals.errorLog },
                    { LogType.Assert, settings.visuals.errorLog }
                };
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(DebugLogManagerState) + ".";

        private static readonly ProfilerMarker _PRF_InitializeState =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeState));

        #endregion
    }
}
