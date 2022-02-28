using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Features
{
    [CallStaticConstructorInEditor]
    public abstract class LifetimeFeature<TFeature, TFeatureMetadata> :
        ApplicationFeature<TFeature, TFeatureMetadata, LifetimeFeatureFunctionalitySet, ILifetimeService,
            ILifetimeWidget, LifetimeComponentManager>,
        ILifetimeFeature
        where TFeature : LifetimeFeature<TFeature, TFeatureMetadata>
        where TFeatureMetadata : LifetimeFeatureMetadata<TFeature, TFeatureMetadata>

    {
    }
}
