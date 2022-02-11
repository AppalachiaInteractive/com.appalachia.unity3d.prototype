using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Services;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Widgets;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Features
{
    [CallStaticConstructorInEditor]
    public abstract class LifetimeFeature<TFeature, TFeatureMetadata> :
        ApplicationFeature<TFeature, TFeatureMetadata, LifetimeFeatureFunctionalitySet, ILifetimeService,
            ILifetimeWidget, LifetimeComponentManager>,
        ILifetimeFeature
        where TFeature : LifetimeFeature<TFeature, TFeatureMetadata>
        where TFeatureMetadata : LifetimeFeatureMetadata<TFeature, TFeatureMetadata>

    {
        protected override GameObject GetRootCanvasGameObject()
        {
            using (_PRF_GetRootCanvasGameObject.Auto())
            {
                NullCheck(Manager, WAS_NOT_READY_IN_TIME, nameof(LifetimeComponentManager));

                NullCheck(
                    Manager.RootCanvas,
                    WAS_NOT_READY_IN_TIME,
                    nameof(LifetimeComponentManager),
                    nameof(Manager.RootCanvas)
                );

                NullCheck(
                    Manager.RootCanvas.GameObject,
                    WAS_NOT_READY_IN_TIME,
                    nameof(LifetimeComponentManager),
                    nameof(Manager.RootCanvas),
                    nameof(Manager.RootCanvas.GameObject)
                );

                NullCheck(
                    Manager.RootCanvas.ScaledCanvas,
                    WAS_NOT_READY_IN_TIME,
                    nameof(LifetimeComponentManager),
                    nameof(Manager.RootCanvas),
                    nameof(Manager.RootCanvas.ScaledCanvas)
                );

                NullCheck(
                    Manager.RootCanvas.ScaledCanvas.gameObject,
                    WAS_NOT_READY_IN_TIME,
                    nameof(LifetimeComponentManager),
                    nameof(Manager.RootCanvas),
                    nameof(Manager.RootCanvas.ScaledCanvas),
                    nameof(Manager.RootCanvas.ScaledCanvas.gameObject)
                );

                return Manager.RootCanvas.ScaledCanvas.gameObject;
            }
        }

        protected override GameObject GetTargetParentObject()
        {
            using (_PRF_GetParentObject.Auto())
            {
                NullCheck(Manager, WAS_NOT_READY_IN_TIME, nameof(LifetimeComponentManager));

                NullCheck(
                    Manager.FeaturesObject,
                    WAS_NOT_READY_IN_TIME,
                    nameof(LifetimeComponentManager),
                    nameof(Manager.FeaturesObject)
                );

                return Manager.FeaturesObject;
            }
        }
    }
}
