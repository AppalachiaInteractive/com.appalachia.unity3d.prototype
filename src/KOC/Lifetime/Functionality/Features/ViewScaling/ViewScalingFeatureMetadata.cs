using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Features;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.ViewScaling
{
    public sealed class
        ViewScalingFeatureMetadata : LifetimeFeatureMetadata<ViewScalingFeature, ViewScalingFeatureMetadata>
    {
        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(ViewScalingFeature target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(ViewScalingFeature widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
            }
        }
    }
}
