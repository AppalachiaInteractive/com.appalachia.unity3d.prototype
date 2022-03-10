using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton.Contracts;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Contracts
{
    public interface ISideBarSubwidget : IAreaSingletonSubwidget<ISideBarSubwidget, ISideBarSubwidgetMetadata>,
                                         IActivable
    {
    }
}
