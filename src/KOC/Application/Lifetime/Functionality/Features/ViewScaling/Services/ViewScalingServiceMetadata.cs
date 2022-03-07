using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.ViewScaling.Models;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.ViewScaling.Services
{
    public class ViewScalingServiceMetadata :
        LifetimeServiceMetadata<ViewScalingService, ViewScalingServiceMetadata, ViewScalingFeature,
            ViewScalingFeatureMetadata>,
        Broadcaster.IServiceMetadata<ViewScalingService, ViewScalingServiceMetadata, ViewScalingArgs>
    {
        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(ViewScalingService target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(ViewScalingService widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
            }
        }
    }
}