using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton.Contracts;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts
{
    public interface IStatusBarSubwidget : IAreaSingletonSubwidget<IStatusBarSubwidget, IStatusBarSubwidgetMetadata>,
                                           IDevTooltipSubwidgetController
    {
    }
}
