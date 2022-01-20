using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Services.Broadcast.CanvasScaling;
using Appalachia.Prototype.KOC.Areas.Common.Widgets.Models;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.ActivityBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.MenuBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.Panel;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.SideBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.StatusBar;
using Appalachia.UI.Controls.Components.Layout.Models;
using Appalachia.Utility.Async;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.GameArea
{
    [CallStaticConstructorInEditor]
    public sealed class DeveloperInterfaceGameAreaWidget : DeveloperInterfaceWidget<
        DeveloperInterfaceGameAreaWidget, DeveloperInterfaceGameAreaWidgetMetadata,
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        static DeveloperInterfaceGameAreaWidget()
        {
            RegisterDependency<DeveloperInterfaceActivityBarWidget>(
                i => _developerInterfaceActivityBarWidget = i
            );
            RegisterDependency<DeveloperInterfaceMenuBarWidget>(i => _developerInterfaceMenuBarWidget = i);
            RegisterDependency<DeveloperInterfacePanelWidget>(i => _developerInterfacePanelWidget = i);
            RegisterDependency<DeveloperInterfaceSideBarWidget>(i => _developerInterfaceSideBarWidget = i);
            RegisterDependency<DeveloperInterfaceStatusBarWidget>(
                i => _developerInterfaceStatusBarWidget = i
            );

            RegisterDependency<CanvasScalingService>(i => _canvasScalingService = i);
        }

        #region Static Fields and Autoproperties

        private static CanvasScalingService _canvasScalingService;

        private static DeveloperInterfaceActivityBarWidget _developerInterfaceActivityBarWidget;
        private static DeveloperInterfaceMenuBarWidget _developerInterfaceMenuBarWidget;
        private static DeveloperInterfacePanelWidget _developerInterfacePanelWidget;
        private static DeveloperInterfaceSideBarWidget _developerInterfaceSideBarWidget;
        private static DeveloperInterfaceStatusBarWidget _developerInterfaceStatusBarWidget;

        #endregion

        #region Fields and Autoproperties

        private WidgetDimensions _activeArea;
        private WidgetDimensions _entireArea;

        #endregion

        public WidgetDimensions activeArea => _activeArea;
        public WidgetDimensions entireArea => _entireArea;

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                _activeArea = new WidgetDimensions();
                _entireArea = new WidgetDimensions();
            }
        }

        protected override void OnApplyMetadataInternal()
        {
        }

        protected override void SubscribeToAllFunctionalties()
        {
            using (_PRF_SubscribeToAllFunctionalties.Auto())
            {
                _developerInterfaceActivityBarWidget.VisuallyChanged += ApplyMetadata;
                _developerInterfaceMenuBarWidget.VisuallyChanged += ApplyMetadata;
                _developerInterfacePanelWidget.VisuallyChanged += ApplyMetadata;
                _developerInterfaceSideBarWidget.VisuallyChanged += ApplyMetadata;
                _developerInterfaceStatusBarWidget.VisuallyChanged += ApplyMetadata;
            }
        }

        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
                _developerInterfaceActivityBarWidget.VisuallyChanged -= ApplyMetadata;
                _developerInterfaceMenuBarWidget.VisuallyChanged -= ApplyMetadata;
                _developerInterfacePanelWidget.VisuallyChanged -= ApplyMetadata;
                _developerInterfaceSideBarWidget.VisuallyChanged -= ApplyMetadata;
                _developerInterfaceStatusBarWidget.VisuallyChanged -= ApplyMetadata;
            }
        }

        protected override void UpdateSizeInternal()
        {
            using (_PRF_UpdateSizeInternal.Auto())
            {
                var activityBar = _developerInterfaceActivityBarWidget;
                var menuBar = _developerInterfaceMenuBarWidget;
                var panel = _developerInterfacePanelWidget;
                var sideBar = _developerInterfaceSideBarWidget;
                var statusBar = _developerInterfaceStatusBarWidget;

                var anchorMin = rectTransform.anchorMin;
                var anchorMax = rectTransform.anchorMax;

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

                var dimensionData = new CanvasDimensionData(resultingRect);

                _canvasScalingService.UpdateCanvasScaling(dimensionData);
            }
        }
    }
}
