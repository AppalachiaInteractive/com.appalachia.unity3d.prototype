using Appalachia.Core.Objects.Initialization;
using Appalachia.UI.Core.Objects;
using Appalachia.Utility.Async;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.Common
{
    public abstract class AreaFunctionalityMetadata<TFunctionality, TFunctionalityMetadata, TAreaManager,
                                                    TAreaMetadata> : ResponsiveSingletonAppalachiaObject<
        TFunctionalityMetadata>
        where TFunctionality : AreaFunctionality<TFunctionality, TFunctionalityMetadata, TAreaManager,
            TAreaMetadata>
        where TFunctionalityMetadata : AreaFunctionalityMetadata<TFunctionality, TFunctionalityMetadata,
            TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        public virtual void Apply(TFunctionality functionality)
        {
            using (_PRF_Apply.Auto())
            {
            }
        }

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

        #region Profiling

        protected static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion
    }
}
