using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.ViewScaling.Services;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Panel.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets.Models;
using Appalachia.UI.Controls.Components.Layout.Models;
using Appalachia.Utility.Async;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.GameArea.Widgets
{
    [CallStaticConstructorInEditor]
    public sealed class GameAreaWidget : DeveloperInterfaceManager_V01.Widget<GameAreaWidget,
        GameAreaWidgetMetadata, GameAreaFeature, GameAreaFeatureMetadata>
    {
        static GameAreaWidget()
        {
            When.Widget(_activityBarWidget).IsAvailableThen(widget => _activityBarWidget = widget);

            When.Widget(_menuBarWidget).IsAvailableThen(widget => _menuBarWidget = widget);

            When.Widget(_panelWidget).IsAvailableThen(widget => _panelWidget = widget);

            When.Widget(_sideBarWidget).IsAvailableThen(widget => _sideBarWidget = widget);

            When.Widget(_statusBarWidget).IsAvailableThen(widget => _statusBarWidget = widget);

            When.Service<ViewScalingService>().IsAvailableThen(i => _canvasScalingService = i);
        }

        #region Static Fields and Autoproperties

        private static ActivityBarWidget _activityBarWidget;
        private static MenuBarWidget _menuBarWidget;
        private static PanelWidget _panelWidget;
        private static SideBarWidget _sideBarWidget;
        private static StatusBarWidget _statusBarWidget;

        private static ViewScalingService _canvasScalingService;

        #endregion

        #region Fields and Autoproperties

        private WidgetDimensions _activeArea;
        private WidgetDimensions _entireArea;

        #endregion

        public WidgetDimensions activeArea => _activeArea;
        public WidgetDimensions entireArea => _entireArea;

        /// <inheritdoc />
        protected override async AppaTask DelayEnabling()
        {
            await base.DelayEnabling();
            await AppaTask.WaitUntil(() => _activityBarWidget != null);
            await AppaTask.WaitUntil(() => _menuBarWidget != null);
            await AppaTask.WaitUntil(() => _panelWidget != null);
            await AppaTask.WaitUntil(() => _sideBarWidget != null);
            await AppaTask.WaitUntil(() => _statusBarWidget != null);
            await AppaTask.WaitUntil(() => _canvasScalingService != null);
        }

        /// <inheritdoc />
        protected override void EnsureWidgetIsCorrectSize()
        {
            using (_PRF_EnsureWidgetIsCorrectSize.Auto())
            {
                base.EnsureWidgetIsCorrectSize();

                var activityBar = _activityBarWidget;
                var menuBar = _menuBarWidget;
                var panel = _panelWidget;
                var sideBar = _sideBarWidget;
                var statusBar = _statusBarWidget;

                var anchorMin = RectTransform.anchorMin;
                var anchorMax = RectTransform.anchorMax;

                anchorMin.x = activityBar.EffectiveAnchorWidth + sideBar.EffectiveAnchorWidth;

                anchorMax.x = 1.00f;

                anchorMin.y = statusBar.EffectiveAnchorHeight + panel.EffectiveAnchorHeight;

                anchorMax.y = 1.0f - menuBar.EffectiveAnchorHeight;

                var anchorWidth = anchorMax.x - anchorMin.x;
                var anchorHeight = anchorMax.y - anchorMin.y;

                var anchoredCenterX = anchorMin.x + (anchorWidth * .5f);
                var anchoredCenterY = anchorMin.y + (anchorHeight * .5f);

                if (metadata.maintainAspectRatio)
                {
                    var smallerDimension = Mathf.Min(anchorWidth, anchorHeight);
                    var halfDimension = smallerDimension * 0.5f;

                    anchorMin.x = anchoredCenterX - halfDimension;
                    anchorMax.x = anchoredCenterX + halfDimension;
                    anchorMin.y = anchoredCenterY - halfDimension;
                    anchorMax.y = anchoredCenterY + halfDimension;
                }

                UpdateAnchorMin(anchorMin);
                UpdateAnchorMax(anchorMax);

                var resultingRect = new Rect
                {
                    xMin = anchorMin.x,
                    xMax = anchorMax.x,
                    yMin = anchorMin.y,
                    yMax = anchorMax.y
                };

                var dimensionData = new ViewDimensionData(resultingRect);

                _canvasScalingService.UpdateViewScaling(dimensionData);
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                _activeArea = new WidgetDimensions();
                _entireArea = new WidgetDimensions();
            }
        }

        /// <inheritdoc />
        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
                base.UnsubscribeFromAllFunctionalities();

                _activityBarWidget.VisualUpdate.Event -= OnRequiresUpdate;
                _menuBarWidget.VisualUpdate.Event -= OnRequiresUpdate;
                _panelWidget.VisualUpdate.Event -= OnRequiresUpdate;
                _sideBarWidget.VisualUpdate.Event -= OnRequiresUpdate;
                _statusBarWidget.VisualUpdate.Event -= OnRequiresUpdate;
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                _activityBarWidget.VisualUpdate.Event += OnRequiresUpdate;
                _menuBarWidget.VisualUpdate.Event += OnRequiresUpdate;
                _panelWidget.VisualUpdate.Event += OnRequiresUpdate;
                _sideBarWidget.VisualUpdate.Event += OnRequiresUpdate;
                _statusBarWidget.VisualUpdate.Event += OnRequiresUpdate;
            }
        }
    }
}
