using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Features
{
    [CallStaticConstructorInEditor]
    public abstract class AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata> :
        ApplicationFeature<TFeature, TFeatureMetadata, AreaFeatureFunctionalitySet, IAreaService, IAreaWidget,
            TAreaManager>,
        IAreaFeature
        where TFeature : AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        protected override GameObject GetRootCanvasGameObject()
        {
            NullCheck(Manager, WAS_NOT_READY_IN_TIME, typeof(TAreaManager).Name);

            NullCheck(
                Manager.RootCanvas,
                WAS_NOT_READY_IN_TIME,
                typeof(TAreaManager).Name,
                nameof(Manager.RootCanvas)
            );

            NullCheck(
                Manager.RootCanvas.GameObject,
                WAS_NOT_READY_IN_TIME,
                typeof(TAreaManager).Name,
                nameof(Manager.RootCanvas),
                nameof(Manager.RootCanvas.GameObject)
            );

            NullCheck(
                Manager.RootCanvas.ScaledCanvas,
                WAS_NOT_READY_IN_TIME,
                typeof(TAreaManager).Name,
                nameof(Manager.RootCanvas),
                nameof(Manager.RootCanvas.ScaledCanvas)
            );

            NullCheck(
                Manager.RootCanvas.ScaledCanvas.gameObject,
                WAS_NOT_READY_IN_TIME,
                typeof(TAreaManager).Name,
                nameof(Manager.RootCanvas),
                nameof(Manager.RootCanvas.ScaledCanvas),
                nameof(Manager.RootCanvas.ScaledCanvas.gameObject)
            );

            return Manager.RootCanvas.ScaledCanvas.gameObject;
        }

        protected override GameObject GetTargetParentObject()
        {
            NullCheck(Manager, WAS_NOT_READY_IN_TIME, typeof(TAreaManager).Name);

            NullCheck(
                Manager.FeaturesObject,
                WAS_NOT_READY_IN_TIME,
                typeof(TAreaManager).Name,
                nameof(Manager.FeaturesObject)
            );

            return Manager.FeaturesObject;
        }
    }
}
