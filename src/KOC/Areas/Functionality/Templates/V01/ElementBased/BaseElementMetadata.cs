using Appalachia.Core.Objects.Root;
using Appalachia.Core.Objects.Root.Contracts;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Templates.V01.ElementBased
{
    public abstract class BaseElementMetadata<TWidget, TWidgetMetadata, TElement,  TElementMetadata,
                                              TFeature, TFeatureMetadata, TAreaManager,
                                              TAreaMetadata> : AppalachiaObject<TElementMetadata>
        where TWidget : ElementBasedWidget<TWidget, TWidgetMetadata, TElement,  TElementMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TWidgetMetadata : ElementBasedWidgetMetadata<TWidget, TWidgetMetadata, TElement, 
            TElementMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TElement : BaseElement<TWidget, TWidgetMetadata, TElement,  TElementMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>,  IEnableNotifier
        where TElementMetadata : BaseElementMetadata<TWidget, TWidgetMetadata, TElement, 
            TElementMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeature : ElementBasedFeature<TWidget, TWidgetMetadata, TElement,  TElementMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : ElementBasedFeatureMetadata<TWidget, TWidgetMetadata, TElement, 
            TElementMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
