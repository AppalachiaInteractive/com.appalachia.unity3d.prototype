using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root.Contracts;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Entries.Contracts
{
    public interface IActivityBarEntry : IAvailabilityMarker, IBehaviour
    {
        IActivityBarEntryMetadata Metadata { get; }
        void OnClicked();

        void UpdateActivityBarEntry();
    }
}
