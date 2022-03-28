using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Controls.Subwidget.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton.Contracts;
using Appalachia.UI.Functionality.Tooltips.Contracts;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts
{
    public interface IStatusBarSubwidget : IAreaSingletonSubwidget<IStatusBarSubwidget, IStatusBarSubwidgetMetadata>,
                                           ITooltipOwner
    {
        IStatusBarSubwidgetControl Control { get; }
    }
}
