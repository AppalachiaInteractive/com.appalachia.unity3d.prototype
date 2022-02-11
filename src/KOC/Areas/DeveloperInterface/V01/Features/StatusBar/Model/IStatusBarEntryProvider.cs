using Appalachia.Core.Objects.Availability;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Model
{
    public interface IStatusBarEntryProvider : IAvailabilityMarker
    {
        StatusBarEntry[] GetStatusBarEntries();
    }
}
