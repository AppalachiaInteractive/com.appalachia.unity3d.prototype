using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Features;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors
{
    public class CursorFeatureMetadata : LifetimeFeatureMetadata<CursorFeature, CursorFeatureMetadata>
    {
        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(CursorFeature target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(CursorFeature functionality)
        {
            using (_PRF_UpdateFunctionality.Auto())
            {
            }
        }
    }
}
