using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Contracts;

namespace Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts
{
    public interface IApplicationSingletonSubwidgetMetadata<in T> : IApplicationSubwidgetMetadata<T>,
                                                                    IApplicationSingletonSubwidgetMetadata
        where T : class, IApplicationSingletonSubwidget
    {
    }

    public interface
        IApplicationSingletonSubwidgetMetadata<in T, TMetadata> : IApplicationSingletonSubwidgetMetadata<T>,
                                                                  IApplicationSubwidgetMetadata<T, TMetadata>
        where T : class, IApplicationSingletonSubwidget<T, TMetadata>
        where TMetadata : class, IApplicationSingletonSubwidgetMetadata<T, TMetadata>
    {
    }
}
