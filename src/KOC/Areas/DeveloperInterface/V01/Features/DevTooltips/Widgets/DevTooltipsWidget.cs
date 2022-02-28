using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Widgets
{
    [CallStaticConstructorInEditor]
    public sealed class DevTooltipsWidget : DeveloperInterfaceManager_V01.WidgetWithControlledSubwidgets<
        DevTooltipSubwidget, DevTooltipsWidget,
        DevTooltipsWidgetMetadata, DevTooltipsFeature, DevTooltipsFeatureMetadata>
    {
        public override GameObject GetSubwidgetParent()
        {
            using (_PRF_GetSubwidgetParent.Auto())
            {
                return canvas.GameObject;
            }
        }

        protected override void OnRegisterSubwidget(DevTooltipSubwidget subwidget)
        {
            using (_PRF_OnRegisterSubwidget.Auto())
            {
            }
        }
    }
}
