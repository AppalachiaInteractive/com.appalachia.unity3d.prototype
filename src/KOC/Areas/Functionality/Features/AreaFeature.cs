using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Features
{
    [CallStaticConstructorInEditor]
    public abstract class AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata> :
        ApplicationFeature<TFeature, TFeatureMetadata, AreaFeatureFunctionalitySet, IAreaService, IAreaWidget,
            TAreaManager>,
        IAreaFeature
        where TFeature : AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
