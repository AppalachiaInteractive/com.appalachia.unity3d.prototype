namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.MenuBar
{
    public sealed class DeveloperInterfaceMenuBarWidget : DeveloperInterfaceWidget<
        DeveloperInterfaceMenuBarWidget, DeveloperInterfaceMenuBarWidgetMetadata,
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        protected override void OnApplyMetadataInternal()
        {
        }

        protected override void SubscribeToAllFunctionalties()
        {
        }

        protected override void UnsubscribeFromAllFunctionalities()
        {
        }

        protected override void UpdateSizeInternal()
        {
            using (_PRF_UpdateSizeInternal.Auto())
            {
                var anchorMin = rectTransform.anchorMin;
                var anchorMax = rectTransform.anchorMax;

                anchorMin.x = 0.0f;
                anchorMax.x = 1.0f;

                anchorMin.y = 1.0f - metadata.height;
                anchorMax.y = 1.0f;

                UpdateAnchorMin(anchorMin);
                UpdateAnchorMax(anchorMax);
            }
        }
    }
}
