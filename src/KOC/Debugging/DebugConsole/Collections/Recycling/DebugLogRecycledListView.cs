using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Debugging.DebugConsole.Settings;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

// Handles the log items in an optimized way such that existing log items are
// recycled within the list instead of creating a new log item at each chance
namespace Appalachia.Prototype.KOC.Debugging.DebugConsole.Collections.Recycling
{
    [ExecutionOrder(ExecutionOrders.DebugLogRecycledListView)]
    public sealed class DebugLogRecycledListView : AppalachiaBehaviour<DebugLogRecycledListView>
    {
        #region Fields and Autoproperties

        // Log items used to visualize the debug entries at specified indices
        private Dictionary<int, DebugLogItem> logItemsAtIndices = new(256);

        private bool isCollapseOn;

        private DebugLogManager manager;
        private float deltaHeightOfSelectedLogEntry;
        private float heightOfSelectedLogEntry;

        private float logItemHeight, _1OverLogItemHeight;
        private float positionOfSelectedLogEntry = float.MaxValue;
        private float viewportHeight;

        // Current indices of debug entries shown on screen
        private int currentTopIndex = -1, currentBottomIndex = -1;

        private int indexOfSelectedLogEntry = int.MaxValue;
        private ScrollRect scrollView;

        private DebugLogManagerState state;

        #endregion

        public float ItemHeight => logItemHeight;
        public float SelectedItemHeight => heightOfSelectedLogEntry;

        // Indices of debug entries to show in collapsedLogEntries
        private DebugLogIndexList indicesOfEntriesToShow => state.indicesOfListEntriesToShow;

        // Unique debug entries
        private List<DebugLogEntry> collapsedLogEntries => state.collapsedLogEntries;

        // Deselect the currently selected log item
        public void DeselectSelectedLogItem()
        {
            var indexOfPreviouslySelectedLogEntry = indexOfSelectedLogEntry;
            indexOfSelectedLogEntry = int.MaxValue;

            positionOfSelectedLogEntry = float.MaxValue;
            heightOfSelectedLogEntry = deltaHeightOfSelectedLogEntry = 0f;

            if ((indexOfPreviouslySelectedLogEntry >= currentTopIndex) &&
                (indexOfPreviouslySelectedLogEntry <= currentBottomIndex))
            {
                ColorLogItem(
                    logItemsAtIndices[indexOfPreviouslySelectedLogEntry],
                    indexOfPreviouslySelectedLogEntry
                );
            }
        }

        public void Initialize(DebugLogManager manager, DebugLogManagerState state, float logItemHeight)
        {
            this.manager = manager;
            this.state = state;
            this.logItemHeight = logItemHeight;
            _1OverLogItemHeight = 1f / logItemHeight;
        }

        // A single collapsed log entry at specified index is updated, refresh its item if visible
        public void OnCollapsedLogEntryAtIndexUpdated(int index)
        {
            DebugLogItem logItem;
            if (logItemsAtIndices.TryGetValue(index, out logItem))
            {
                logItem.ShowCount();
            }
        }

        // Number of debug entries may be changed, update the list
        public void OnLogEntriesUpdated(bool updateAllVisibleItemContents)
        {
            CalculateContentHeight();
            viewportHeight = viewportTransform.rect.height;

            if (updateAllVisibleItemContents)
            {
                HardResetItems();
            }

            UpdateItemsInTheList(updateAllVisibleItemContents);
        }

        // A log item is clicked, highlight it
        public void OnLogItemClicked(DebugLogItem item)
        {
            OnLogItemClickedInternal(item.Index, item);
        }

        // Log window's height has changed, update the list
        public void OnViewportHeightChanged()
        {
            viewportHeight = viewportTransform.rect.height;
            UpdateItemsInTheList(false);
        }

        // Log window's width has changed, update the expanded (currently selected) log's height
        public void OnViewportWidthChanged()
        {
            if (indexOfSelectedLogEntry >= indicesOfEntriesToShow.Count)
            {
                return;
            }

            if (currentTopIndex == -1)
            {
                UpdateItemsInTheList(
                    false
                ); // Try to generate some DebugLogItems, we need one DebugLogItem to calculate the text height
                if (currentTopIndex == -1) // No DebugLogItems are generated, weird
                {
                    return;
                }
            }

            var referenceItem = logItemsAtIndices[currentTopIndex];

            heightOfSelectedLogEntry = referenceItem.CalculateExpandedHeight(
                collapsedLogEntries[indicesOfEntriesToShow[indexOfSelectedLogEntry]].ToString()
            );
            deltaHeightOfSelectedLogEntry = heightOfSelectedLogEntry - logItemHeight;

            CalculateContentHeight();

            HardResetItems();
            UpdateItemsInTheList(true);

            manager.ValidateScrollPosition();
        }

        // Force expand the log item at specified index
        public void SelectAndFocusOnLogItemAtIndex(int itemIndex)
        {
            if (indexOfSelectedLogEntry !=
                itemIndex) // Make sure that we aren't deselecting the target log item
            {
                OnLogItemClickedInternal(itemIndex);
            }

            var transformComponentCenterYAtTop = viewportHeight * 0.5f;
            var transformComponentCenterYAtBottom = transformComponent.sizeDelta.y - (viewportHeight * 0.5f);
            var transformComponentTargetCenterY = (itemIndex * logItemHeight) + (viewportHeight * 0.5f);
            if (Math.Abs(transformComponentCenterYAtTop - transformComponentCenterYAtBottom) < float.Epsilon)
            {
                scrollView.verticalNormalizedPosition = 0.5f;
            }
            else
            {
                scrollView.verticalNormalizedPosition = Mathf.Clamp01(
                    Mathf.InverseLerp(
                        transformComponentCenterYAtBottom,
                        transformComponentCenterYAtTop,
                        transformComponentTargetCenterY
                    )
                );
            }

            manager.SetSnapToBottom(false);
        }

        public void SetCollapseMode(bool collapse)
        {
            isCollapseOn = collapse;
        }

        // Calculate the indices of log entries to show
        // and handle log items accordingly
        public void UpdateItemsInTheList(bool updateAllVisibleItemContents)
        {
            using (_PRF_UpdateItemsInTheList.Auto())
            {
                // If there is at least one log entry to show
                if (indicesOfEntriesToShow.Count > 0)
                {
                    var contentPosTop = transformComponent.anchoredPosition.y - 1f;
                    var contentPosBottom = contentPosTop + viewportHeight + 2f;

                    if (positionOfSelectedLogEntry <= contentPosBottom)
                    {
                        if (positionOfSelectedLogEntry <= contentPosTop)
                        {
                            contentPosTop -= deltaHeightOfSelectedLogEntry;
                            contentPosBottom -= deltaHeightOfSelectedLogEntry;

                            if (contentPosTop < (positionOfSelectedLogEntry - 1f))
                            {
                                contentPosTop = positionOfSelectedLogEntry - 1f;
                            }

                            if (contentPosBottom < (contentPosTop + 2f))
                            {
                                contentPosBottom = contentPosTop + 2f;
                            }
                        }
                        else
                        {
                            contentPosBottom -= deltaHeightOfSelectedLogEntry;
                            if (contentPosBottom < (positionOfSelectedLogEntry + 1f))
                            {
                                contentPosBottom = positionOfSelectedLogEntry + 1f;
                            }
                        }
                    }

                    var newTopIndex = (int)(contentPosTop * _1OverLogItemHeight);
                    var newBottomIndex = (int)(contentPosBottom * _1OverLogItemHeight);

                    if (newTopIndex < 0)
                    {
                        newTopIndex = 0;
                    }

                    if (newBottomIndex > (indicesOfEntriesToShow.Count - 1))
                    {
                        newBottomIndex = indicesOfEntriesToShow.Count - 1;
                    }

                    if (currentTopIndex == -1)
                    {
                        // There are no log items visible on screen,
                        // just create the new log items
                        updateAllVisibleItemContents = true;

                        currentTopIndex = newTopIndex;
                        currentBottomIndex = newBottomIndex;

                        CreateLogItemsBetweenIndices(newTopIndex, newBottomIndex);
                    }
                    else
                    {
                        // There are some log items visible on screen

                        if ((newBottomIndex < currentTopIndex) || (newTopIndex > currentBottomIndex))
                        {
                            // If user scrolled a lot such that, none of the log items are now within
                            // the bounds of the scroll view, pool all the previous log items and create
                            // new log items for the new list of visible debug entries
                            updateAllVisibleItemContents = true;

                            DestroyLogItemsBetweenIndices(currentTopIndex, currentBottomIndex);
                            CreateLogItemsBetweenIndices(newTopIndex, newBottomIndex);
                        }
                        else
                        {
                            // User did not scroll a lot such that, there are still some log items within
                            // the bounds of the scroll view. Don't destroy them but update their content,
                            // if necessary
                            if (newTopIndex > currentTopIndex)
                            {
                                DestroyLogItemsBetweenIndices(currentTopIndex, newTopIndex - 1);
                            }

                            if (newBottomIndex < currentBottomIndex)
                            {
                                DestroyLogItemsBetweenIndices(newBottomIndex + 1, currentBottomIndex);
                            }

                            if (newTopIndex < currentTopIndex)
                            {
                                CreateLogItemsBetweenIndices(newTopIndex, currentTopIndex - 1);

                                // If it is not necessary to update all the log items,
                                // then just update the newly created log items. Otherwise,
                                // wait for the major update
                                if (!updateAllVisibleItemContents)
                                {
                                    UpdateLogItemContentsBetweenIndices(newTopIndex, currentTopIndex - 1);
                                }
                            }

                            if (newBottomIndex > currentBottomIndex)
                            {
                                CreateLogItemsBetweenIndices(currentBottomIndex + 1, newBottomIndex);

                                // If it is not necessary to update all the log items,
                                // then just update the newly created log items. Otherwise,
                                // wait for the major update
                                if (!updateAllVisibleItemContents)
                                {
                                    UpdateLogItemContentsBetweenIndices(
                                        currentBottomIndex + 1,
                                        newBottomIndex
                                    );
                                }
                            }
                        }

                        currentTopIndex = newTopIndex;
                        currentBottomIndex = newBottomIndex;
                    }

                    if (updateAllVisibleItemContents)
                    {
                        // Update all the log items
                        UpdateLogItemContentsBetweenIndices(currentTopIndex, currentBottomIndex);
                    }
                }
                else
                {
                    HardResetItems();
                }
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                await initializer.Do(
                    this,
                    nameof(logItemsAtIndices),
                    logItemsAtIndices == null,
                    () => logItemsAtIndices = new(256)
                );

                scrollView = await initializer.Get<ScrollRect>(
                    viewportTransform,
                    GetComponentStrategy.ParentObject
                );

                scrollView.onValueChanged.AddListener(pos => UpdateItemsInTheList(false));

                viewportHeight = viewportTransform.rect.height;
            }
        }

        private void CalculateContentHeight()
        {
            using (_PRF_CalculateContentHeight.Auto())
            {
                var newHeight = Mathf.Max(
                    1f,
                    (indicesOfEntriesToShow.Count * logItemHeight) + deltaHeightOfSelectedLogEntry
                );
                transformComponent.sizeDelta = new Vector2(0f, newHeight);
            }
        }

        // Color a log item using its index
        private void ColorLogItem(DebugLogItem logItem, int index)
        {
            using (_PRF_ColorLogItem.Auto())
            {
                if (index == indexOfSelectedLogEntry)
                {
                    logItem.Image.color = logItemSelectedColor;
                }
                else if ((index % 2) == 0)
                {
                    logItem.Image.color = logItemNormalColor1;
                }
                else
                {
                    logItem.Image.color = logItemNormalColor2;
                }
            }
        }

        // Create (or unpool) a log item
        private void CreateLogItemAtIndex(int index)
        {
            using (_PRF_CreateLogItemAtIndex.Auto())
            {
                var logItem = debugManager.PopLogItem();

                // Reposition the log item
                var anchoredPosition = new Vector2(1f, -index * logItemHeight);
                if (index > indexOfSelectedLogEntry)
                {
                    anchoredPosition.y -= deltaHeightOfSelectedLogEntry;
                }

                logItem.Transform.anchoredPosition = anchoredPosition;

                // Color the log item
                ColorLogItem(logItem, index);

                // To access this log item easily in the future, add it to the dictionary
                logItemsAtIndices[index] = logItem;
            }
        }

        private void CreateLogItemsBetweenIndices(int topIndex, int bottomIndex)
        {
            using (_PRF_CreateLogItemsBetweenIndices.Auto())
            {
                for (var i = topIndex; i <= bottomIndex; i++)
                {
                    CreateLogItemAtIndex(i);
                }
            }
        }

        private void DestroyLogItemsBetweenIndices(int topIndex, int bottomIndex)
        {
            using (_PRF_DestroyLogItemsBetweenIndices.Auto())
            {
                if (logItemsAtIndices.Count == 0)
                {
                    return;
                }

                for (var i = topIndex; i <= bottomIndex; i++)
                {
                    debugManager.PoolLogItem(logItemsAtIndices[i]);
                }
            }
        }

        private void HardResetItems()
        {
            if (currentTopIndex != -1)
            {
                DestroyLogItemsBetweenIndices(currentTopIndex, currentBottomIndex);
                currentTopIndex = -1;
            }
        }

        private void OnLogItemClickedInternal(int itemIndex, DebugLogItem referenceItem = null)
        {
            if (indexOfSelectedLogEntry != itemIndex)
            {
                DeselectSelectedLogItem();

                if (!referenceItem)
                {
                    if (currentTopIndex == -1)
                    {
                        UpdateItemsInTheList(
                            false
                        ); // Try to generate some DebugLogItems, we need one DebugLogItem to calculate the text height
                    }

                    referenceItem = logItemsAtIndices[currentTopIndex];
                }

                indexOfSelectedLogEntry = itemIndex;
                positionOfSelectedLogEntry = itemIndex * logItemHeight;
                heightOfSelectedLogEntry = referenceItem.CalculateExpandedHeight(
                    collapsedLogEntries[indicesOfEntriesToShow[itemIndex]].ToString()
                );
                deltaHeightOfSelectedLogEntry = heightOfSelectedLogEntry - logItemHeight;

                manager.SetSnapToBottom(false);
            }
            else
            {
                DeselectSelectedLogItem();
            }

            if ((indexOfSelectedLogEntry >= currentTopIndex) &&
                (indexOfSelectedLogEntry <= currentBottomIndex))
            {
                ColorLogItem(logItemsAtIndices[indexOfSelectedLogEntry], indexOfSelectedLogEntry);
            }

            CalculateContentHeight();

            HardResetItems();
            UpdateItemsInTheList(true);

            manager.ValidateScrollPosition();
        }

        private void UpdateLogItemContentsBetweenIndices(int topIndex, int bottomIndex)
        {
            DebugLogItem logItem;
            for (var i = topIndex; i <= bottomIndex; i++)
            {
                logItem = logItemsAtIndices[i];
                logItem.SetContent(
                    collapsedLogEntries[indicesOfEntriesToShow[i]],
                    i,
                    i == indexOfSelectedLogEntry
                );

                if (isCollapseOn)
                {
                    logItem.ShowCount();
                }
                else
                {
                    logItem.HideCount();
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(DebugLogRecycledListView) + ".";

        private static readonly ProfilerMarker _PRF_UpdateItemsInTheList =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateItemsInTheList));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_CalculateContentHeight =
            new ProfilerMarker(_PRF_PFX + nameof(CalculateContentHeight));

        private static readonly ProfilerMarker _PRF_ColorLogItem =
            new ProfilerMarker(_PRF_PFX + nameof(ColorLogItem));

        private static readonly ProfilerMarker _PRF_CreateLogItemAtIndex =
            new ProfilerMarker(_PRF_PFX + nameof(CreateLogItemAtIndex));

        private static readonly ProfilerMarker _PRF_CreateLogItemsBetweenIndices =
            new ProfilerMarker(_PRF_PFX + nameof(CreateLogItemsBetweenIndices));

        private static readonly ProfilerMarker _PRF_DestroyLogItemsBetweenIndices =
            new ProfilerMarker(_PRF_PFX + nameof(DestroyLogItemsBetweenIndices));

        #endregion

#pragma warning disable 0649

        // Cached components
        [SerializeField] private RectTransform transformComponent;

        [SerializeField] private RectTransform viewportTransform;

        [SerializeField] private DebugLogManager debugManager;

        [SerializeField] private Color logItemNormalColor1;

        [SerializeField] private Color logItemNormalColor2;

        [SerializeField] private Color logItemSelectedColor;
#pragma warning restore 0649
    }
}
