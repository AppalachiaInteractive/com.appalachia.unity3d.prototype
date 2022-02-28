using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Features;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos
{
    public class RuntimeGizmoDrawerFeatureMetadata : LifetimeFeatureMetadata<RuntimeGizmoDrawerFeature,
        RuntimeGizmoDrawerFeatureMetadata>
    {
        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(RuntimeGizmoDrawerFeature target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(RuntimeGizmoDrawerFeature widget)
        {
            using (_PRF_UpdateFunctionality.Auto())
            {
            }
        }
    }
}
