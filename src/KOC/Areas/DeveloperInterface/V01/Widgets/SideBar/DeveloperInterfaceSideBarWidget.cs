using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.ActivityBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.MenuBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.StatusBar;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.SideBar
{
    [CallStaticConstructorInEditor]
    public sealed class DeveloperInterfaceSideBarWidget : DeveloperInterfaceWidget<
        DeveloperInterfaceSideBarWidget, DeveloperInterfaceSideBarWidgetMetadata,
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        static DeveloperInterfaceSideBarWidget()
        {
            RegisterDependency<DeveloperInterfaceActivityBarWidget>(
                i => _developerInterfaceActivityBarWidget = i
            );
            RegisterDependency<DeveloperInterfaceMenuBarWidget>(i => _developerInterfaceMenuBarWidget = i);
            RegisterDependency<DeveloperInterfaceStatusBarWidget>(
                i => _developerInterfaceStatusBarWidget = i
            );
        }

        #region Static Fields and Autoproperties

        private static DeveloperInterfaceActivityBarWidget _developerInterfaceActivityBarWidget;
        private static DeveloperInterfaceMenuBarWidget _developerInterfaceMenuBarWidget;
        private static DeveloperInterfaceStatusBarWidget _developerInterfaceStatusBarWidget;

        #endregion

        protected override void OnApplyMetadataInternal()
        {
        }

        protected override void SubscribeToAllFunctionalties()
        {
            using (_PRF_SubscribeToAllFunctionalties.Auto())
            {
                _developerInterfaceActivityBarWidget.VisuallyChanged += ApplyMetadata;
                _developerInterfaceMenuBarWidget.VisuallyChanged += ApplyMetadata;
                _developerInterfaceStatusBarWidget.VisuallyChanged += ApplyMetadata;
            }
        }

        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
                _developerInterfaceActivityBarWidget.VisuallyChanged -= ApplyMetadata;
                _developerInterfaceMenuBarWidget.VisuallyChanged -= ApplyMetadata;
                _developerInterfaceStatusBarWidget.VisuallyChanged -= ApplyMetadata;
            }
        }

        protected override void UpdateSizeInternal()
        {
            using (_PRF_UpdateSizeInternal.Auto())
            {
                var activityBar = _developerInterfaceActivityBarWidget;
                var menuBar = _developerInterfaceMenuBarWidget;
                var statusBar = _developerInterfaceStatusBarWidget;

                var anchorMin = rectTransform.anchorMin;
                var anchorMax = rectTransform.anchorMax;

                anchorMin.x = activityBar.EffectiveAnchorWidth;
                anchorMax.x = anchorMin.x + metadata.width;

                anchorMin.y = statusBar.EffectiveAnchorHeight;
                anchorMax.y = 1.0f - menuBar.EffectiveAnchorHeight;

                UpdateAnchorMin(anchorMin);
                UpdateAnchorMax(anchorMax);
            }
        }
    }
}
