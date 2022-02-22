using Appalachia.Core.Objects.Availability;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Templates.V01.SingletonElementBased
{
    public abstract class ElementBasedWidgetMetadata<TWidget, TWidgetMetadata, TElement, TIElement,
                                                     TElementMetadata, TFeature, TFeatureMetadata,
                                                     TAreaManager, TAreaMetadata> : AreaWidgetMetadata<TWidget
        , TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
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
