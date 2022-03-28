using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Instanced;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Features;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Subwidgets.Instanced.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Subwidgets.Instanced
{
    [CallStaticConstructorInEditor]
    public abstract class
        LifetimeInstancedSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
                                   TWidgetMetadata, TFeature, TFeatureMetadata> :
            ApplicationInstancedSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
                TWidgetMetadata, TFeature, TFeatureMetadata, LifetimeFeatureFunctionalitySet, ILifetimeService,
                ILifetimeWidget, LifetimeComponentManager>,
            ILifetimeInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
        where TSubwidget :
        LifetimeInstancedSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidget,
        ILifetimeInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
        where TSubwidgetMetadata :
        LifetimeInstancedSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata>, TISubwidgetMetadata,
        ILifetimeInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TISubwidget : class, ILifetimeInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, ILifetimeInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TWidget :
        LifetimeWidgetWithInstancedSubwidgets<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata>, ILifetimeWidget
        where TWidgetMetadata : LifetimeWidgetWithInstancedSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata,
            TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
        where TFeature : LifetimeFeature<TFeature, TFeatureMetadata>
        where TFeatureMetadata : LifetimeFeatureMetadata<TFeature, TFeatureMetadata>
    {
    }
}
