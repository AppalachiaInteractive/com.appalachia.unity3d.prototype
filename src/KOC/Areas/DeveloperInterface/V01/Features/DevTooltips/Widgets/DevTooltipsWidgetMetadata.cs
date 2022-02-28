using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Services;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Widgets
{
    public sealed class DevTooltipsWidgetMetadata : DeveloperInterfaceMetadata_V01.
        WidgetWithControlledSubwidgetsMetadata<DevTooltipSubwidget, DevTooltipsWidget,
            DevTooltipsWidgetMetadata, DevTooltipsFeature, DevTooltipsFeatureMetadata>
    {
        #region Static Fields and Autoproperties

        private static RectVisualizerService _rectVisualizerService;

        #endregion

        protected override bool ShowAnimationDurationField => false;
        protected override bool ShowBackgroundField => false;

        protected override bool ShowCanvasField => false;
        protected override bool ShowFeatureDisabledVisibilityModeField => false;
        protected override bool ShowFeatureEnabledVisibilityModeField => false;
        protected override bool ShowFontStyleField => true;
        protected override bool ShowRoundedBackgroundField => false;
        protected override bool ShowTransitionsWithFadeField => false;


        protected override void UpdateFunctionalityInternal(DevTooltipsWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);
            }
        }
    }
}
