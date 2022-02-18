namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar.Widgets
{
    public sealed class MenuBarWidget : DeveloperInterfaceManager_V01.Widget<MenuBarWidget,
        MenuBarWidgetMetadata, MenuBarFeature, MenuBarFeatureMetadata>
    {
        /// <inheritdoc />
        protected override void EnsureWidgetIsCorrectSize()
        {
            using (_PRF_EnsureWidgetIsCorrectSize.Auto())
            {
                base.EnsureWidgetIsCorrectSize();

                var anchorMin = RectTransform.anchorMin;
                var anchorMax = RectTransform.anchorMax;

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
