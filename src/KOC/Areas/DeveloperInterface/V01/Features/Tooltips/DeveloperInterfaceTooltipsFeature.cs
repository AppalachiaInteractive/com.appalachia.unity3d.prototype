using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Tooltips.Widgets;
using Appalachia.Prototype.KOC.Areas.Functionality.Templates.Tooltips;
using Appalachia.Utility.Extensions;
using MANAGER = Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.DeveloperInterfaceManager_V01;
using METADATA = Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.DeveloperInterfaceMetadata_V01;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Tooltips
{
    #region Nested type: TooltipsFeature

    public sealed class DeveloperInterfaceTooltipsFeature : AreaTooltipsFeature<DeveloperInterfaceTooltipsWidget,
        DeveloperInterfaceTooltipsWidgetMetadata, DeveloperInterfaceTooltipsFeature,
        DeveloperInterfaceTooltipsFeatureMetadata, MANAGER, METADATA>
    {
        public override void SortWidgets()
        {
            using (_PRF_SortWidgets.Auto())
            {
                base.SortWidgets();
            }
        }
    }

    #endregion

    #region Nested type: TooltipsWidget

    #endregion
}
