using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features;
using Appalachia.Utility.Extensions;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.Common.Features
{
    [CallStaticConstructorInEditor]
    public abstract class AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata> :
        ApplicationFeature<TFeature, TFeatureMetadata>,
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
                areaManager = i;

                GameObject containerObject = null;
                areaManager.gameObject.GetOrAddChild(
                    ref containerObject,
                    APPASTR.ObjectNames.Features,
                    false
                );

                instance.gameObject.SetParentTo(containerObject);
            };
        }

        #region Static Fields and Autoproperties

        protected static TAreaManager areaManager;

        #endregion

        protected override bool NestUnderApplicationManager => false;
    }
}
