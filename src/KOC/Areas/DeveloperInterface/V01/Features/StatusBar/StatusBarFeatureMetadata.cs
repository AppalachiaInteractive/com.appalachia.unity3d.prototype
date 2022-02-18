namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar
{
    public class StatusBarFeatureMetadata : DeveloperInterfaceMetadata_V01.FeatureMetadata<StatusBarFeature,
        StatusBarFeatureMetadata>
    {
        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(StatusBarFeature target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(StatusBarFeature functionality)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
            }
        }
    }
}
