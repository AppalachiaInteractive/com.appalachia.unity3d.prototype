using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Templates.V01.ElementBased
{
    public abstract class ElementBasedWidgetMetadata<TWidget, TWidgetMetadata, TElement, TElementMetadata,
                                                     TFeature, TFeatureMetadata, TAreaManager,
                                                     TAreaMetadata> : AreaWidgetMetadata<TWidget,
        TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TWidget : ElementBasedWidget<TWidget, TWidgetMetadata, TElement, TElementMetadata, TFeature,
            TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TWidgetMetadata : ElementBasedWidgetMetadata<TWidget, TWidgetMetadata, TElement,
            TElementMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TElement : BaseElement<TWidget, TWidgetMetadata, TElement, TElementMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>, IEnableNotifier
        where TElementMetadata : BaseElementMetadata<TWidget, TWidgetMetadata, TElement, TElementMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeature : ElementBasedFeature<TWidget, TWidgetMetadata, TElement, TElementMetadata, TFeature,
            TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : ElementBasedFeatureMetadata<TWidget, TWidgetMetadata, TElement,
            TElementMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
