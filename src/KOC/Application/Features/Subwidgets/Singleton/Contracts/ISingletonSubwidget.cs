using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root.Contracts;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts
{
    public interface ISingletonSubwidget<TSubwidget, out TSubwidgetMetadata> : IAvailabilityMarker, IBehaviour
        where TSubwidget : class, ISingletonSubwidget<TSubwidget, TSubwidgetMetadata>
        where TSubwidgetMetadata : class, ISingletonSubwidgetMetadata<TSubwidget, TSubwidgetMetadata>
    {
        TSubwidgetMetadata Metadata { get; }
        void UpdateSubwidget();
    }
}
