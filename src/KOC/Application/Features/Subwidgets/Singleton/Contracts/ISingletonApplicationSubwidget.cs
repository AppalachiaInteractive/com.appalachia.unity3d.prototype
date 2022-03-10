using Appalachia.Core.Objects.Availability;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Contracts;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts
{
    public interface IApplicationSingletonSubwidget : IApplicationSubwidget, IAvailabilityMarker
    {
    }

    public interface IApplicationSingletonSubwidget<T> : IApplicationSingletonSubwidget, IApplicationSubwidget<T>
        where T : class, IApplicationSingletonSubwidget<T>
    {
    }

    public interface IApplicationSingletonSubwidget<T, out TMetadata> : IApplicationSingletonSubwidget<T>,
                                                                        IApplicationSubwidget<T, TMetadata>
        where T : class, IApplicationSingletonSubwidget<T, TMetadata>
        where TMetadata : class, IApplicationSingletonSubwidgetMetadata<T>
    {
    }
}
