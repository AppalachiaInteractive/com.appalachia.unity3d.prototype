using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton
{
    public interface
        IAreaSingletonSubwidgetMetadata<in TSubwidget, TSubwidgetMetadata> : ISingletonSubwidgetMetadata<
            TSubwidget, TSubwidgetMetadata>
        where TSubwidget : class, IAreaSingletonSubwidget<TSubwidget, TSubwidgetMetadata>
        where TSubwidgetMetadata : class, IAreaSingletonSubwidgetMetadata<TSubwidget, TSubwidgetMetadata>
    {
    }
}
