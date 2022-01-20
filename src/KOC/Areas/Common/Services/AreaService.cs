using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Services;
using Appalachia.Utility.Extensions;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.Common.Services
{
    [CallStaticConstructorInEditor]
    public abstract class AreaService<TService, TServiceMetadata, TAreaManager, TAreaMetadata> :
        ApplicationService<TService, TServiceMetadata>,
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
                areaManager = i;

                GameObject containerObject = null;

                areaManager.gameObject.GetOrAddChild(
                    ref containerObject,
                    APPASTR.ObjectNames.Services,
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
