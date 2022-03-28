using System;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;
using Unity.Profiling;

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
        #region ILifetimeFeature Members

        public void ForEachService(Action<ILifetimeService> forEachAction)
        {
            using (_PRF_ForEachService.Auto())
            {
                foreach (var service in FunctionalitySet.Services)
                {
                    forEachAction(service);
                }
            }
        }

        public void ForEachWidget(Action<ILifetimeWidget> forEachAction)
        {
            using (_PRF_ForEachWidget.Auto())
            {
                foreach (var widget in FunctionalitySet.Widgets)
                {
                    forEachAction(widget);
                }
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_ForEachService =
            new ProfilerMarker(_PRF_PFX + nameof(ForEachService));

        private static readonly ProfilerMarker _PRF_ForEachWidget =
            new ProfilerMarker(_PRF_PFX + nameof(ForEachWidget));

        #endregion
    }
}
