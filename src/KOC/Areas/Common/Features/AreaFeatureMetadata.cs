using Appalachia.Prototype.KOC.Application.Features;

namespace Appalachia.Prototype.KOC.Areas.Common.Features
{
    public abstract class
        AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata> :
            ApplicationFeatureMetadata<TFeature, TFeatureMetadata>
        where TFeature : AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
