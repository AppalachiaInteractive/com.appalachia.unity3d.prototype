using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Subwidgets.Singleton.Contracts
{
    public interface
        ILifetimeSingletonSubwidget<TISubwidget, TISubwidgetMetadata> : IApplicationSingletonSubwidget<TISubwidget,
            TISubwidgetMetadata>
        where TISubwidget : class, ILifetimeSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, ILifetimeSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
    {
    }
}
