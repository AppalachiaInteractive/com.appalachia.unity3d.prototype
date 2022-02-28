using System;
using System.Collections.Generic;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core;
using Appalachia.UI.Controls.Extensions;
using Appalachia.UI.Core.Components.Subsets;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets
{
    public sealed class StatusBarWidget : DeveloperInterfaceManager_V01.WidgetWithSingletonSubwidgets<
        IStatusBarSubwidget, IStatusBarSubwidgetMetadata, StatusBarWidget, StatusBarWidgetMetadata,
        StatusBarFeature, StatusBarFeatureMetadata>
    {
        #region Constants and Static Readonly

        private const string STATUS_BAR_ENTRY_PARENT_NAME = "Status Bar Subwidgets";

        #endregion

        #region Fields and Autoproperties

        private GameObject _statusBarSubwidgetParent;
        private List<IStatusBarSubwidget> _leftStatusBarSubwidgets;
        private List<IStatusBarSubwidget> _rightStatusBarSubwidgets;

        public HorizontalLayoutGroupSubset leftStatusBarLayoutGroup;

        public HorizontalLayoutGroupSubset rightStatusBarLayoutGroup;

        #endregion

        public GameObject StatusBarSubwidgetParent => _statusBarSubwidgetParent;
        public HorizontalLayoutGroupSubset LeftStatusBarLayoutGroup => leftStatusBarLayoutGroup;
        public HorizontalLayoutGroupSubset RightStatusBarLayoutGroup => rightStatusBarLayoutGroup;

        public IReadOnlyList<IStatusBarSubwidget> LeftStatusBarSubwidgets => _leftStatusBarSubwidgets;
        public IReadOnlyList<IStatusBarSubwidget> RightStatusBarSubwidgets => _rightStatusBarSubwidgets;

        public void SortSubwidgetsByPriority()
        {
            using (_PRF_SortEntriesByPriority.Auto())
            {
                SortSubwidgetsByPriority(_leftStatusBarSubwidgets);
                SortSubwidgetsByPriority(_rightStatusBarSubwidgets);
            }
        }

        public void ValidateSubwidgets()
        {
            using (_PRF_ValidateEntries.Auto())
            {
                RemoveIncorrectSubwidgetsFromList(
                    _leftStatusBarSubwidgets,
                    _rightStatusBarSubwidgets,
                    StatusBarSection.Left
                );

                RemoveIncorrectSubwidgetsFromList(
                    _rightStatusBarSubwidgets,
                    _leftStatusBarSubwidgets,
                    StatusBarSection.Right
                );

                EnsureSubwidgetHasCorrectParent(
                    _leftStatusBarSubwidgets,
                    leftStatusBarLayoutGroup.RectTransform
                );
                EnsureSubwidgetHasCorrectParent(
                    _rightStatusBarSubwidgets,
                    rightStatusBarLayoutGroup.RectTransform
                );

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

        protected override void OnRegisterSubwidget(IStatusBarSubwidget subwidget)
        {
            using (_PRF_OnRegisterSubwidget.Auto())
            {
                _leftStatusBarSubwidgets ??= new List<IStatusBarSubwidget>();
                _rightStatusBarSubwidgets ??= new List<IStatusBarSubwidget>();

                switch (subwidget.Metadata.Section)
                {
                    case StatusBarSection.Left:
                        _leftStatusBarSubwidgets.Add(subwidget);

                        break;
                    case StatusBarSection.Right:
                        _rightStatusBarSubwidgets.Add(subwidget);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                _leftStatusBarSubwidgets ??= new List<IStatusBarSubwidget>();
                _rightStatusBarSubwidgets ??= new List<IStatusBarSubwidget>();

                canvas.GameObject.GetOrAddChild(
                    ref _statusBarSubwidgetParent,
                    STATUS_BAR_ENTRY_PARENT_NAME,
                    true
                );

                ValidateSubwidgets();

                var canvasChildCount = canvas.RectTransform.childCount;

                var subwidgetRect = _statusBarSubwidgetParent.transform as RectTransform;
                subwidgetRect.FullScreen(true);

                _statusBarSubwidgetParent.transform.SetSiblingIndex(canvasChildCount - 1);
            }
        }

        private static void SortSubwidgetsByPriority(List<IStatusBarSubwidget> subwidgets)
        {
            using (_PRF_SortEntriesByPriority.Auto())
            {
                subwidgets.Sort((e1, e2) => e1.Metadata.Priority.CompareTo(e2.Metadata.Priority));
            }
        }

        private void EnsureSubwidgetHasCorrectParent(List<IStatusBarSubwidget> subwidgets, Transform parent)
        {
            using (_PRF_EnsureEntryHasCorrectParent.Auto())
            {
                for (var index = 0; index < subwidgets.Count; index++)
                {
                    var statusBarSubwidget = subwidgets[index];
                    statusBarSubwidget.Transform.SetParent(parent);
                }
            }
        }

        private void RemoveIncorrectSubwidgetsFromList(
            List<IStatusBarSubwidget> reviewing,
            List<IStatusBarSubwidget> other,
            StatusBarSection correctSection)
        {
            using (_PRF_RemoveIncorrectEntriesFromList.Auto())
            {
                for (var index = reviewing.Count - 1; index >= 0; index--)
                {
                    var statusBarSubwidget = reviewing[index];

                    if (statusBarSubwidget.Metadata.Section == correctSection)
                    {
                        continue;
                    }

                    reviewing.RemoveAt(index);
                    other.Add(statusBarSubwidget);
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_EnsureEntryHasCorrectParent =
            new ProfilerMarker(_PRF_PFX + nameof(EnsureSubwidgetHasCorrectParent));

        private static readonly ProfilerMarker _PRF_RemoveIncorrectEntriesFromList =
            new ProfilerMarker(_PRF_PFX + nameof(RemoveIncorrectSubwidgetsFromList));

        private static readonly ProfilerMarker _PRF_SortEntriesByPriority =
            new ProfilerMarker(_PRF_PFX + nameof(SortSubwidgetsByPriority));

        private static readonly ProfilerMarker _PRF_ValidateEntries =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateSubwidgets));

        #endregion
    }
}
