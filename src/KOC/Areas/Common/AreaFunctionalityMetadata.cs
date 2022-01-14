using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Core.Objects;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.Common
{
    public abstract class AreaFunctionalityMetadata<TFunctionality, TFunctionalityMetadata, TAreaManager,
                                                    TAreaMetadata> : UIResponsiveSingletonAppalachiaObject<
        TFunctionalityMetadata>
        where TFunctionality : AreaFunctionality<TFunctionality, TFunctionalityMetadata, TAreaManager,
            TAreaMetadata>
        where TFunctionalityMetadata : AreaFunctionalityMetadata<TFunctionality, TFunctionalityMetadata,
            TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
#if UNITY_EDITOR
                ValidateAddressableInformation();
#endif
            }
        }
    }
}
