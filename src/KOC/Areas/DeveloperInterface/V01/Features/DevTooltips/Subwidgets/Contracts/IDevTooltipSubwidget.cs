using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Instanced.Contracts;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets.Contracts
{
    public interface IDevTooltipSubwidget : IAreaInstancedSubwidget<IDevTooltipSubwidget, IDevTooltipSubwidgetMetadata>,
                                            IActivable
    {
    }
}
