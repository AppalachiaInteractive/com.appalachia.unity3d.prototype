using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Services
{
    public sealed class DeveloperInfoProviderServiceMetadata : DeveloperInterfaceMetadata_V01.ServiceMetadata<
        DeveloperInfoProviderService, DeveloperInfoProviderServiceMetadata, DeveloperInfoFeature,
        DeveloperInfoFeatureMetadata>
    {
        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(DeveloperInfoProviderService target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(DeveloperInfoProviderService functionality)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
            }
        }
    }
}
