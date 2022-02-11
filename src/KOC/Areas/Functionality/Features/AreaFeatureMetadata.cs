using Appalachia.Prototype.KOC.Application.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Features
{
    public abstract class AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata> :
        ApplicationFeatureMetadata<TFeature, TFeatureMetadata, AreaFeatureFunctionalitySet, IAreaService,
            IAreaWidget, TAreaManager>
        where TFeature : AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
