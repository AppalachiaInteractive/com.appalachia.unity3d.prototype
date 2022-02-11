using Appalachia.Core.Objects.Availability;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar.Model
{
    public interface IMenuBarEntryProvider : IAvailabilityMarker
    {
        MenuBarEntry[] GetMenuBarEntries();
    }
}
