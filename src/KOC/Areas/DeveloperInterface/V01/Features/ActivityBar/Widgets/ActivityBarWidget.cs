using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Entries;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Entries.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Entries.Core;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets;
using Appalachia.UI.Core.Components.Subsets;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets
{
    [CallStaticConstructorInEditor]
    public sealed class ActivityBarWidget : DeveloperInterfaceManager_V01.Widget<ActivityBarWidget,
        ActivityBarWidgetMetadata, ActivityBarFeature, ActivityBarFeatureMetadata>
    {
        #region Constants and Static Readonly

        private const string ACTIVITY_BAR_ENTRY_PARENT_NAME = "Activity Bar Entries";

        #endregion

        static ActivityBarWidget()
        {
            When.Widget(_menuBarWidget).IsAvailableThen(menuBarWidget => { _menuBarWidget = menuBarWidget; });
            When.Widget(_statusBarWidget)
                .IsAvailableThen(statusBarWidget => { _statusBarWidget = statusBarWidget; });
        }

        #region Static Fields and Autoproperties

        private static MenuBarWidget _menuBarWidget;
        private static StatusBarWidget _statusBarWidget;

        #endregion

        #region Fields and Autoproperties

        private GameObject _activityBarEntryParent;
        private List<IActivityBarEntry> _topActivityBarEntries;
        private List<IActivityBarEntry> _bottomActivityBarEntries;

        [FormerlySerializedAs("topLayoutGroup")]
        public VerticalLayoutGroupSubset topActivityBarLayoutGroup;

        [FormerlySerializedAs("bottomLayoutGroup")]
        public VerticalLayoutGroupSubset bottomActivityBarLayoutGroup;

        #endregion

        public GameObject ActivityBarEntryParent => _activityBarEntryParent;
        public IReadOnlyList<IActivityBarEntry> BottomActivityBarEntries => _bottomActivityBarEntries;

        public IReadOnlyList<IActivityBarEntry> TopActivityBarEntries => _topActivityBarEntries;
        public VerticalLayoutGroupSubset BottomActivityBarLayoutGroup => bottomActivityBarLayoutGroup;
        public VerticalLayoutGroupSubset TopActivityBarLayoutGroup => topActivityBarLayoutGroup;

        /// <summary>
        ///     Adds the specified activity bar entry to the appropriate layout group, and refreshes the layout.
        /// </summary>
        /// <param name="activityBarEntry">The activity bar entry to add.</param>
        public void RegisterActivity(IActivityBarEntry activityBarEntry)
        {
            using (_PRF_RegisterActivity.Auto())
            {
                // TODO implement this

                switch (activityBarEntry.Metadata.Section)
                {
                    case ActivityBarSection.Top:
                        _topActivityBarEntries.Add(activityBarEntry);

                        activityBarEntry.Transform.SetParent(topActivityBarLayoutGroup.RectTransform);
                        LayoutRebuilder.MarkLayoutForRebuild(topActivityBarLayoutGroup.RectTransform);

                        break;
                    case ActivityBarSection.Bottom:
                        _bottomActivityBarEntries.Add(activityBarEntry);

                        activityBarEntry.Transform.SetParent(bottomActivityBarLayoutGroup.RectTransform);
                        LayoutRebuilder.MarkLayoutForRebuild(bottomActivityBarLayoutGroup.RectTransform);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <inheritdoc />
        protected override async AppaTask DelayEnabling()
        {
            await base.DelayEnabling();

            await AppaTask.WaitUntil(() => _menuBarWidget != null);
            await AppaTask.WaitUntil(() => _statusBarWidget != null);
        }

        /// <inheritdoc />
        protected override void EnsureWidgetIsCorrectSize()
        {
            using (_PRF_EnsureWidgetIsCorrectSize.Auto())
            {
                base.EnsureWidgetIsCorrectSize();

                var menuBar = _menuBarWidget;
                var statusBar = _statusBarWidget;

                var anchorMin = RectTransform.anchorMin;
                var anchorMax = RectTransform.anchorMax;

                anchorMin.x = 0.00f;
                anchorMax.x = metadata.width;

                anchorMin.y = statusBar.EffectiveAnchorHeight;
                anchorMax.y = 1.0f - menuBar.EffectiveAnchorHeight;

                UpdateAnchorMin(anchorMin);
                UpdateAnchorMax(anchorMax);
            }
        }

        /// <inheritdoc />
        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
                base.UnsubscribeFromAllFunctionalities();

                _menuBarWidget.VisuallyChanged.Event -= OnDependencyChanged;
                _statusBarWidget.VisuallyChanged.Event -= OnDependencyChanged;
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                _statusBarWidget.VisuallyChanged.Event += OnDependencyChanged;
                _menuBarWidget.VisuallyChanged.Event += OnDependencyChanged;
                _topActivityBarEntries ??= new List<IActivityBarEntry>();
                _bottomActivityBarEntries ??= new List<IActivityBarEntry>();

                canvas.GameObject.GetOrAddChild(
                    ref _activityBarEntryParent,
                    ACTIVITY_BAR_ENTRY_PARENT_NAME,
                    true
                );

                var canvasChildCount = canvas.RectTransform.childCount;
                _activityBarEntryParent.transform.SetSiblingIndex(canvasChildCount - 1);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterActivity =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterActivity));

        #endregion
    }
}
