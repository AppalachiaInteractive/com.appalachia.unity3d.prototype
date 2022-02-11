using Appalachia.Core.Objects.Availability;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Model
{
    public interface IActivityBarEntryProvider : IAvailabilityMarker
    {
        ActivityBarEntry[] GetActivityBarEntries();
    }
}
