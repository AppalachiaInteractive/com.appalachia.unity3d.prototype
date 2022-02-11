using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Features;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.RuntimeGizmos
{
    public class RuntimeGizmoDrawerFeatureMetadata : LifetimeFeatureMetadata<RuntimeGizmoDrawerFeature,
        RuntimeGizmoDrawerFeatureMetadata>
    {
        protected override void UpdateFunctionality(RuntimeGizmoDrawerFeature widget)
        {
            using (_PRF_UpdateFunctionality.Auto())
            {
            }
        }

        protected override void SubscribeResponsiveComponents(RuntimeGizmoDrawerFeature target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }
    }
}
