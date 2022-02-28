using Appalachia.Prototype.KOC.Application.Features.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Features;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services
{
    public abstract class LifetimeServiceMetadata<TService, TServiceMetadata, TFeature, TFeatureMetadata> :
        ApplicationServiceMetadata<TService, TServiceMetadata, TFeature, TFeatureMetadata,
            LifetimeFeatureFunctionalitySet, ILifetimeService, ILifetimeWidget, LifetimeComponentManager>
        where TService : LifetimeService<TService, TServiceMetadata, TFeature, TFeatureMetadata>
        where TServiceMetadata :
        LifetimeServiceMetadata<TService, TServiceMetadata, TFeature, TFeatureMetadata>
        where TFeature : LifetimeFeature<TFeature, TFeatureMetadata>
        where TFeatureMetadata : LifetimeFeatureMetadata<TFeature, TFeatureMetadata>

    {
    }
}
