using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Templates.Tooltips.Widgets;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Templates.Tooltips
{
    public abstract class AreaTooltipsFeatureMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
                                                  TAreaMetadata> : AreaFeatureMetadata<TFeature, TFeatureMetadata,
        TAreaManager, TAreaMetadata>
                                       where TWidget : AreaTooltipsWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
            TAreaMetadata>
        where TWidgetMetadata : AreaTooltipsWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TAreaManager, TAreaMetadata>
        where TFeature : AreaTooltipsFeature<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
            TAreaMetadata>
        where TFeatureMetadata : AreaTooltipsFeatureMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
