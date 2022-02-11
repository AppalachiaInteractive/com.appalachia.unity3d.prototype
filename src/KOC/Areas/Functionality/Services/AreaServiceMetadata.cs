using Appalachia.Prototype.KOC.Application.Features.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Services
{
    public abstract class AreaServiceMetadata<TService, TServiceMetadata, TFeature, TFeatureMetadata,
                                              TAreaManager, TAreaMetadata> : ApplicationServiceMetadata<
        TService, TServiceMetadata, TFeature, TFeatureMetadata, AreaFeatureFunctionalitySet, IAreaService,
        IAreaWidget, TAreaManager>
        where TService : AreaService<TService, TServiceMetadata, TFeature, TFeatureMetadata, TAreaManager,
            TAreaMetadata>
        where TServiceMetadata : AreaServiceMetadata<TService, TServiceMetadata, TFeature, TFeatureMetadata,
            TAreaManager, TAreaMetadata>
        where TFeature : AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
