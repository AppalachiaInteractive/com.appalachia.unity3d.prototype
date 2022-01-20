using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Services
{
    [CallStaticConstructorInEditor]
    public abstract class ApplicationService<TService, TServiceMetadata> :
        ApplicationFunctionality<TService, TServiceMetadata>,
        IApplicationService
        where TService : ApplicationService<TService, TServiceMetadata>
        where TServiceMetadata : ApplicationServiceMetadata<TService, TServiceMetadata>

    {
        #region Constants and Static Readonly

        protected const string GROUP_NAME = "Service";

        #endregion

        protected override bool ParentObjectIsUI => false;
        protected override string ParentObjectName => APPASTR.ObjectNames.Services;

        protected abstract void OnApplyMetadataInternal();

        protected override void ApplyMetadataInternal()
        {
            using (_PRF_ApplyMetadataInternal.Auto())
            {
                OnApplyMetadataInternal();
            }
        }

        #region Profiling

        protected static readonly ProfilerMarker _PRF_OnApplyMetadataInternal =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplyMetadataInternal));

        #endregion
    }
}
