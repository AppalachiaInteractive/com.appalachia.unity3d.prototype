using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Styling;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Instanced.Contracts;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets.Contracts
{
    public interface
        IDevTooltipSubwidgetMetadata : IAreaInstancedSubwidgetMetadata<IDevTooltipSubwidget,
            IDevTooltipSubwidgetMetadata>
    {
        DevTooltipStyleOverride StyleOverride {get;}

        DevTooltipControlConfig ControlConfig {get;}
    }
}
