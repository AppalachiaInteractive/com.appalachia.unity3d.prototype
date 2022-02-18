using System;
using System.Text;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Components;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Model;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Settings;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR && UNITY_2021_1_OR_NEWER
using Screen = UnityEngine.Device.Screen; // To support Device Simulator on Unity 2021.1+
#endif

// Receives debug entries and custom events (e.g. Clear, Collapse, Filter by Type)
// and notifies the recycled list view of changes to the list of debug entries
// 
// - Vocabulary -
// Debug/Log entry: a Debug.Log/LogError/LogWarning/LogException/LogAssertion request made by
//                   the client and intercepted by this manager object
// Debug/Log item: a visual (uGUI) representation of a debug entry
// 
// There can be a lot of debug entries in the system but there will only be a handful of log items 
// to show their properties on screen (these log items are recycled as the list is scrolled)

// An enum to represent filtered log types
namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog
{
    [CallStaticConstructorInEditor]
    [ExecutionOrder(ExecutionOrders.DebugLogManager)]
    public partial class DebugLogManager : SingletonAppalachiaBehaviour<DebugLogManager>
    {
        static DebugLogManager()
        {
            RegisterDependency<DebugLogSettings>(i => _debugLogSettings = i);
        }

        public DebugLogManager(Action onLogWindowShown)
        {
            OnLogWindowShown = onLogWindowShown;
        }

        #region Static Fields and Autoproperties

        [ShowInInspector, InlineEditor, HideLabel, FoldoutGroup("Settings")]
        private static DebugLogSettings _debugLogSettings;

        #endregion

        #region Fields and Autoproperties

        [InlineProperty, HideLabel, FoldoutGroup("References")]
        public DebugLogReferences references;

        [InlineProperty, HideLabel, FoldoutGroup("State")]
        public DebugLogState state;

        // Callbacks for log window show/hide events
        public Action OnLogWindowShown, OnLogWindowHidden;

        #endregion

        public bool IsLogWindowVisible => state.isLogWindowVisible;

        public bool PopupEnabled
        {
            get => references.popupManager.gameObject.activeSelf;
            set => references.popupManager.gameObject.SetActive(value);
        }

        #region Event Functions

        private void LateUpdate()
        {
            using (_PRF_LateUpdate.Auto())
            {
                if (ShouldSkipUpdate || !references.popupManager.FullyInitialized)
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
                    if (!state.isLogWindowVisible)
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
                if (state.isLogWindowVisible && state.shouldUpdateRecycledListView)
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

                if (state.screenDimensionsChanged)
                {
                    // Update the recycled list view
                    if (state.isLogWindowVisible)
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
                        if (logWindowWidth >= _debugLogSettings.general.topSearchbarMinWidth)
                        {
                            if (references.searchbar.parent == references.searchbarSlotBottom)
                            {
                                references.searchbarSlotTop.gameObject.SetActive(true);
                                if (AppalachiaApplication.IsPlaying)
                                {
                                    references.searchbar.SetParent(references.searchbarSlotTop, false);
                                }

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
                                if (AppalachiaApplication.IsPlaying)
                                {
                                    references.searchbar.SetParent(references.searchbarSlotBottom, false);
                                }

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

                    if (state.isLogWindowVisible)
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
                if (!FullyInitialized)
                {
                    return;
                }

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

                if (references.popupManager.FullyInitialized)
                {
                    references.popupManager.Show();
                }
                else
                {
                    references.popupManager.InitializationComplete += p => references.popupManager.Show();
                }

                state.isLogWindowVisible = false;

                if (OnLogWindowHidden != null)
                {
                    OnLogWindowHidden();
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
                    if (logLength > _debugLogSettings.general.maxLogLength)
                    {
                        logString = logString.Substring(0, _debugLogSettings.general.maxLogLength - 11) +
                                    "<truncated>";
                    }
                }
                else
                {
                    logLength += stackTrace.Length;
                    if (logLength > _debugLogSettings.general.maxLogLength)
                    {
                        // Decide which log component(s) to truncate
                        var halfMaxLogLength = _debugLogSettings.general.maxLogLength / 2;
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
                                                _debugLogSettings.general.maxLogLength -
                                                stackTrace.Length -
                                                11
                                            ) +
                                            "<truncated>";
                            }
                        }
                        else
                        {
                            // Truncate stackTrace
                            stackTrace = stackTrace.Substring(
                                             0,
                                             _debugLogSettings.general.maxLogLength - logString.Length - 12
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
                references.logWindowCanvasGroup.alpha = _debugLogSettings.window.alpha;

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

                state.isLogWindowVisible = true;

                if (OnLogWindowShown != null)
                {
                    OnLogWindowShown();
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
                if (state.isLogWindowVisible)
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
                    ? _debugLogSettings.visuals.collapseButtonSelectedColor
                    : _debugLogSettings.visuals.collapseButtonNormalColor;
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
                    references.filterErrorButton.color = _debugLogSettings.visuals.filterButtonsSelectedColor;
                }
                else
                {
                    references.filterErrorButton.color = _debugLogSettings.visuals.filterButtonsNormalColor;
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
                    references.filterInfoButton.color = _debugLogSettings.visuals.filterButtonsSelectedColor;
                }
                else
                {
                    references.filterInfoButton.color = _debugLogSettings.visuals.filterButtonsNormalColor;
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
                    references.filterWarningButton.color =
                        _debugLogSettings.visuals.filterButtonsSelectedColor;
                }
                else
                {
                    references.filterWarningButton.color = _debugLogSettings.visuals.filterButtonsNormalColor;
                }

                FilterLogs();
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
                    var newLogItemInstance = Instantiate(
                        _debugLogSettings.visuals.logItemPrefab,
                        references.logItemsContainer,
                        false
                    );
                    newLogItem = newLogItemInstance.GetComponent<DebugLogItem>();

                    newLogItem.Initialize(references.recycledListView);
                }

                return newLogItem;
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
                if (_debugLogSettings.window.enableHorizontalResizing)
                {
                    if (_debugLogSettings.window.resizeFromRight)
                    {
                        localPoint.x += (canvasPivot.x * canvasSize.x) + resizeButtonWidth;
                        if (localPoint.x < _debugLogSettings.window.minimumWidth)
                        {
                            localPoint.x = _debugLogSettings.window.minimumWidth;
                        }

                        var anchorMax = references.logWindowTR.anchorMax;
                        anchorMax.x = Mathf.Clamp01(localPoint.x / canvasSize.x);
                        references.logWindowTR.anchorMax = anchorMax;
                    }
                    else
                    {
                        localPoint.x += (canvasPivot.x * canvasSize.x) - resizeButtonWidth;
                        if (localPoint.x > (canvasSize.x - _debugLogSettings.window.minimumWidth))
                        {
                            localPoint.x = canvasSize.x - _debugLogSettings.window.minimumWidth;
                        }

                        anchorMin.x = Mathf.Clamp01(localPoint.x / canvasSize.x);
                    }
                }

                // Vertical resizing
                var notchHeight =
                    -references.logWindowTR.sizeDelta
                               .y; // Size of notch screen cutouts at the top of the screen

                localPoint.y += (canvasPivot.y * canvasSize.y) - resizeButtonHeight;
                if (localPoint.y > (canvasSize.y - _debugLogSettings.window.minimumHeight - notchHeight))
                {
                    localPoint.y = canvasSize.y - _debugLogSettings.window.minimumHeight - notchHeight;
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

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                if ((_debugLogSettings.popup.enablePopup && _debugLogSettings.popup.startInPopupMode) ||
                    (!_debugLogSettings.popup.enablePopup && _debugLogSettings.general.startMinimized))
                {
                    HideLogWindow();
                }
                else
                {
                    ShowLogWindow();
                }

                PopupEnabled = _debugLogSettings.popup.enablePopup;

                InitializeState();
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenDisabled()
        {
            await base.WhenDisabled();

            using (_PRF_WhenDisabled.Auto())
            {
                // Stop receiving debug entries
                UnityEngine.Application.logMessageReceivedThreaded -= ReceivedLog;

#if !UNITY_EDITOR && UNITY_ANDROID
			if( logcatListener != null )
				logcatListener.Stop();
#endif
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                // Intercept debug entries
                UnityEngine.Application.logMessageReceivedThreaded -= ReceivedLog;
                UnityEngine.Application.logMessageReceivedThreaded += ReceivedLog;

                if (_debugLogSettings.mobile.receiveLogcatLogsInAndroid)
                {
#if !UNITY_EDITOR && UNITY_ANDROID
				if( logcatListener == null )
					logcatListener = new DebugLogLogcatListener();

				logcatListener.Start( logcatArguments );
#endif
                }
            }
        }

        // If a cutout is intersecting with debug window on notch screens, shift the window downwards
        private void CheckScreenCutout()
        {
            using (_PRF_CheckScreenCutout.Auto())
            {
                if (!_debugLogSettings.mobile.avoidScreenCutout)
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

                                if (logEntry.logTypeSpriteRepresentation == _debugLogSettings.visuals.infoLog)
                                {
                                    if (isInfoEnabled)
                                    {
                                        state.indicesOfListEntriesToShow.Add(i);
                                    }
                                }
                                else if (logEntry.logTypeSpriteRepresentation ==
                                         _debugLogSettings.visuals.warningLog)
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

                                if (logEntry.logTypeSpriteRepresentation == _debugLogSettings.visuals.infoLog)
                                {
                                    if (isInfoEnabled)
                                    {
                                        state.indicesOfListEntriesToShow.Add(
                                            state.uncollapsedLogEntriesIndices[i]
                                        );
                                    }
                                }
                                else if (logEntry.logTypeSpriteRepresentation ==
                                         _debugLogSettings.visuals.warningLog)
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
                if (state == null)
                {
                    state = new DebugLogState();
                }

                if (!state.initialized)
                {
                    state.InitializeState(_debugLogSettings);
                }

                if (references == null)
                {
                    references = new DebugLogReferences();
                }

                if (!references.initialized)
                {
                    references.InitalizeReferences(transform, this, _debugLogSettings, state);
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
                    if (state.isLogWindowVisible)
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
                          ((logTypeSpriteRepresentation == _debugLogSettings.visuals.infoLog) &&
                           ((state.logFilter & DebugLogFilter.Info) == DebugLogFilter.Info)) ||
                          ((logTypeSpriteRepresentation == _debugLogSettings.visuals.warningLog) &&
                           ((state.logFilter & DebugLogFilter.Warning) == DebugLogFilter.Warning)) ||
                          ((logTypeSpriteRepresentation == _debugLogSettings.visuals.errorLog) &&
                           ((state.logFilter & DebugLogFilter.Error) == DebugLogFilter.Error))))
                {
                    state.indicesOfListEntriesToShow.Add(logEntryIndex);
                    logEntryIndexInEntriesToShow = state.indicesOfListEntriesToShow.Count - 1;

                    if (state.isLogWindowVisible)
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
                    state.isLogWindowVisible &&
                    (logEntryIndexInEntriesToShow >= 0))
                {
                    state.indexOfLogEntryToSelectAndFocus = logEntryIndexInEntriesToShow;
                }
            }
        }

        private void SaveLogsToFile()
        {
            using (_PRF_SaveLogsToFile.Auto())
            {
                SaveLogsToFile(
                    AppaPath.Combine(
                        AppalachiaApplication.PersistentDataPath,
                        DateTime.Now.ToString("O") + ".txt"
                    )
                );
            }
        }

        private void SaveLogsToFile(string filePath)
        {
            using (_PRF_SaveLogsToFile.Auto())
            {
                AppaFile.WriteAllText(filePath, GetAllLogs());
                Context.Log.Info("Logs saved to: " + filePath);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_CheckScreenCutout =
            new(_PRF_PFX + nameof(CheckScreenCutout));

        private static readonly ProfilerMarker _PRF_ClearLogs = new(_PRF_PFX + nameof(ClearLogs));

        private static readonly ProfilerMarker _PRF_CollapseButtonPressed =
            new(_PRF_PFX + nameof(CollapseButtonPressed));

        private static readonly ProfilerMarker _PRF_ExpandLatestPendingLog =
            new(_PRF_PFX + nameof(ExpandLatestPendingLog));

        private static readonly ProfilerMarker _PRF_FilterErrorButtonPressed =
            new(_PRF_PFX + nameof(FilterErrorButtonPressed));

        private static readonly ProfilerMarker _PRF_FilterLogButtonPressed =
            new(_PRF_PFX + nameof(FilterLogButtonPressed));

        private static readonly ProfilerMarker _PRF_FilterLogs = new(_PRF_PFX + nameof(FilterLogs));

        private static readonly ProfilerMarker _PRF_FilterWarningButtonPressed =
            new(_PRF_PFX + nameof(FilterWarningButtonPressed));

        private static readonly ProfilerMarker _PRF_GetAllLogs = new(_PRF_PFX + nameof(GetAllLogs));
        private static readonly ProfilerMarker _PRF_HideLogWindow = new(_PRF_PFX + nameof(HideLogWindow));

        private static readonly ProfilerMarker _PRF_InitializeState = new(_PRF_PFX + nameof(InitializeState));

        private static readonly ProfilerMarker _PRF_OnRectTransformDimensionsChange =
            new(_PRF_PFX + nameof(OnRectTransformDimensionsChange));

        private static readonly ProfilerMarker _PRF_PoolLogItem = new(_PRF_PFX + nameof(PoolLogItem));
        private static readonly ProfilerMarker _PRF_PopLogItem = new(_PRF_PFX + nameof(PopLogItem));
        private static readonly ProfilerMarker _PRF_ProcessLog = new(_PRF_PFX + nameof(ProcessLog));
        private static readonly ProfilerMarker _PRF_ReceivedLog = new(_PRF_PFX + nameof(ReceivedLog));

        private static readonly ProfilerMarker _PRF_Resize = new(_PRF_PFX + nameof(Resize));
        private static readonly ProfilerMarker _PRF_SaveLogsToFile = new(_PRF_PFX + nameof(SaveLogsToFile));

        private static readonly ProfilerMarker _PRF_SearchTermChanged =
            new(_PRF_PFX + nameof(SearchTermChanged));

        private static readonly ProfilerMarker _PRF_SetSnapToBottom = new(_PRF_PFX + nameof(SetSnapToBottom));
        private static readonly ProfilerMarker _PRF_ShowLogWindow = new(_PRF_PFX + nameof(ShowLogWindow));

        private static readonly ProfilerMarker _PRF_StripStackTraceFromLatestPendingLog =
            new(_PRF_PFX + nameof(StripStackTraceFromLatestPendingLog));

        private static readonly ProfilerMarker _PRF_Toggle = new(_PRF_PFX + nameof(Toggle));

        private static readonly ProfilerMarker _PRF_ValidateScrollPosition =
            new(_PRF_PFX + nameof(ValidateScrollPosition));

        #endregion
    }
}
