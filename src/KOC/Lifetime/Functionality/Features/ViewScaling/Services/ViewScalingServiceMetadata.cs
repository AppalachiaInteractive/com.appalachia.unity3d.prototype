using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Services;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.ViewScaling.Models;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.ViewScaling.Services
{
    public class ViewScalingServiceMetadata :
        LifetimeServiceMetadata<ViewScalingService, ViewScalingServiceMetadata, ViewScalingFeature,
            ViewScalingFeatureMetadata>,
        Broadcaster.IServiceMetadata<ViewScalingService, ViewScalingServiceMetadata, ViewScalingArgs>
    {
        protected override void UpdateFunctionality(ViewScalingService widget)
        {
            using (_PRF_Apply.Auto())
            {
            }
        }

        protected override void SubscribeResponsiveComponents(ViewScalingService target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }
    }
}
