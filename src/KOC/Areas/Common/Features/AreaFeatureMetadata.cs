namespace Appalachia.Prototype.KOC.Areas.Common.Features
{
    public abstract class
        AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata> :
            AreaFunctionalityMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeature : AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        public override void Apply(TFeature functionality)
        {
            using (_PRF_Apply.Auto())
            {
                base.Apply(functionality);
            }
        }
    }
}
