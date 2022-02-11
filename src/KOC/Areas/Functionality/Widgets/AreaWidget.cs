using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Widgets;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Widgets
{
    [CallStaticConstructorInEditor]
    public abstract class AreaWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
                                     TAreaMetadata> :
        ApplicationWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, AreaFeatureFunctionalitySet,
            IAreaService, IAreaWidget, TAreaManager>,
        IAreaWidget
        where TWidget : AreaWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
            TAreaMetadata>
        where TWidgetMetadata : AreaWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TAreaManager, TAreaMetadata>
        where TFeature : AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
