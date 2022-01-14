using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Utility.Extensions;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.Common.Services
{
    [CallStaticConstructorInEditor]
    public abstract class AreaService<TService, TServiceMetadata, TAreaManager, TAreaMetadata> :
        AreaFunctionality<TService, TServiceMetadata, TAreaManager, TAreaMetadata>,
        IAreaService
        where TService : AreaService<TService, TServiceMetadata, TAreaManager, TAreaMetadata>
        where TServiceMetadata : AreaServiceMetadata<TService, TServiceMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        static AreaService()
        {
            AreaManager<TAreaManager, TAreaMetadata>.InstanceAvailable += i =>
            {
                GameObject servicesContainerObject = null;
                i.gameObject.GetOrCreateChild(
                    ref servicesContainerObject,
                    APPASTR.ObjectNames.Services,
                    false
                );

                instance.gameObject.SetParentTo(servicesContainerObject);
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
