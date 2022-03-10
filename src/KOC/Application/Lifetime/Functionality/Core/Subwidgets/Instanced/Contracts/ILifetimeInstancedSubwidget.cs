using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Instanced.Contracts;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Subwidgets.Instanced.Contracts
{
    public interface
        ILifetimeInstancedSubwidget<TISubwidget, TISubwidgetMetadata> : IApplicationInstancedSubwidget<TISubwidget,
            TISubwidgetMetadata>
        where TISubwidget : class, ILifetimeInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, ILifetimeInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
    {
    }
}
