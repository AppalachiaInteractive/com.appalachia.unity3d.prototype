/*
using System;
using System.Collections;
using System.Text;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Debugging.DebugLog;
using Appalachia.Prototype.KOC.Debugging.DebugLog.Components;
using Appalachia.Prototype.KOC.Debugging.DeveloperConsole.Settings;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
#if UNITY_EDITOR && UNITY_2021_1_OR_NEWER
using Screen = UnityEngine.Device.Screen; // To support Device Simulator on Unity 2021.1+
#endif

namespace Appalachia.Prototype.KOC.Debugging.DeveloperConsole
{
    [CallStaticConstructorInEditor]
    [ExecutionOrder(ExecutionOrders.DebugLogManager)]
    public partial class DeveloperConsoleManager : SingletonAppalachiaBehaviour<DeveloperConsoleManager>
    {
        static DeveloperConsoleManager()
        {
            RegisterDependency<DeveloperConsoleSettings>(i => _developerConsoleSettings = i);
        }

        public DeveloperConsoleManager(Action onCommandPaletteShown)
        {
            OnCommandPaletteShown = onCommandPaletteShown;
        }

        #region Static Fields and Autoproperties

        [ShowInInspector, InlineEditor, HideLabel, FoldoutGroup("Settings")]
        private static DeveloperConsoleSettings _developerConsoleSettings;

        #endregion

        #region Fields and Autoproperties

        public DeveloperConsoleSettings settings => _developerConsoleSettings;

        [InlineProperty, HideLabel, FoldoutGroup("References")]
        public DeveloperConsoleReferences references;

        [InlineProperty, HideLabel, FoldoutGroup("State")]
        public DeveloperConsoleState state;

        // Callbacks for log window show/hide events
        public Action OnCommandPaletteShown, OnCommandPaletteHidden;

        #endregion

        public bool IsCommandPaletteVisible => state.isCommandPaletteVisible;

        #region Event Functions

        private void LateUpdate()
        {
            using (_PRF_LateUpdate.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                InitializeState();

                var queuedLogCount = state.queuedLogEntries.Count;
                if (queuedLogCount > 0)
                {
                    for (var i = 0; i < queuedLogCount; i++)
                    {
                        QueuedDebugLogEntry logEntry;
                        lock (state.logEntriesLock)
                        {
                            logEntry = state.queuedLogEntries.RemoveFirst();
                        }

                        ProcessLog(logEntry);
                    }
                }

                // Update entry count texts in a single batch
                if ((state.newInfoEntryCount > 0) ||
                    (state.newWarningEntryCount > 0) ||
                    (state.newErrorEntryCount > 0))
                {
                    if (state.newInfoEntryCount > 0)
                    {
                        state.infoEntryCount += state.newInfoEntryCount;
                        references.infoEntryCountText.text = state.infoEntryCount.ToString();
                    }

                    if (state.newWarningEntryCount > 0)
                    {
                        state.warningEntryCount += state.newWarningEntryCount;
                        references.warningEntryCountText.text = state.warningEntryCount.ToString();
                    }

                    if (state.newErrorEntryCount > 0)
                    {
                        state.errorEntryCount += state.newErrorEntryCount;
                        references.errorEntryCountText.text = state.errorEntryCount.ToString();
                    }

                    // If debug popup is visible, notify it of the new debug entries
                    if (!state.isCommandPaletteVisible)
                    {
                        references.popupManager.NewLogsArrived(
                            state.newInfoEntryCount,
                            state.newWarningEntryCount,
                            state.newErrorEntryCount
                        );
                    }

                    state.newInfoEntryCount = 0;
                    state.newWarningEntryCount = 0;
                    state.newErrorEntryCount = 0;
                }

                // Update visible logs if necessary
                if (state.isCommandPaletteVisible && state.shouldUpdateRecycledListView)
                {
                    references.recycledListView.OnLogEntriesUpdated(false);
                    state.shouldUpdateRecycledListView = false;
                }

                // Automatically expand the target log (if any)
                if (state.indexOfLogEntryToSelectAndFocus >= 0)
                {
                    if (state.indexOfLogEntryToSelectAndFocus < state.indicesOfListEntriesToShow.Count)
                    {
                        references.recycledListView.SelectAndFocusOnLogItemAtIndex(
                            state.indexOfLogEntryToSelectAndFocus
                        );
                    }

                    state.indexOfLogEntryToSelectAndFocus = -1;
                }

                if (settings.command.showCommandSuggestions &&
                    references.commandInputField.isFocused &&
                    (references.commandInputField.caretPosition != state.commandInputFieldPrevCaretPos))
                {
                    RefreshCommandSuggestions(references.commandInputField.text);
                }

                if (state.screenDimensionsChanged)
                {
                    // Update the recycled list view
                    if (state.isCommandPaletteVisible)
                    {
                        references.recycledListView.OnViewportHeightChanged();
                    }
                    else
                    {
                        references.popupManager.UpdatePosition(true);
                    }

#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
                    CheckScreenCutout();
#endif

                    state.screenDimensionsChanged = false;
                }

                var logWindowWidth = references.logWindowTR.rect.width;
                if (!Mathf.Approximately(logWindowWidth, state.logWindowPreviousWidth))
                {
                    state.logWindowPreviousWidth = logWindowWidth;

                    if (references.searchbar)
                    {
                        if (logWindowWidth >= settings.general.topSearchbarMinWidth)
                        {
                            if (references.searchbar.parent == references.searchbarSlotBottom)
                            {
                                references.searchbarSlotTop.gameObject.SetActive(true);
                                references.searchbar.SetParent(references.searchbarSlotTop, false);
                                references.searchbarSlotBottom.gameObject.SetActive(false);

                                references.logItemsScrollRectTR.anchoredPosition = Vector2.zero;
                                references.logItemsScrollRectTR.sizeDelta =
                                    references.logItemsScrollRectOriginalSize;
                            }
                        }
                        else
                        {
                            if (references.searchbar.parent == references.searchbarSlotTop)
                            {
                                references.searchbarSlotBottom.gameObject.SetActive(true);
                                references.searchbar.SetParent(references.searchbarSlotBottom, false);
                                references.searchbarSlotTop.gameObject.SetActive(false);

                                var searchbarHeight = references.searchbarSlotBottom.sizeDelta.y;
                                references.logItemsScrollRectTR.anchoredPosition = new Vector2(
                                    0f,
                                    searchbarHeight * -0.5f
                                );
                                references.logItemsScrollRectTR.sizeDelta =
                                    references.logItemsScrollRectOriginalSize -
                                    new Vector2(0f, searchbarHeight);
                            }
                        }
                    }

                    if (state.isCommandPaletteVisible)
                    {
                        references.recycledListView.OnViewportWidthChanged();
                    }
                }

                // If snapToBottom is enabled, force the scrollbar to the bottom
                if (state.snapToBottom)
                {
                    references.logItemsScrollRect.verticalNormalizedPosition = 0f;

                    if (references.snapToBottomButton.activeSelf)
                    {
                        references.snapToBottomButton.SetActive(false);
                    }
                }
                else
                {
                    var scrollPos = references.logItemsScrollRect.verticalNormalizedPosition;
                    if (references.snapToBottomButton.activeSelf !=
                        ((scrollPos > 1E-6f) && (scrollPos < 0.9999f)))
                    {
                        references.snapToBottomButton.SetActive(!references.snapToBottomButton.activeSelf);
                    }
                }

                if (state.isCommandPaletteVisible &&
                    references.commandInputField.isFocused &&
                    (state.commandHistory.Count > 0))
                {
                    if (Keyboard.current != null)
                    {
                        if (Keyboard.current[Key.UpArrow].wasPressedThisFrame)
                        {
                            if (state.commandHistoryIndex == -1)
                            {
                                state.commandHistoryIndex = state.commandHistory.Count - 1;
                                state.unfinishedCommand = references.commandInputField.text;
                            }
                            else if (--state.commandHistoryIndex < 0)
                            {
                                state.commandHistoryIndex = 0;
                            }

                            references.commandInputField.text =
                                state.commandHistory[state.commandHistoryIndex];
                            references.commandInputField.caretPosition =
                                references.commandInputField.text.Length;
                        }
                        else if (Keyboard.current[Key.DownArrow].wasPressedThisFrame &&
                                 (state.commandHistoryIndex != -1))

                        {
                            if (++state.commandHistoryIndex < state.commandHistory.Count)
                            {
                                references.commandInputField.text =
                                    state.commandHistory[state.commandHistoryIndex];
                            }
                            else
                            {
                                state.commandHistoryIndex = -1;
                                references.commandInputField.text = state.unfinishedCommand ?? string.Empty;
                            }
                        }
                    }
                }

#if !UNITY_EDITOR && UNITY_ANDROID
			if( logcatListener != null )
			{
				string log;
				while( ( log = logcatListener.GetLog() ) != null )
					ReceivedLog( "LOGCAT: " + log, string.Empty, LogType.Log );
			}
#endif
            }
        }

        // Window is resized, update the list
        private void OnRectTransformDimensionsChange()
        {
            using (_PRF_OnRectTransformDimensionsChange.Auto())
            {
                InitializeState();

                state.screenDimensionsChanged = true;
            }
        }

        #endregion

        // Clear all the logs
        public void ClearLogs()
        {
            using (_PRF_ClearLogs.Auto())
            {
                state.snapToBottom = true;

                state.infoEntryCount = 0;
                state.warningEntryCount = 0;
                state.errorEntryCount = 0;

                references.infoEntryCountText.text = "0";
                references.warningEntryCountText.text = "0";
                references.errorEntryCountText.text = "0";

                state.collapsedLogEntries.Clear();
                state.collapsedLogEntriesMap.Clear();
                state.uncollapsedLogEntriesIndices.Clear();
                state.indicesOfListEntriesToShow.Clear();

                references.recycledListView.DeselectSelectedLogItem();
                references.recycledListView.OnLogEntriesUpdated(true);
            }
        }

        // Automatically expand the latest log in queuedLogEntries
        public void ExpandLatestPendingLog()
        {
            using (_PRF_ExpandLatestPendingLog.Auto())
            {
                state.pendingLogToAutoExpand = state.queuedLogEntries.Count;
            }
        }

        public string GetAllLogs()
        {
            using (_PRF_GetAllLogs.Auto())
            {
                var count = state.uncollapsedLogEntriesIndices.Count;
                var length = 0;
                var newLineLength = Environment.NewLine.Length;
                for (var i = 0; i < count; i++)
                {
                    var entry = state.collapsedLogEntries[state.uncollapsedLogEntriesIndices[i]];
                    length += entry.logString.Length + entry.stackTrace.Length + (newLineLength * 3);
                }

                length += 100; // Just in case...

                var sb = new StringBuilder(length);
                for (var i = 0; i < count; i++)
                {
                    var entry = state.collapsedLogEntries[state.uncollapsedLogEntriesIndices[i]];
                    sb.AppendLine(entry.logString).AppendLine(entry.stackTrace).AppendLine();
                }

                return sb.ToString();
            }
        }

        public void HideLogWindow()
        {
            using (_PRF_HideLogWindow.Auto())
            {
                // Hide the log window
                references.logWindowCanvasGroup.interactable = false;
                references.logWindowCanvasGroup.blocksRaycasts = false;
                references.logWindowCanvasGroup.alpha = 0f;

                if (references.commandInputField.isFocused)
                {
                    references.commandInputField.DeactivateInputField();
                }

                if (references.popupManager.FullyInitialized)
                {
                    references.popupManager.Show();
                }
                else
                {
                    references.popupManager.InitializationComplete += p => references.popupManager.Show();
                }

                state.isCommandPaletteVisible = false;

                if (OnCommandPaletteHidden != null)
                {
                    OnCommandPaletteHidden();
                }
            }
        }

        // A debug entry is received
        public void ReceivedLog(string logString, string stackTrace, LogType logType)
        {
            using (_PRF_ReceivedLog.Auto())
            {
                // Truncate the log if it is longer than maxLogLength
                var logLength = logString.Length;
                if (stackTrace == null)
                {
                    if (logLength > settings.general.maxLogLength)
                    {
                        logString = logString.Substring(0, settings.general.maxLogLength - 11) +
                                    "<truncated>";
                    }
                }
                else
                {
                    logLength += stackTrace.Length;
                    if (logLength > settings.general.maxLogLength)
                    {
                        // Decide which log component(s) to truncate
                        var halfMaxLogLength = settings.general.maxLogLength / 2;
                        if (logString.Length >= halfMaxLogLength)
                        {
                            if (stackTrace.Length >= halfMaxLogLength)
                            {
                                // Truncate both logString and stackTrace
                                logString = logString.Substring(0, halfMaxLogLength - 11) + "<truncated>";

                                // If stackTrace doesn't end with a blank line, its last line won't be visible in the console for some reason
                                stackTrace = stackTrace.Substring(0, halfMaxLogLength - 12) + "<truncated>\n";
                            }
                            else
                            {
                                // Truncate logString
                                logString = logString.Substring(
                                                0,
                                                settings.general.maxLogLength - stackTrace.Length - 11
                                            ) +
                                            "<truncated>";
                            }
                        }
                        else
                        {
                            // Truncate stackTrace
                            stackTrace = stackTrace.Substring(
                                             0,
                                             settings.general.maxLogLength - logString.Length - 12
                                         ) +
                                         "<truncated>\n";
                        }
                    }
                }

                var queuedLogEntry = new QueuedDebugLogEntry(logString, stackTrace, logType);

                lock (state.logEntriesLock)
                {
                    state.queuedLogEntries.Add(queuedLogEntry);
                }
            }
        }

        // Value of snapToBottom is changed (user scrolled the list manually)
        public void SetSnapToBottom(bool snapToBottom)
        {
            using (_PRF_SetSnapToBottom.Auto())
            {
                state.snapToBottom = snapToBottom;
            }
        }

        public void ShowLogWindow()
        {
            using (_PRF_ShowLogWindow.Auto())
            {
                // Show the log window
                references.logWindowCanvasGroup.interactable = true;
                references.logWindowCanvasGroup.blocksRaycasts = true;
                references.logWindowCanvasGroup.alpha = settings.window.alpha;

                if (references.popupManager.FullyInitialized)
                {
                    references.popupManager.Hide();
                }
                else
                {
                    references.popupManager.InitializationComplete += p => references.popupManager.Hide();
                }

                // Update the recycled list view 
                // (in case new entries were intercepted while log window was hidden)
                references.recycledListView.OnLogEntriesUpdated(true);

#if UNITY_EDITOR || UNITY_STANDALONE

                // Focus on the command input field on standalone platforms when the console is opened
                if (settings.command.autoFocusOnCommandInputField)
                {
                    StartCoroutine(ActivateCommandInputFieldCoroutine());
                }
#endif

                state.isCommandPaletteVisible = true;

                if (OnCommandPaletteShown != null)
                {
                    OnCommandPaletteShown();
                }
            }
        }

        // Omits the latest log's stack trace
        public void StripStackTraceFromLatestPendingLog()
        {
            using (_PRF_StripStackTraceFromLatestPendingLog.Auto())
            {
                var log = state.queuedLogEntries[state.queuedLogEntries.Count - 1];
                state.queuedLogEntries[state.queuedLogEntries.Count - 1] = new QueuedDebugLogEntry(
                    log.logString,
                    string.Empty,
                    log.logType
                );
            }
        }

        public void Toggle()
        {
            using (_PRF_Toggle.Auto())
            {
                if (state.isCommandPaletteVisible)
                {
                    HideLogWindow();
                }
                else
                {
                    ShowLogWindow();
                }
            }
        }

        // Collapse button is clicked
        internal void CollapseButtonPressed()
        {
            using (_PRF_CollapseButtonPressed.Auto())
            {
                // Swap the value of collapse mode
                state.isCollapseOn = !state.isCollapseOn;

                state.snapToBottom = true;
                references.collapseButton.color = state.isCollapseOn
                    ? settings.visuals.collapseButtonSelectedColor
                    : settings.visuals.collapseButtonNormalColor;
                references.recycledListView.SetCollapseMode(state.isCollapseOn);

                // Determine the new list of debug entries to show
                FilterLogs();
            }
        }

        // Filtering mode of error logs has changed
        internal void FilterErrorButtonPressed()
        {
            using (_PRF_FilterErrorButtonPressed.Auto())
            {
                state.logFilter = state.logFilter ^ DebugLogFilter.Error;

                if ((state.logFilter & DebugLogFilter.Error) == DebugLogFilter.Error)
                {
                    references.filterErrorButton.color = settings.visuals.filterButtonsSelectedColor;
                }
                else
                {
                    references.filterErrorButton.color = settings.visuals.filterButtonsNormalColor;
                }

                FilterLogs();
            }
        }

        // Filtering mode of info logs has changed
        internal void FilterLogButtonPressed()
        {
            using (_PRF_FilterLogButtonPressed.Auto())
            {
                state.logFilter = state.logFilter ^ DebugLogFilter.Info;

                if ((state.logFilter & DebugLogFilter.Info) == DebugLogFilter.Info)
                {
                    references.filterInfoButton.color = settings.visuals.filterButtonsSelectedColor;
                }
                else
                {
                    references.filterInfoButton.color = settings.visuals.filterButtonsNormalColor;
                }

                FilterLogs();
            }
        }

        // Filtering mode of warning logs has changed
        internal void FilterWarningButtonPressed()
        {
            using (_PRF_FilterWarningButtonPressed.Auto())
            {
                state.logFilter = state.logFilter ^ DebugLogFilter.Warning;

                if ((state.logFilter & DebugLogFilter.Warning) == DebugLogFilter.Warning)
                {
                    references.filterWarningButton.color = settings.visuals.filterButtonsSelectedColor;
                }
                else
                {
                    references.filterWarningButton.color = settings.visuals.filterButtonsNormalColor;
                }

                FilterLogs();
            }
        }

        // Command input field has lost focus
        internal void OnEndEditCommand(string command)
        {
            using (_PRF_OnEndEditCommand.Auto())
            {
                if (references.commandSuggestionsContainer.gameObject.activeSelf)
                {
                    references.commandSuggestionsContainer.gameObject.SetActive(false);
                }
            }
        }

        // Command field input is changed, check if command is submitted
        internal char OnValidateCommand(string text, int charIndex, char addedChar)
        {
            using (_PRF_OnValidateCommand.Auto())
            {
                if (addedChar == '\t') // Autocomplete attempt
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        var autoCompletedCommand = DeveloperConsole.GetAutoCompleteCommand(text);
                        if (!string.IsNullOrEmpty(autoCompletedCommand))
                        {
                            references.commandInputField.text = autoCompletedCommand;
                        }
                    }

                    return '\0';
                }

                if (addedChar == '\n') // Command is submitted
                {
                    // Clear the command field
                    if (settings.command.clearCommandAfterExecution)
                    {
                        references.commandInputField.text = string.Empty;
                    }

                    if (text.Length > 0)
                    {
                        if ((state.commandHistory.Count == 0) ||
                            (state.commandHistory[state.commandHistory.Count - 1] != text))
                        {
                            state.commandHistory.Add(text);
                        }

                        state.commandHistoryIndex = -1;
                        state.unfinishedCommand = null;

                        // Execute the command
                        DeveloperConsole.ExecuteCommand(text);

                        // Snap to bottom and select the latest entry
                        SetSnapToBottom(true);
                    }

                    return '\0';
                }

                return addedChar;
            }
        }

        // Pool an unused log item
        internal void PoolLogItem(DebugLogItem logItem)
        {
            using (_PRF_PoolLogItem.Auto())
            {
                logItem.CanvasGroup.alpha = 0f;
                logItem.CanvasGroup.blocksRaycasts = false;

                state.pooledLogItems.Add(logItem);
            }
        }

        // Fetch a log item from the pool
        internal DebugLogItem PopLogItem()
        {
            using (_PRF_PopLogItem.Auto())
            {
                DebugLogItem newLogItem;

                // If pool is not empty, fetch a log item from the pool,
                // create a new log item otherwise
                if (state.pooledLogItems.Count > 0)
                {
                    newLogItem = state.pooledLogItems[state.pooledLogItems.Count - 1];
                    state.pooledLogItems.RemoveAt(state.pooledLogItems.Count - 1);

                    newLogItem.CanvasGroup.alpha = 1f;
                    newLogItem.CanvasGroup.blocksRaycasts = true;
                }
                else
                {
                    newLogItem = Instantiate(
                        settings.visuals.logItemPrefab,
                        references.logItemsContainer,
                        false
                    );
                    newLogItem.Initialize(references.recycledListView);
                }

                return newLogItem;
            }
        }

        // Show suggestions for the currently entered command
        internal void RefreshCommandSuggestions(string command)
        {
            using (_PRF_RefreshCommandSuggestions.Auto())
            {
                if (!settings.command.showCommandSuggestions)
                {
                    return;
                }

                state.commandInputFieldPrevCaretPos = references.commandInputField.caretPosition;

                // Don't recalculate the command suggestions if the input command hasn't changed (i.e. only caret's position has changed)
                var commandChanged = command != state.commandInputFieldPrevCommand;
                var commandNameOrParametersChanged = false;
                if (commandChanged)
                {
                    state.commandInputFieldPrevCommand = command;

                    state.matchingCommandSuggestions.Clear();
                    state.commandCaretIndexIncrements.Clear();

                    var prevCommandName = state.commandInputFieldPrevCommandName;
                    int numberOfParameters;
                    DeveloperConsole.GetCommandSuggestions(
                        command,
                        state.matchingCommandSuggestions,
                        state.commandCaretIndexIncrements,
                        ref state.commandInputFieldPrevCommandName,
                        out numberOfParameters
                    );
                    if ((prevCommandName != state.commandInputFieldPrevCommandName) ||
                        (numberOfParameters != state.commandInputFieldPrevParamCount))
                    {
                        state.commandInputFieldPrevParamCount = numberOfParameters;
                        commandNameOrParametersChanged = true;
                    }
                }

                var caretArgumentIndex = 0;
                var caretPos = references.commandInputField.caretPosition;
                for (var i = 0;
                     (i < state.commandCaretIndexIncrements.Count) &&
                     (caretPos > state.commandCaretIndexIncrements[i]);
                     i++)
                {
                    caretArgumentIndex++;
                }

                if (caretArgumentIndex != state.commandInputFieldPrevCaretArgumentIndex)
                {
                    state.commandInputFieldPrevCaretArgumentIndex = caretArgumentIndex;
                }
                else if (!commandChanged || !commandNameOrParametersChanged)
                {
                    // Command suggestions don't need to be updated if:
                    // a) neither the entered command nor the argument that the caret is hovering has changed
                    // b) entered command has changed but command's name hasn't changed, parameter count hasn't changed and the argument
                    //    that the caret is hovering hasn't changed (i.e. user has continued typing a parameter's value)
                    return;
                }

                if (state.matchingCommandSuggestions.Count == 0)
                {
                    OnEndEditCommand(command);
                }
                else
                {
                    if (!references.commandSuggestionsContainer.gameObject.activeSelf)
                    {
                        references.commandSuggestionsContainer.gameObject.SetActive(true);
                    }

                    var suggestionInstancesCount = state.commandSuggestionInstances.Count;
                    var suggestionsCount = state.matchingCommandSuggestions.Count;

                    for (var i = 0; i < suggestionsCount; i++)
                    {
                        if (i >= state.visibleCommandSuggestionInstances)
                        {
                            if (i >= suggestionInstancesCount)
                            {
                                state.commandSuggestionInstances.Add(
                                    Instantiate(
                                        settings.visuals.commandSuggestionPrefab,
                                        references.commandSuggestionsContainer,
                                        false
                                    )
                                );
                            }
                            else
                            {
                                state.commandSuggestionInstances[i].gameObject.SetActive(true);
                            }

                            state.visibleCommandSuggestionInstances++;
                        }

                        var suggestedCommand = state.matchingCommandSuggestions[i];
                        state.commandSuggestionsStringBuilder.Clear();
                        if (caretArgumentIndex > 0)
                        {
                            state.commandSuggestionsStringBuilder.Append(suggestedCommand.command);
                        }
                        else
                        {
                            state.commandSuggestionsStringBuilder.Append(
                                settings.visuals.commandSuggestionHighlightStart
                            );
                            state.commandSuggestionsStringBuilder.Append(
                                state.matchingCommandSuggestions[i].command
                            );
                            state.commandSuggestionsStringBuilder.Append(
                                settings.visuals.commandSuggestionHighlightEnd
                            );
                        }

                        if (suggestedCommand.parameters.Length > 0)
                        {
                            state.commandSuggestionsStringBuilder.Append(" ");

                            // If the command name wasn't highlighted, a parameter must always be highlighted
                            var caretParameterIndex = caretArgumentIndex - 1;
                            if (caretParameterIndex >= suggestedCommand.parameters.Length)
                            {
                                caretParameterIndex = suggestedCommand.parameters.Length - 1;
                            }

                            for (var j = 0; j < suggestedCommand.parameters.Length; j++)
                            {
                                if (caretParameterIndex != j)
                                {
                                    state.commandSuggestionsStringBuilder.Append(
                                        suggestedCommand.parameters[j]
                                    );
                                }
                                else
                                {
                                    state.commandSuggestionsStringBuilder.Append(
                                        settings.visuals.commandSuggestionHighlightStart
                                    );

                                    state.commandSuggestionsStringBuilder.Append(
                                        suggestedCommand.parameters[j]
                                    );

                                    state.commandSuggestionsStringBuilder.Append(
                                        settings.visuals.commandSuggestionHighlightEnd
                                    );
                                }
                            }
                        }

                        state.commandSuggestionInstances[i].text =
                            state.commandSuggestionsStringBuilder.ToString();
                    }

                    for (var i = state.visibleCommandSuggestionInstances - 1; i >= suggestionsCount; i--)
                    {
                        state.commandSuggestionInstances[i].gameObject.SetActive(false);
                    }

                    state.visibleCommandSuggestionInstances = suggestionsCount;
                }
            }
        }

        // Debug window is being resized,
        // Set the sizeDelta property of the window accordingly while
        // preventing window dimensions from going below the minimum dimensions
        internal void Resize(PointerEventData eventData)
        {
            using (_PRF_Resize.Auto())
            {
                Vector2 localPoint;
                if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        references.canvasTR,
                        eventData.position,
                        eventData.pressEventCamera,
                        out localPoint
                    ))
                {
                    return;
                }

                // To be able to maximize the log window easily:
                // - When enableHorizontalResizing is true and resizing horizontally, resize button will be grabbed from its left edge (if resizeFromRight is true) or its right edge
                // - While resizing vertically, resize button will be grabbed from its top edge
                const float resizeButtonWidth = 64f;
                const float resizeButtonHeight = 36f;

                var canvasPivot = references.canvasTR.pivot;
                var canvasSize = references.canvasTR.rect.size;
                var anchorMin = references.logWindowTR.anchorMin;

                // Horizontal resizing
                if (settings.window.enableHorizontalResizing)
                {
                    if (settings.window.resizeFromRight)
                    {
                        localPoint.x += (canvasPivot.x * canvasSize.x) + resizeButtonWidth;
                        if (localPoint.x < settings.window.minimumWidth)
                        {
                            localPoint.x = settings.window.minimumWidth;
                        }

                        var anchorMax = references.logWindowTR.anchorMax;
                        anchorMax.x = Mathf.Clamp01(localPoint.x / canvasSize.x);
                        references.logWindowTR.anchorMax = anchorMax;
                    }
                    else
                    {
                        localPoint.x += (canvasPivot.x * canvasSize.x) - resizeButtonWidth;
                        if (localPoint.x > (canvasSize.x - settings.window.minimumWidth))
                        {
                            localPoint.x = canvasSize.x - settings.window.minimumWidth;
                        }

                        anchorMin.x = Mathf.Clamp01(localPoint.x / canvasSize.x);
                    }
                }

                // Vertical resizing
                var notchHeight =
                    -references.logWindowTR.sizeDelta
                               .y; // Size of notch screen cutouts at the top of the screen

                localPoint.y += (canvasPivot.y * canvasSize.y) - resizeButtonHeight;
                if (localPoint.y > (canvasSize.y - settings.window.minimumHeight - notchHeight))
                {
                    localPoint.y = canvasSize.y - settings.window.minimumHeight - notchHeight;
                }

                anchorMin.y = Mathf.Clamp01(localPoint.y / canvasSize.y);

                references.logWindowTR.anchorMin = anchorMin;

                // Update the recycled list view
                references.recycledListView.OnViewportHeightChanged();
            }
        }

        // Search term has changed
        internal void SearchTermChanged(string searchTerm)
        {
            using (_PRF_SearchTermChanged.Auto())
            {
                if (searchTerm != null)
                {
                    searchTerm = searchTerm.Trim();
                }

                state.searchTerm = searchTerm;
                var inSearchMode = !string.IsNullOrEmpty(searchTerm);
                if (inSearchMode || state.isInSearchMode)
                {
                    state.isInSearchMode = inSearchMode;
                    FilterLogs();
                }
            }
        }

        // Make sure the scroll bar of the scroll rect is adjusted properly
        internal void ValidateScrollPosition()
        {
            using (_PRF_ValidateScrollPosition.Auto())
            {
                references.logItemsScrollRect.OnScroll(state.nullPointerEventData);
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            if ((settings.popup.enablePopup && settings.popup.startInPopupMode) ||
                (!settings.popup.enablePopup && settings.general.startMinimized))
            {
                HideLogWindow();
            }
            else
            {
                ShowLogWindow();
            }

            PopupEnabled = settings.popup.enablePopup;
        }

        protected override async AppaTask WhenDisabled()
        {
            using (_PRF_WhenDisabled.Auto())
            {
                await base.WhenDisabled();

                // Stop receiving debug entries
                UnityEngine.Application.logMessageReceivedThreaded -= ReceivedLog;

#if !UNITY_EDITOR && UNITY_ANDROID
			if( logcatListener != null )
				logcatListener.Stop();
#endif

                DeveloperConsole.RemoveCommand("logs.save");
            }
        }

        protected override async AppaTask WhenEnabled()
        {
            using (_PRF_WhenEnabled.Auto())
            {
                await base.WhenEnabled();

                InitializeState();

                // Intercept debug entries
                UnityEngine.Application.logMessageReceivedThreaded -= ReceivedLog;
                UnityEngine.Application.logMessageReceivedThreaded += ReceivedLog;

                if (settings.mobile.receiveLogcatLogsInAndroid)
                {
#if !UNITY_EDITOR && UNITY_ANDROID
				if( logcatListener == null )
					logcatListener = new DebugLogLogcatListener();

				logcatListener.Start( logcatArguments );
#endif
                }

                DeveloperConsole.AddCommand("logs.save", "Saves logs to persistentDataPath", SaveLogsToFile);
                DeveloperConsole.AddCommand<string>(
                    "logs.save",
                    "Saves logs to the specified file",
                    SaveLogsToFile
                );
            }
        }

        private IEnumerator ActivateCommandInputFieldCoroutine()
        {
#if UNITY_EDITOR || UNITY_STANDALONE

            // Waiting 1 frame before activating commandInputField ensures that the toggleKey isn't captured by it
            yield return null;
            references.commandInputField.ActivateInputField();

            yield return null;
            references.commandInputField.MoveTextEnd(false);
#endif
        }

        // If a cutout is intersecting with debug window on notch screens, shift the window downwards
        private void CheckScreenCutout()
        {
            using (_PRF_CheckScreenCutout.Auto())
            {
                if (!settings.mobile.avoidScreenCutout)
                {
                    return;
                }

#if UNITY_2017_2_OR_NEWER && ( UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS )

                // Check if there is a cutout at the top of the screen
                var screenHeight = Screen.height;
                var safeYMax = Screen.safeArea.yMax;
                if (safeYMax < (screenHeight - 1)) // 1: a small threshold
                {
                    // There is a cutout, shift the log window downwards
                    var cutoutPercentage = (screenHeight - safeYMax) / Screen.height;
                    var cutoutLocalSize = cutoutPercentage * references.canvasTR.rect.height;

                    references.logWindowTR.anchoredPosition = new Vector2(0f, -cutoutLocalSize);
                    references.logWindowTR.sizeDelta = new Vector2(0f,        -cutoutLocalSize);
                }
                else
                {
                    references.logWindowTR.anchoredPosition = Vector2.zero;
                    references.logWindowTR.sizeDelta = Vector2.zero;
                }
#endif
            }
        }

        // Determine the filtered list of debug entries to show on screen
        private void FilterLogs()
        {
            using (_PRF_FilterLogs.Auto())
            {
                state.indicesOfListEntriesToShow.Clear();

                if (state.logFilter != DebugLogFilter.None)
                {
                    if (state.logFilter == DebugLogFilter.All)
                    {
                        if (state.isCollapseOn)
                        {
                            if (!state.isInSearchMode)
                            {
                                // All the unique debug entries will be listed just once.
                                // So, list of debug entries to show is the same as the
                                // order these unique debug entries are added to collapsedLogEntries
                                for (int i = 0, count = state.collapsedLogEntries.Count; i < count; i++)
                                {
                                    state.indicesOfListEntriesToShow.Add(i);
                                }
                            }
                            else
                            {
                                for (int i = 0, count = state.collapsedLogEntries.Count; i < count; i++)
                                {
                                    if (state.collapsedLogEntries[i].MatchesSearchTerm(state.searchTerm))
                                    {
                                        state.indicesOfListEntriesToShow.Add(i);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!state.isInSearchMode)
                            {
                                for (int i = 0, count = state.uncollapsedLogEntriesIndices.Count;
                                     i < count;
                                     i++)
                                {
                                    state.indicesOfListEntriesToShow.Add(
                                        state.uncollapsedLogEntriesIndices[i]
                                    );
                                }
                            }
                            else
                            {
                                for (int i = 0, count = state.uncollapsedLogEntriesIndices.Count;
                                     i < count;
                                     i++)
                                {
                                    if (state.collapsedLogEntries[state.uncollapsedLogEntriesIndices[i]]
                                             .MatchesSearchTerm(state.searchTerm))
                                    {
                                        state.indicesOfListEntriesToShow.Add(
                                            state.uncollapsedLogEntriesIndices[i]
                                        );
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // Show only the debug entries that match the current filter
                        var isInfoEnabled = (state.logFilter & DebugLogFilter.Info) == DebugLogFilter.Info;
                        var isWarningEnabled =
                            (state.logFilter & DebugLogFilter.Warning) == DebugLogFilter.Warning;
                        var isErrorEnabled = (state.logFilter & DebugLogFilter.Error) == DebugLogFilter.Error;

                        if (state.isCollapseOn)
                        {
                            for (int i = 0, count = state.collapsedLogEntries.Count; i < count; i++)
                            {
                                var logEntry = state.collapsedLogEntries[i];

                                if (state.isInSearchMode && !logEntry.MatchesSearchTerm(state.searchTerm))
                                {
                                    continue;
                                }

                                if (logEntry.logTypeSpriteRepresentation == settings.visuals.infoLog)
                                {
                                    if (isInfoEnabled)
                                    {
                                        state.indicesOfListEntriesToShow.Add(i);
                                    }
                                }
                                else if (logEntry.logTypeSpriteRepresentation == settings.visuals.warningLog)
                                {
                                    if (isWarningEnabled)
                                    {
                                        state.indicesOfListEntriesToShow.Add(i);
                                    }
                                }
                                else if (isErrorEnabled)
                                {
                                    state.indicesOfListEntriesToShow.Add(i);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0, count = state.uncollapsedLogEntriesIndices.Count; i < count; i++)
                            {
                                var logEntry =
                                    state.collapsedLogEntries[state.uncollapsedLogEntriesIndices[i]];

                                if (state.isInSearchMode && !logEntry.MatchesSearchTerm(state.searchTerm))
                                {
                                    continue;
                                }

                                if (logEntry.logTypeSpriteRepresentation == settings.visuals.infoLog)
                                {
                                    if (isInfoEnabled)
                                    {
                                        state.indicesOfListEntriesToShow.Add(
                                            state.uncollapsedLogEntriesIndices[i]
                                        );
                                    }
                                }
                                else if (logEntry.logTypeSpriteRepresentation == settings.visuals.warningLog)
                                {
                                    if (isWarningEnabled)
                                    {
                                        state.indicesOfListEntriesToShow.Add(
                                            state.uncollapsedLogEntriesIndices[i]
                                        );
                                    }
                                }
                                else if (isErrorEnabled)
                                {
                                    state.indicesOfListEntriesToShow.Add(
                                        state.uncollapsedLogEntriesIndices[i]
                                    );
                                }
                            }
                        }
                    }
                }

                // Update the recycled list view
                references.recycledListView.DeselectSelectedLogItem();
                references.recycledListView.OnLogEntriesUpdated(true);

                ValidateScrollPosition();
            }
        }

        private void InitializeState()
        {
            using (_PRF_InitializeState.Auto())
            {
                if (settings == null)
                {
                    settings = _developerConsoleSettings;
                }

                if (state == null)
                {
                    state = new DeveloperConsoleState();
                }

                if (!state.initialized)
                {
                    state.InitializeState(settings);
                }

                if (references == null)
                {
                    references = new DeveloperConsoleReferences();
                }

                if (!references.initialized)
                {
                    references.InitalizeReferences(transform, this, settings, state);
                }
            }
        }

        // Present the log entry in the console
        private void ProcessLog(QueuedDebugLogEntry queuedLogEntry)
        {
            using (_PRF_ProcessLog.Auto())
            {
                var logType = queuedLogEntry.logType;
                DebugLogEntry logEntry;
                if (state.pooledLogEntries.Count > 0)
                {
                    logEntry = state.pooledLogEntries[state.pooledLogEntries.Count - 1];
                    state.pooledLogEntries.RemoveAt(state.pooledLogEntries.Count - 1);
                }
                else
                {
                    logEntry = new DebugLogEntry();
                }

                logEntry.Initialize(queuedLogEntry.logString, queuedLogEntry.stackTrace);

                // Check if this entry is a duplicate (i.e. has been received before)
                int logEntryIndex;
                var isEntryInCollapsedEntryList =
                    state.collapsedLogEntriesMap.TryGetValue(logEntry, out logEntryIndex);
                if (!isEntryInCollapsedEntryList)
                {
                    // It is not a duplicate,
                    // add it to the list of unique debug entries
                    logEntry.logTypeSpriteRepresentation = state.logSpriteRepresentations[logType];

                    logEntryIndex = state.collapsedLogEntries.Count;
                    state.collapsedLogEntries.Add(logEntry);
                    state.collapsedLogEntriesMap[logEntry] = logEntryIndex;
                }
                else
                {
                    // It is a duplicate, pool the duplicate log entry and
                    // increment the original debug item's collapsed count
                    state.pooledLogEntries.Add(logEntry);

                    logEntry = state.collapsedLogEntries[logEntryIndex];
                    logEntry.count++;
                }

                // Add the index of the unique debug entry to the list
                // that stores the order the debug entries are received
                state.uncollapsedLogEntriesIndices.Add(logEntryIndex);

                // If this debug entry matches the current filters,
                // add it to the list of debug entries to show
                var logEntryIndexInEntriesToShow = -1;
                var logTypeSpriteRepresentation = logEntry.logTypeSpriteRepresentation;
                if (state.isCollapseOn && isEntryInCollapsedEntryList)
                {
                    if (state.isCommandPaletteVisible)
                    {
                        if (!state.isInSearchMode && (state.logFilter == DebugLogFilter.All))
                        {
                            logEntryIndexInEntriesToShow = logEntryIndex;
                        }
                        else
                        {
                            logEntryIndexInEntriesToShow =
                                state.indicesOfListEntriesToShow.IndexOf(logEntryIndex);
                        }

                        references.recycledListView.OnCollapsedLogEntryAtIndexUpdated(
                            logEntryIndexInEntriesToShow
                        );
                    }
                }
                else if ((!state.isInSearchMode || queuedLogEntry.MatchesSearchTerm(state.searchTerm)) &&
                         ((state.logFilter == DebugLogFilter.All) ||
                          ((logTypeSpriteRepresentation == settings.visuals.infoLog) &&
                           ((state.logFilter & DebugLogFilter.Info) == DebugLogFilter.Info)) ||
                          ((logTypeSpriteRepresentation == settings.visuals.warningLog) &&
                           ((state.logFilter & DebugLogFilter.Warning) == DebugLogFilter.Warning)) ||
                          ((logTypeSpriteRepresentation == settings.visuals.errorLog) &&
                           ((state.logFilter & DebugLogFilter.Error) == DebugLogFilter.Error))))
                {
                    state.indicesOfListEntriesToShow.Add(logEntryIndex);
                    logEntryIndexInEntriesToShow = state.indicesOfListEntriesToShow.Count - 1;

                    if (state.isCommandPaletteVisible)
                    {
                        state.shouldUpdateRecycledListView = true;
                    }
                }

                if (logType == LogType.Log)
                {
                    state.newInfoEntryCount++;
                }
                else if (logType == LogType.Warning)
                {
                    state.newWarningEntryCount++;
                }
                else
                {
                    state.newErrorEntryCount++;
                }

                // Automatically expand this log if necessary
                if ((state.pendingLogToAutoExpand > 0) &&
                    (--state.pendingLogToAutoExpand <= 0) &&
                    state.isCommandPaletteVisible &&
                    (logEntryIndexInEntriesToShow >= 0))
                {
                    state.indexOfLogEntryToSelectAndFocus = logEntryIndexInEntriesToShow;
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(DebugLogManager) + ".";

        

        

        

        private static readonly ProfilerMarker _PRF_InitializeState = new(_PRF_PFX + nameof(InitializeState));
        private static readonly ProfilerMarker _PRF_Toggle = new(_PRF_PFX + nameof(Toggle));

        private static readonly ProfilerMarker _PRF_OnRectTransformDimensionsChange =
            new(_PRF_PFX + nameof(OnRectTransformDimensionsChange));


        private static readonly ProfilerMarker _PRF_ShowLogWindow = new(_PRF_PFX + nameof(ShowLogWindow));
        private static readonly ProfilerMarker _PRF_HideLogWindow = new(_PRF_PFX + nameof(HideLogWindow));

        private static readonly ProfilerMarker _PRF_OnValidateCommand =
            new(_PRF_PFX + nameof(OnValidateCommand));

        private static readonly ProfilerMarker _PRF_ReceivedLog = new(_PRF_PFX + nameof(ReceivedLog));
        private static readonly ProfilerMarker _PRF_ProcessLog = new(_PRF_PFX + nameof(ProcessLog));
        private static readonly ProfilerMarker _PRF_SetSnapToBottom = new(_PRF_PFX + nameof(SetSnapToBottom));

        private static readonly ProfilerMarker _PRF_ValidateScrollPosition =
            new(_PRF_PFX + nameof(ValidateScrollPosition));

        private static readonly ProfilerMarker _PRF_ExpandLatestPendingLog =
            new(_PRF_PFX + nameof(ExpandLatestPendingLog));

        private static readonly ProfilerMarker _PRF_StripStackTraceFromLatestPendingLog =
            new(_PRF_PFX + nameof(StripStackTraceFromLatestPendingLog));

        private static readonly ProfilerMarker _PRF_ClearLogs = new(_PRF_PFX + nameof(ClearLogs));

        private static readonly ProfilerMarker _PRF_CollapseButtonPressed =
            new(_PRF_PFX + nameof(CollapseButtonPressed));

        private static readonly ProfilerMarker _PRF_FilterLogButtonPressed =
            new(_PRF_PFX + nameof(FilterLogButtonPressed));

        private static readonly ProfilerMarker _PRF_FilterWarningButtonPressed =
            new(_PRF_PFX + nameof(FilterWarningButtonPressed));

        private static readonly ProfilerMarker _PRF_FilterErrorButtonPressed =
            new(_PRF_PFX + nameof(FilterErrorButtonPressed));

        private static readonly ProfilerMarker _PRF_SearchTermChanged =
            new(_PRF_PFX + nameof(SearchTermChanged));

        private static readonly ProfilerMarker _PRF_RefreshCommandSuggestions =
            new(_PRF_PFX + nameof(RefreshCommandSuggestions));

        private static readonly ProfilerMarker _PRF_OnEndEditCommand =
            new(_PRF_PFX + nameof(OnEndEditCommand));

        private static readonly ProfilerMarker _PRF_Resize = new(_PRF_PFX + nameof(Resize));
        private static readonly ProfilerMarker _PRF_FilterLogs = new(_PRF_PFX + nameof(FilterLogs));
        private static readonly ProfilerMarker _PRF_GetAllLogs = new(_PRF_PFX + nameof(GetAllLogs));
        private static readonly ProfilerMarker _PRF_SaveLogsToFile = new(_PRF_PFX + nameof(SaveLogsToFile));

        private static readonly ProfilerMarker _PRF_CheckScreenCutout =
            new(_PRF_PFX + nameof(CheckScreenCutout));

        private static readonly ProfilerMarker _PRF_PoolLogItem = new(_PRF_PFX + nameof(PoolLogItem));
        private static readonly ProfilerMarker _PRF_PopLogItem = new(_PRF_PFX + nameof(PopLogItem));

        #endregion
    }
}
*/


