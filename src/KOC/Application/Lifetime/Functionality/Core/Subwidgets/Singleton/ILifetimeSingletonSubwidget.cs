using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Subwidgets.Singleton
{
    public interface ILifetimeSingletonSubwidget<TISubwidget, out TISubwidgetMetadata> : ISingletonSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidget : class, ILifetimeSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, ILifetimeSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
    {
    }
}
