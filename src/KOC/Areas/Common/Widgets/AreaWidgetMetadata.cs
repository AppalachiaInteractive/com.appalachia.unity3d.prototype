using Appalachia.Prototype.KOC.Application.Widgets;

namespace Appalachia.Prototype.KOC.Areas.Common.Widgets
{
    public abstract class
        AreaWidgetMetadata<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata> : ApplicationWidgetMetadata<
            TWidget, TWidgetMetadata>
        where TWidget : AreaWidget<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata>
        where TWidgetMetadata : AreaWidgetMetadata<TWidget, TWidgetMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
