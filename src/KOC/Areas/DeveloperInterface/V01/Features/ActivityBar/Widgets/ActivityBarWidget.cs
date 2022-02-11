using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Model;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets;
using Appalachia.UI.Controls.Sets.Button;
using Appalachia.Utility.Async;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets
{
    [CallStaticConstructorInEditor]
    public sealed class ActivityBarWidget : DeveloperInterfaceManager_V01.Widget<ActivityBarWidget,
        ActivityBarWidgetMetadata, ActivityBarFeature, ActivityBarFeatureMetadata>
    {
        #region Constants and Static Readonly

        private const string BOTTOM_BUTTON_PARENT_NAME = BUTTON_PARENT_BASE + "BOTTOM";

        private const string BUTTON_PARENT_BASE = "Activity Buttons - ";
        private const string TOP_BUTTON_PARENT_NAME = BUTTON_PARENT_BASE + "TOP";

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

        [NonSerialized] private List<ButtonComponentSet> _topButtons;
        [NonSerialized] private List<ButtonComponentSet> _bottomButtons;
        [SerializeField] private GameObject _topButtonParent;
        [SerializeField] private GameObject _bottomButtonParent;

        #endregion

        public GameObject BottomButtonParent => _bottomButtonParent;

        public GameObject TopButtonParent => _topButtonParent;

        public IReadOnlyList<ActivityBarEntry> BottomEntries
        {
            get
            {
                if (Feature == null)
                {
                    return null;
                }

                return Feature.BottomEntries;
            }
        }

        public IReadOnlyList<ActivityBarEntry> TopEntries
        {
            get
            {
                if (Feature == null)
                {
                    return null;
                }

                return Feature.TopEntries;
            }
        }

        public IReadOnlyList<ButtonComponentSet> BottomButtons
        {
            get
            {
                _bottomButtons ??= new List<ButtonComponentSet>();
                return _bottomButtons;
            }
        }

        public IReadOnlyList<ButtonComponentSet> TopButtons
        {
            get
            {
                _topButtons ??= new List<ButtonComponentSet>();
                return _topButtons;
            }
        }

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                _statusBarWidget.VisuallyChanged.Event += OnDependencyChanged;
                _menuBarWidget.VisuallyChanged.Event += OnDependencyChanged;
            }
        }

        public void UpdateButtons()
        {
            using (_PRF_UpdateButtons.Auto())
            {
                UpdateActivitySection(
                    ref _topButtons,
                    ref _topButtonParent,
                    TOP_BUTTON_PARENT_NAME,
                    TopEntries
                );

                UpdateActivitySection(
                    ref _bottomButtons,
                    ref _bottomButtonParent,
                    BOTTOM_BUTTON_PARENT_NAME,
                    BottomEntries
                );
            }
        }

        protected override async AppaTask DelayEnabling()
        {
            await base.DelayEnabling();

            await AppaTask.WaitUntil(() => _menuBarWidget != null);
            await AppaTask.WaitUntil(() => _statusBarWidget != null);
        }

        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
                base.UnsubscribeFromAllFunctionalities();

                _menuBarWidget.VisuallyChanged.Event -= OnDependencyChanged;
                _statusBarWidget.VisuallyChanged.Event -= OnDependencyChanged;
            }
        }

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

        private void UpdateActivitySection(
            ref List<ButtonComponentSet> buttonList,
            ref GameObject parent,
            string parentName,
            IReadOnlyList<ActivityBarEntry> entries)
        {
            buttonList ??= new List<ButtonComponentSet>();

            canvas.GameObject.GetOrAddChild(ref parent, parentName, true);

            while (buttonList.Count > entries.Count)
            {
                var lastIndex = buttonList.Count - 1;

                var last = buttonList[lastIndex];
                last.GameObject.DestroySafely();

                buttonList.RemoveAt(lastIndex);
            }

            while (buttonList.Count < entries.Count)
            {
                var entry = entries[buttonList.Count];

                ButtonComponentSet newButtonComponentSet = null;

                metadata.UpdateComponentSet(
                    ref metadata._buttonData,
                    ref newButtonComponentSet,
                    parent,
                    entry.name
                );

                buttonList.Add(newButtonComponentSet);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_UpdateButtons =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateButtons));

        #endregion
    }
}
