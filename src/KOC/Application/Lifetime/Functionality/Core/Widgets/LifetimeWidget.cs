using System;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Features;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Subwidgets;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets
{
    [CallStaticConstructorInEditor]
    public abstract class LifetimeWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata> :
        ApplicationWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            LifetimeFeatureFunctionalitySet, ILifetimeService, ILifetimeWidget, LifetimeComponentManager>,
        ILifetimeWidget
        where TWidget : LifetimeWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>,ILifetimeWidget
        where TWidgetMetadata : LifetimeWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
        where TFeature : LifetimeFeature<TFeature, TFeatureMetadata>
        where TFeatureMetadata : LifetimeFeatureMetadata<TFeature, TFeatureMetadata>

    {
        public void ForEachSubwidget(Action<ILifetimeSubwidget> forEachAction)
        {
        }
    }
}
