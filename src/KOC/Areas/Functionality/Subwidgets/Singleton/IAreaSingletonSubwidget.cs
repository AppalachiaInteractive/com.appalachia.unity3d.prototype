using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton.Contracts;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton
{
    public interface IAreaSingletonSubwidget<TSubwidget, out TSubwidgetMetadata> : ISingletonSubwidget<TSubwidget, TSubwidgetMetadata>
        where TSubwidget : class, IAreaSingletonSubwidget<TSubwidget, TSubwidgetMetadata>
        where TSubwidgetMetadata : class, IAreaSingletonSubwidgetMetadata<TSubwidget, TSubwidgetMetadata>
    {
    }
}
