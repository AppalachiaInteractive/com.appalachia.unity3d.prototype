using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root.Contracts;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Entries.Contracts
{
    public interface IStatusBarEntry : IAvailabilityMarker, IBehaviour
    {
        IStatusBarEntryMetadata Metadata { get; }
        void OnClicked();

        void UpdateStatusBarEntry();
    }
}
