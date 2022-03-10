using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton.Contracts
{
    public interface
        IAreaSingletonSubwidgetMetadata<in TISubwidget, TISubwidgetMetadata> : IApplicationSingletonSubwidgetMetadata<
            TISubwidget, TISubwidgetMetadata>
        where TISubwidget : class, IAreaSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, IAreaSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
    {
    }
}
