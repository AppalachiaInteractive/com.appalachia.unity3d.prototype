using System;
using System.Collections.Generic;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Entries.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Entries.Core;
using Appalachia.UI.Controls.Extensions;
using Appalachia.UI.Core.Components.Subsets;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets
{
    public sealed class StatusBarWidget : DeveloperInterfaceManager_V01.Widget<StatusBarWidget,
        StatusBarWidgetMetadata, StatusBarFeature, StatusBarFeatureMetadata>
    {
        #region Constants and Static Readonly

        private const string STATUS_BAR_ENTRY_PARENT_NAME = "Status Bar Entries";

        #endregion

        #region Fields and Autoproperties

        private GameObject _statusBarEntryParent;
        private List<IStatusBarEntry> _leftStatusBarEntries;
        private List<IStatusBarEntry> _rightStatusBarEntries;

        public HorizontalLayoutGroupSubset leftStatusBarLayoutGroup;

        public HorizontalLayoutGroupSubset rightStatusBarLayoutGroup;

        #endregion

        public GameObject StatusBarEntryParent => _statusBarEntryParent;
        public HorizontalLayoutGroupSubset LeftStatusBarLayoutGroup => leftStatusBarLayoutGroup;
        public HorizontalLayoutGroupSubset RightStatusBarLayoutGroup => rightStatusBarLayoutGroup;

        public IReadOnlyList<IStatusBarEntry> LeftStatusBarEntries => _leftStatusBarEntries;
        public IReadOnlyList<IStatusBarEntry> RightStatusBarEntries => _rightStatusBarEntries;

        /// <summary>
        ///     Adds the specified status bar entry to the appropriate layout group, and refreshes the layout.
        /// </summary>
        /// <param name="statusBarEntry">The status bar entry to add.</param>
        public void RegisterStatus(IStatusBarEntry statusBarEntry)
        {
            using (_PRF_RegisterStatus.Auto())
            {
                _leftStatusBarEntries ??= new List<IStatusBarEntry>();
                _rightStatusBarEntries ??= new List<IStatusBarEntry>();

                switch (statusBarEntry.Metadata.Section)
                {
                    case StatusBarSection.Left:
                        _leftStatusBarEntries.Add(statusBarEntry);

                        break;
                    case StatusBarSection.Right:
                        _rightStatusBarEntries.Add(statusBarEntry);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void SortEntriesByPriority()
        {
            using (_PRF_SortEntriesByPriority.Auto())
            {
                SortEntriesByPriority(_leftStatusBarEntries);
                SortEntriesByPriority(_rightStatusBarEntries);
            }
        }

        public void ValidateEntries()
        {
            using (_PRF_ValidateEntries.Auto())
            {
                RemoveIncorrectEntriesFromList(
                    _leftStatusBarEntries,
                    _rightStatusBarEntries,
                    StatusBarSection.Left
                );

                RemoveIncorrectEntriesFromList(
                    _rightStatusBarEntries,
                    _leftStatusBarEntries,
                    StatusBarSection.Right
                );

                EnsureEntryHasCorrectParent(_leftStatusBarEntries,  leftStatusBarLayoutGroup.RectTransform);
                EnsureEntryHasCorrectParent(_rightStatusBarEntries, rightStatusBarLayoutGroup.RectTransform);

                LayoutRebuilder.MarkLayoutForRebuild(leftStatusBarLayoutGroup.RectTransform);
                LayoutRebuilder.MarkLayoutForRebuild(rightStatusBarLayoutGroup.RectTransform);
            }
        }

        /// <inheritdoc />
        protected override void EnsureWidgetIsCorrectSize()
        {
            using (_PRF_EnsureWidgetIsCorrectSize.Auto())
            {
                base.EnsureWidgetIsCorrectSize();

                var anchorMin = RectTransform.anchorMin;
                var anchorMax = RectTransform.anchorMax;

                anchorMin.x = 0.00f;
                anchorMax.x = 1.00f;

                anchorMin.y = 0.00f;
                anchorMax.y = metadata.height;

                UpdateAnchorMin(anchorMin);
                UpdateAnchorMax(anchorMax);
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                _leftStatusBarEntries ??= new List<IStatusBarEntry>();
                _rightStatusBarEntries ??= new List<IStatusBarEntry>();

                canvas.GameObject.GetOrAddChild(
                    ref _statusBarEntryParent,
                    STATUS_BAR_ENTRY_PARENT_NAME,
                    true
                );

                ValidateEntries();

                var canvasChildCount = canvas.RectTransform.childCount;

                var entriesRect = _statusBarEntryParent.transform as RectTransform;
                entriesRect.FullScreen(true);
                
                _statusBarEntryParent.transform.SetSiblingIndex(canvasChildCount - 1);
            }
        }

        private static void SortEntriesByPriority(List<IStatusBarEntry> entries)
        {
            using (_PRF_SortEntriesByPriority.Auto())
            {
                entries.Sort((e1, e2) => e1.Metadata.Priority.CompareTo(e2.Metadata.Priority));
            }
        }

        private void EnsureEntryHasCorrectParent(List<IStatusBarEntry> entries, Transform parent)
        {
            using (_PRF_EnsureEntryHasCorrectParent.Auto())
            {
                for (var index = 0; index < entries.Count; index++)
                {
                    var statusBarEntry = entries[index];
                    statusBarEntry.Transform.SetParent(parent);
                }
            }
        }

        private void RemoveIncorrectEntriesFromList(
            List<IStatusBarEntry> reviewing,
            List<IStatusBarEntry> other,
            StatusBarSection correctSection)
        {
            using (_PRF_RemoveIncorrectEntriesFromList.Auto())
            {
                for (var index = reviewing.Count - 1; index >= 0; index--)
                {
                    var statusBarEntry = reviewing[index];

                    if (statusBarEntry.Metadata.Section == correctSection)
                    {
                        continue;
                    }

                    reviewing.RemoveAt(index);
                    other.Add(statusBarEntry);
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_EnsureEntryHasCorrectParent =
            new ProfilerMarker(_PRF_PFX + nameof(EnsureEntryHasCorrectParent));

        private static readonly ProfilerMarker _PRF_RegisterStatus =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterStatus));

        private static readonly ProfilerMarker _PRF_RemoveIncorrectEntriesFromList =
            new ProfilerMarker(_PRF_PFX + nameof(RemoveIncorrectEntriesFromList));

        private static readonly ProfilerMarker _PRF_SortEntriesByPriority =
            new ProfilerMarker(_PRF_PFX + nameof(SortEntriesByPriority));

        private static readonly ProfilerMarker _PRF_ValidateEntries =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateEntries));

        #endregion
    }
}
