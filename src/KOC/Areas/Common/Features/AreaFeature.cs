using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Utility.Extensions;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.Common.Features
{
    [CallStaticConstructorInEditor]
    public abstract class AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata> :
        AreaFunctionality<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>,
        IAreaFeature
        where TFeature : AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        static AreaFeature()
        {
            AreaManager<TAreaManager, TAreaMetadata>.InstanceAvailable += i =>
            {
                GameObject featuresContainerObject = null;
                i.gameObject.GetOrCreateChild(
                    ref featuresContainerObject,
                    APPASTR.ObjectNames.Features,
                    false
                );

                instance.gameObject.SetParentTo(featuresContainerObject);
            };
        }

        protected abstract void OnApplyMetadataInternal();

        protected override void ApplyMetadataInternal()
        {
            using (_PRF_ApplyMetadataInternal.Auto())
            {
                OnApplyMetadataInternal();
            }
        }
    }
}
