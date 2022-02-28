using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Features;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services
{
    [CallStaticConstructorInEditor]
    public abstract class LifetimeService<TService, TServiceMetadata, TFeature, TFeatureMetadata> :
        ApplicationService<TService, TServiceMetadata, TFeature, TFeatureMetadata,
            LifetimeFeatureFunctionalitySet, ILifetimeService, ILifetimeWidget, LifetimeComponentManager>,
        ILifetimeService
        where TService : LifetimeService<TService, TServiceMetadata, TFeature, TFeatureMetadata>, ILifetimeService
        where TServiceMetadata :
        LifetimeServiceMetadata<TService, TServiceMetadata, TFeature, TFeatureMetadata>
        where TFeature : LifetimeFeature<TFeature, TFeatureMetadata>
        where TFeatureMetadata : LifetimeFeatureMetadata<TFeature, TFeatureMetadata>

    {
    }
}
