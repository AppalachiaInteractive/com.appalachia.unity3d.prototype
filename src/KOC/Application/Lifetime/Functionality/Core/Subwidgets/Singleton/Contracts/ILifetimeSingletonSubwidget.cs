using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Subwidgets.Singleton.Contracts
{
    public interface
        ILifetimeSingletonSubwidget<TISubwidget, TISubwidgetMetadata> : IApplicationSingletonSubwidget<TISubwidget,
            TISubwidgetMetadata>, ILifetimeSubwidget
        where TISubwidget : class, ILifetimeSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, ILifetimeSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
    {
    }
}
