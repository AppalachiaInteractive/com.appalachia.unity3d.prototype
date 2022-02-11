using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Features;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.ViewScaling
{
    public sealed class
        ViewScalingFeatureMetadata : LifetimeFeatureMetadata<ViewScalingFeature, ViewScalingFeatureMetadata>
    {
        protected override void UpdateFunctionality(ViewScalingFeature widget)
        {
            using (_PRF_Apply.Auto())
            {
            }
        }

        protected override void SubscribeResponsiveComponents(ViewScalingFeature target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }
    }
}
