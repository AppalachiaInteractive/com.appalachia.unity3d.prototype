using Appalachia.Core.Objects.Availability;
using Appalachia.Core.Objects.Root;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Templates.V01.SingletonElementBased
{
    public abstract class BaseSingletonElementMetadata<TWidget, TWidgetMetadata, TElement, TIElement, TElementMetadata, TFeature,
                                                TFeatureMetadata, TAreaManager, TAreaMetadata> : SingletonAppalachiaObject<TElementMetadata>
        where TWidget : ElementBasedWidget<TWidget, TWidgetMetadata, TElement, TIElement, TElementMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TWidgetMetadata : ElementBasedWidgetMetadata<TWidget, TWidgetMetadata, TElement, TIElement,
            TElementMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TElement : BaseSingletonElement<TWidget, TWidgetMetadata, TElement, TIElement, TElementMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>, TIElement
        where TIElement : class, IAvailabilityMarker
        where TElementMetadata : BaseSingletonElementMetadata<TWidget, TWidgetMetadata, TElement, TIElement,
            TElementMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeature : ElementBasedFeature<TWidget, TWidgetMetadata, TElement, TIElement, TElementMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : ElementBasedFeatureMetadata<TWidget, TWidgetMetadata, TElement, TIElement,
            TElementMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
