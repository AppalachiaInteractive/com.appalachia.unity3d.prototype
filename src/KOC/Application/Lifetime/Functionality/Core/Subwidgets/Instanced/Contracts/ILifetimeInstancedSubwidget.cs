using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Instanced.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Subwidgets.Instanced.Contracts
{
    public interface
        ILifetimeInstancedSubwidget<TISubwidget, TISubwidgetMetadata> :
            IApplicationInstancedSubwidget<TISubwidget, TISubwidgetMetadata>,
            ILifetimeSubwidget
        where TISubwidget : class, ILifetimeInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, ILifetimeInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
    {
    }
}
