using Appalachia.Prototype.KOC.Application.Features;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Features
{
    public abstract class LifetimeFeatureMetadata<TFeature, TFeatureMetadata> : ApplicationFeatureMetadata<
        TFeature, TFeatureMetadata, LifetimeFeatureFunctionalitySet, ILifetimeService, ILifetimeWidget,
        LifetimeComponentManager>
        where TFeature : LifetimeFeature<TFeature, TFeatureMetadata>
        where TFeatureMetadata : LifetimeFeatureMetadata<TFeature, TFeatureMetadata>

    {
    }
}
