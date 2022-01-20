namespace Appalachia.Prototype.KOC.Application.Features
{
    public abstract class
        ApplicationFeatureMetadata<TFeature, TFeatureMetadata> : ApplicationFunctionalityMetadata<TFeature,
            TFeatureMetadata>
        where TFeature : ApplicationFeature<TFeature, TFeatureMetadata>
        where TFeatureMetadata : ApplicationFeatureMetadata<TFeature, TFeatureMetadata>

    {
    }
}
