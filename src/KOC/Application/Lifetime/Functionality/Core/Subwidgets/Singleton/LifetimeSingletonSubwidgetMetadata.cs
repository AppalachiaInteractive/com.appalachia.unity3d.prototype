using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Features;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Subwidgets.Singleton
{
    [CallStaticConstructorInEditor]
    public abstract class LifetimeSingletonSubwidgetMetadata<TSubwidget, TISubwidget, TSubwidgetMetadata,
                                                             TISubwidgetMetadata, TWidget, TWidgetMetadata,
                                                             TFeature, TFeatureMetadata> :
        ApplicationSingletonSubwidgetMetadata<TSubwidget, TISubwidget, TSubwidgetMetadata, TISubwidgetMetadata
            , TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, LifetimeFeatureFunctionalitySet,
            ILifetimeService, ILifetimeWidget, LifetimeComponentManager>
        where TISubwidget : class, ILifetimeSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, ILifetimeSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TSubwidget :
        LifetimeSingletonSubwidget<TSubwidget, TISubwidget, TSubwidgetMetadata, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidget
        where TSubwidgetMetadata : LifetimeSingletonSubwidgetMetadata<TSubwidget, TISubwidget,
            TSubwidgetMetadata, TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>,
        TISubwidgetMetadata
        where TWidget : LifetimeWidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata>, ILifetimeWidget
        where TWidgetMetadata : LifetimeWidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata
            , TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
        where TFeature : LifetimeFeature<TFeature, TFeatureMetadata>
        where TFeatureMetadata : LifetimeFeatureMetadata<TFeature, TFeatureMetadata>
    {
    }
}
