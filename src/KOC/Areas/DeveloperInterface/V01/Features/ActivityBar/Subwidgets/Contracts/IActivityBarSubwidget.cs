using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Controls.Subwidget.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton.Contracts;
using Appalachia.UI.Functionality.Tooltips.Contracts;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Contracts
{
    public interface IActivityBarSubwidget :
        IAreaSingletonSubwidget<IActivityBarSubwidget, IActivityBarSubwidgetMetadata>,
        IActivable,
        ITooltipOwner
    {
        IActivityBarSubwidgetControl Control { get; }
    }
}
