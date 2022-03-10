using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Instanced.Contracts;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Instanced.Contracts
{
    public interface
        IAreaInstancedSubwidgetMetadata<in TISubwidget, TISubwidgetMetadata> : IApplicationInstancedSubwidgetMetadata<
            TISubwidget, TISubwidgetMetadata>
        where TISubwidget : class, IAreaInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, IAreaInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
    {
    }
}
