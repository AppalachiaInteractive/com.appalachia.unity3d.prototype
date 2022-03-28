using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.Functionality.Templates.Tooltips.Widgets;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Tooltips.Widgets
{
    [CallStaticConstructorInEditor]
    public sealed class DeveloperInterfaceTooltipsWidget : AreaTooltipsWidget<DeveloperInterfaceTooltipsWidget,
        DeveloperInterfaceTooltipsWidgetMetadata, DeveloperInterfaceTooltipsFeature,
        DeveloperInterfaceTooltipsFeatureMetadata, DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        protected override GameObject GetWidgetParentObject()
        {
            using (_PRF_GetWidgetParentObject.Auto())
            {
                return Manager.GetWidgetParentObject(metadata.inUnscaledView);
            }
        }
    }
}