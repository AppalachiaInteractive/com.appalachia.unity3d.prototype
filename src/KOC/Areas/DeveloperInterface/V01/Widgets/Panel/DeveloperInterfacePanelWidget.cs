using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.ActivityBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.SideBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.StatusBar;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.Panel
{
    [CallStaticConstructorInEditor]
    public sealed class DeveloperInterfacePanelWidget : DeveloperInterfaceWidget<DeveloperInterfacePanelWidget
        , DeveloperInterfacePanelWidgetMetadata, DeveloperInterfaceManager_V01,
        DeveloperInterfaceMetadata_V01>
    {
        static DeveloperInterfacePanelWidget()
        {
            RegisterDependency<DeveloperInterfaceSideBarWidget>(i => _developerInterfaceSideBarWidget = i);
            RegisterDependency<DeveloperInterfaceActivityBarWidget>(
                i => _developerInterfaceActivityBarWidget = i
            );
            RegisterDependency<DeveloperInterfaceStatusBarWidget>(
                i => _developerInterfaceStatusBarWidget = i
            );
        }

        #region Static Fields and Autoproperties

        private static DeveloperInterfaceActivityBarWidget _developerInterfaceActivityBarWidget;

        private static DeveloperInterfaceSideBarWidget _developerInterfaceSideBarWidget;
        private static DeveloperInterfaceStatusBarWidget _developerInterfaceStatusBarWidget;

        #endregion

        protected override void OnApplyMetadataInternal()
        {
        }

        protected override void SubscribeToAllFunctionalties()
        {
            using (_PRF_SubscribeToAllFunctionalties.Auto())
            {
                _developerInterfaceSideBarWidget.VisuallyChanged += ApplyMetadata;
                _developerInterfaceActivityBarWidget.VisuallyChanged += ApplyMetadata;
                _developerInterfaceStatusBarWidget.VisuallyChanged += ApplyMetadata;
            }
        }

        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
                _developerInterfaceSideBarWidget.VisuallyChanged -= ApplyMetadata;
                _developerInterfaceActivityBarWidget.VisuallyChanged -= ApplyMetadata;
                _developerInterfaceStatusBarWidget.VisuallyChanged -= ApplyMetadata;
            }
        }

        protected override void UpdateSizeInternal()
        {
            using (_PRF_UpdateSizeInternal.Auto())
            {
                var sideBar = _developerInterfaceSideBarWidget;
                var activityBar = _developerInterfaceActivityBarWidget;
                var statusBar = _developerInterfaceStatusBarWidget;

                var anchorMin = rectTransform.anchorMin;
                var anchorMax = rectTransform.anchorMax;

                anchorMin.x = activityBar.EffectiveAnchorWidth + sideBar.EffectiveAnchorWidth;

                anchorMax.x = 1.0f;

                anchorMin.y = statusBar.EffectiveAnchorHeight;
                anchorMax.y = anchorMin.y + metadata.height;

                UpdateAnchorMin(anchorMin);
                UpdateAnchorMax(anchorMax);
            }
        }
    }
}
