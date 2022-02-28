using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Services
{
    [CallStaticConstructorInEditor]
    public abstract class AreaService<TService, TServiceMetadata, TFeature, TFeatureMetadata, TAreaManager,
                                      TAreaMetadata> :
        ApplicationService<TService, TServiceMetadata, TFeature, TFeatureMetadata, AreaFeatureFunctionalitySet
            , IAreaService, IAreaWidget, TAreaManager>,
        IAreaService
        where TService : AreaService<TService, TServiceMetadata, TFeature, TFeatureMetadata, TAreaManager,
            TAreaMetadata>, IAreaService
        where TServiceMetadata : AreaServiceMetadata<TService, TServiceMetadata, TFeature, TFeatureMetadata,
            TAreaManager, TAreaMetadata>
        where TFeature : AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
