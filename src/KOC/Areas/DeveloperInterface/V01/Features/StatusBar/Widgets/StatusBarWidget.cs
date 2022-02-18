namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets
{
    public sealed class StatusBarWidget : DeveloperInterfaceManager_V01.Widget<StatusBarWidget,
        StatusBarWidgetMetadata, StatusBarFeature, StatusBarFeatureMetadata>
    {
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
    }
}
