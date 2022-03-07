using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Contracts
{
    public interface
        ISideBarSubwidgetMetadata : IAreaSingletonSubwidgetMetadata<ISideBarSubwidget, ISideBarSubwidgetMetadata>
    {
        bool Enabled { get; }
    }
}