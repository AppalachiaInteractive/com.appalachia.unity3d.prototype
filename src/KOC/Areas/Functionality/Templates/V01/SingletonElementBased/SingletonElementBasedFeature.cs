using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Availability;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Templates.V01.SingletonElementBased
{
    [CallStaticConstructorInEditor]
    public abstract class ElementBasedFeature<TWidget, TWidgetMetadata, TElement, TIElement, TElementMetadata,
                                              TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata> :
        AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
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
        static ElementBasedFeature()
        {
            FunctionalitySet.RegisterWidget<TWidget>(_dependencyTracker, i => _widget = i);

            When.Any<TIElement>().IsAvailableThen(RegisterElement);
        }

        #region Static Fields and Autoproperties

        private static TWidget _widget;

        #endregion

        private static void RegisterElement(TIElement status)
        {
            using (_PRF_RegisterStatus.Auto())
            {
                _widget.RegisterElement(status);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterStatus =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterElement));

        #endregion
    }
}
