using Appalachia.Prototype.KOC.Areas.Common.Widgets;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.StatusBar
{
    public sealed class DeveloperInterfaceStatusBarWidget : AreaWidget<DeveloperInterfaceStatusBarWidget,
        DeveloperInterfaceStatusBarWidgetMetadata, DeveloperInterfaceManager_V01,
        DeveloperInterfaceMetadata_V01>
    {
        protected override void OnApplyMetadataInternal()
        {
            using (_PRF_OnApplyMetadataInternal.Auto())
            {
            }
        }

        protected override void SubscribeToAllFunctionalties()
        {
            using (_PRF_SubscribeToAllFunctionalties.Auto())
            {
            }
        }

        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
            }
        }

        protected override void UpdateSizeInternal()
        {
            using (_PRF_UpdateSizeInternal.Auto())
            {
                var anchorMin = rectTransform.anchorMin;
                var anchorMax = rectTransform.anchorMax;

                anchorMin.x = 0.00f;
                anchorMax.x = 1.00f;

                anchorMin.y = 0.00f;
                anchorMax.y = metadata.height;

                rectTransform.anchorMin = anchorMin;
                rectTransform.anchorMax = anchorMax;
            }
        }
    }
}
