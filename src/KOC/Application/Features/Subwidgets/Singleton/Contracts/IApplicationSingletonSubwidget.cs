using Appalachia.Core.Objects.Availability;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Contracts;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts
{
    public interface IApplicationSingletonSubwidget : IApplicationSubwidget, IAvailabilityMarker
    {
    }
}