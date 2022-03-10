using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Services;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Widgets
{
    public sealed class DevTooltipsWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetWithInstancedSubwidgetsMetadata
    <DevTooltipSubwidget, DevTooltipSubwidgetMetadata, IDevTooltipSubwidget, IDevTooltipSubwidgetMetadata,
        DevTooltipsWidget, DevTooltipsWidgetMetadata, DevTooltipsFeature, DevTooltipsFeatureMetadata>
    {
        #region Static Fields and Autoproperties

        private static RectVisualizerService _rectVisualizerService;

        #endregion

        protected override bool ShowsTooltip => false;
        
        protected override bool HideAnimationDurationField => true;
        protected override bool HideBackgroundField => true;
        protected override bool HideCanvasField => true;
        protected override bool HideFeatureDisabledVisibilityModeField => true;
        protected override bool HideFeatureEnabledVisibilityModeField => true;
        protected override bool HideFontStyleField => false;
        protected override bool HideRoundedBackgroundField => true;
        protected override bool HideTransitionsWithFadeField => true;

        protected override void UpdateFunctionalityInternal(DevTooltipsWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);
            }
        }
    }
}
