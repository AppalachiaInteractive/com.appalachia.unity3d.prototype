using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Controls.Main;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Core;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets;
using Appalachia.Utility.Async;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets
{
    [CallStaticConstructorInEditor]
    public sealed partial class ActivityBarWidget : DeveloperInterfaceManager_V01.WidgetWithSingletonSubwidgets<
        IActivityBarSubwidget, IActivityBarSubwidgetMetadata, ActivityBarWidget, ActivityBarWidgetMetadata,
        ActivityBarFeature, ActivityBarFeatureMetadata>
    {
        static ActivityBarWidget()
        {
            When.Widget(_menuBarWidget).IsAvailableThen(menuBarWidget => { _menuBarWidget = menuBarWidget; });
            When.Widget(_statusBarWidget).IsAvailableThen(statusBarWidget => { _statusBarWidget = statusBarWidget; });
        }

        #region Static Fields and Autoproperties

        private static MenuBarWidget _menuBarWidget;
        private static StatusBarWidget _statusBarWidget;

        #endregion

        #region Fields and Autoproperties

        private List<IActivityBarSubwidget> _topActivityBarSubwidgets;
        private List<IActivityBarSubwidget> _bottomActivityBarSubwidgets;

        public ActivityBarControl activityBar;

        #endregion

        public ActivityBarControl ActivityBarControl => activityBar;

        public IReadOnlyList<IActivityBarSubwidget> BottomActivityBarSubwidgets => _bottomActivityBarSubwidgets;

        public IReadOnlyList<IActivityBarSubwidget> TopActivityBarSubwidgets => _topActivityBarSubwidgets;

        public override void ValidateSubwidgets()
        {
            using (_PRF_ValidateSubwidgets.Auto())
            {
                ActivityBarControl.Refresh(ref activityBar, canvas.ChildContainer, nameof(activityBar));
                
                RemoveIncorrectSubwidgetsFromList(
                    _topActivityBarSubwidgets,
                    _bottomActivityBarSubwidgets,
                    e => e.Metadata.Section == ActivityBarSection.Top
                );

                RemoveIncorrectSubwidgetsFromList(
                    _bottomActivityBarSubwidgets,
                    _topActivityBarSubwidgets,
                    e => e.Metadata.Section == ActivityBarSection.Bottom
                );

                EnsureSubwidgetsHaveCorrectParent(_topActivityBarSubwidgets, activityBar.TopActivityBar.RectTransform);
                EnsureSubwidgetsHaveCorrectParent(
                    _bottomActivityBarSubwidgets,
                    activityBar.bottomActivityBar.RectTransform
                );

                LayoutRebuilder.MarkLayoutForRebuild(activityBar.topActivityBar.RectTransform);
                LayoutRebuilder.MarkLayoutForRebuild(activityBar.bottomActivityBar.RectTransform);
            }
        }

        public void SortSubwidgetsByPriority()
        {
            using (_PRF_SortSubwidgetsByPriority.Auto())
            {
                SortSubwidgetsByPriority<IActivityBarSubwidget, IActivityBarSubwidgetMetadata>(
                    _topActivityBarSubwidgets
                );
                SortSubwidgetsByPriority<IActivityBarSubwidget, IActivityBarSubwidgetMetadata>(
                    _bottomActivityBarSubwidgets
                );
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
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                ActivityBarControl.Refresh(ref activityBar, canvas.ChildContainer, nameof(activityBar));
            }
        }

        protected override void OnRegisterSubwidget(IActivityBarSubwidget subwidget)
        {
            using (_PRF_OnRegisterSubwidget.Auto())
            {
                _topActivityBarSubwidgets ??= new List<IActivityBarSubwidget>();
                _bottomActivityBarSubwidgets ??= new List<IActivityBarSubwidget>();

                switch (subwidget.Metadata.Section)
                {
                    case ActivityBarSection.Top:
                        _topActivityBarSubwidgets.Add(subwidget);

                        break;
                    case ActivityBarSection.Bottom:
                        _bottomActivityBarSubwidgets.Add(subwidget);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <inheritdoc />
        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
                base.UnsubscribeFromAllFunctionalities();

                _menuBarWidget.VisualUpdate.Event -= OnRequiresUpdate;
                _statusBarWidget.VisualUpdate.Event -= OnRequiresUpdate;
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                _statusBarWidget.VisualUpdate.Event += OnRequiresUpdate;
                _menuBarWidget.VisualUpdate.Event += OnRequiresUpdate;

                _topActivityBarSubwidgets ??= new List<IActivityBarSubwidget>();
                _bottomActivityBarSubwidgets ??= new List<IActivityBarSubwidget>();

                ValidateSubwidgets();
            }
        }
    }
}
