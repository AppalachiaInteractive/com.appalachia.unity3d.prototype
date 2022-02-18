namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.GameArea
{
    public class GameAreaFeatureMetadata : DeveloperInterfaceMetadata_V01.FeatureMetadata<GameAreaFeature,
        GameAreaFeatureMetadata>
    {
        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(GameAreaFeature target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(GameAreaFeature functionality)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
            }
        }
    }
}
