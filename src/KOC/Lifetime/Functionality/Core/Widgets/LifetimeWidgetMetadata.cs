using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Features;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Services;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Widgets
{
    public abstract class LifetimeWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata> :
        ApplicationWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            LifetimeFeatureFunctionalitySet, ILifetimeService, ILifetimeWidget, LifetimeComponentManager>
        where TWidget : LifetimeWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
        where TWidgetMetadata : LifetimeWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata>
        where TFeature : LifetimeFeature<TFeature, TFeatureMetadata>
        where TFeatureMetadata : LifetimeFeatureMetadata<TFeature, TFeatureMetadata>

    {
    }
}
