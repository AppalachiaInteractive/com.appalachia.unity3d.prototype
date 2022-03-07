using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Controlled;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Features;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Subwidgets.Controlled
{
    [CallStaticConstructorInEditor]
    public abstract class
        LifetimeWidgetWithControlledSubwidgetsMetadata<TSubwidget, TWidget, TWidgetMetadata, TFeature,
                                                       TFeatureMetadata> :
            ApplicationWidgetWithControlledSubwidgetsMetadata<TSubwidget, TWidget, TWidgetMetadata, TFeature,
                TFeatureMetadata, LifetimeFeatureFunctionalitySet, ILifetimeService, ILifetimeWidget,
                LifetimeComponentManager>
        where TSubwidget : LifetimeControlledSubwidget<TSubwidget, TWidget, TWidgetMetadata, TFeature,
            TFeatureMetadata>
        where TWidget :
        LifetimeWidgetWithControlledSubwidgets<TSubwidget, TWidget, TWidgetMetadata, TFeature,
            TFeatureMetadata>, ILifetimeWidget
        where TWidgetMetadata : LifetimeWidgetWithControlledSubwidgetsMetadata<TSubwidget, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata>
        where TFeature : LifetimeFeature<TFeature, TFeatureMetadata>
        where TFeatureMetadata : LifetimeFeatureMetadata<TFeature, TFeatureMetadata>
    {
    }
}