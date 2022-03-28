using System;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Tooltips.Widgets;
using Appalachia.Prototype.KOC.Areas.Functionality.Templates.Tooltips;
using MANAGER = Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.DeveloperInterfaceManager_V01;
using METADATA = Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.DeveloperInterfaceMetadata_V01;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Tooltips
{
    [Serializable]
    public sealed class DeveloperInterfaceTooltipsFeatureMetadata : AreaTooltipsFeatureMetadata<
        DeveloperInterfaceTooltipsWidget, DeveloperInterfaceTooltipsWidgetMetadata, DeveloperInterfaceTooltipsFeature,
        DeveloperInterfaceTooltipsFeatureMetadata, MANAGER, METADATA>
    {
    }
}
