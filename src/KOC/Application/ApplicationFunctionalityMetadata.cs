using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Async;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application
{
    public abstract class ApplicationFunctionalityMetadata<TFunctionality, TFunctionalityMetadata> :
        SingletonAppalachiaObject<TFunctionalityMetadata>,
        IApplicationFunctionalityMetadata<TFunctionality>
        where TFunctionality : ApplicationFunctionality<TFunctionality, TFunctionalityMetadata>
        where TFunctionalityMetadata :
        ApplicationFunctionalityMetadata<TFunctionality, TFunctionalityMetadata>
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

        #region IApplicationFunctionalityMetadata<TFunctionality> Members

        public abstract void Apply(TFunctionality functionality);

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion
    }
}
