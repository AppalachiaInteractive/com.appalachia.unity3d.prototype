using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root.Contracts;
using Appalachia.Core.Objects.Routing;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Utility.Events;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Templates.V01.ElementBased
{
    [CallStaticConstructorInEditor]
    public abstract class ElementBasedFeature<TWidget, TWidgetMetadata, TElement,  TElementMetadata,
                                              TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata> :
        AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
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
        static ElementBasedFeature()
        {
            FunctionalitySet.RegisterWidget<TWidget>(_dependencyTracker, i => _widget = i);

            ObjectEnableEventRouter.SubscribeTo<TElement>(RegisterElement);
        }

        #region Static Fields and Autoproperties

        private static TWidget _widget;

        #endregion

        private static void RegisterElement(ComponentEvent<TElement>.Args args)
        {
            using (_PRF_RegisterStatus.Auto())
            {
                _widget.RegisterElement(args.component);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterStatus =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterElement));

        #endregion
    }
}
