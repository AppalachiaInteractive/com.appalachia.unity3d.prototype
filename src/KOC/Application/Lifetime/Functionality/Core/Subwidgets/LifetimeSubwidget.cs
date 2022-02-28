using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Features;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Subwidgets
{
    [CallStaticConstructorInEditor]
    public abstract class
        LifetimeSubwidget<TElement, TElementMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata> :
            ApplicationSubwidget<TElement, TElementMetadata, TWidget, TWidgetMetadata, TFeature,
                TFeatureMetadata, LifetimeFeatureFunctionalitySet, ILifetimeService, ILifetimeWidget,
                LifetimeComponentManager>
        where TElement : LifetimeSubwidget<TElement, TElementMetadata, TWidget, TWidgetMetadata, TFeature,
            TFeatureMetadata>
        where TElementMetadata : LifetimeSubwidgetMetadata<TElement, TElementMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata>
        where TWidget : LifetimeWidgetWithSubwidgets<TElement, TElementMetadata, TWidget, TWidgetMetadata,
            TFeature, TFeatureMetadata>, ILifetimeWidget
        where TWidgetMetadata : LifetimeWidgetWithSubwidgetsMetadata<TElement, TElementMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata>
        where TFeature : LifetimeFeature<TFeature, TFeatureMetadata>
        where TFeatureMetadata : LifetimeFeatureMetadata<TFeature, TFeatureMetadata>

    {
    }
}
