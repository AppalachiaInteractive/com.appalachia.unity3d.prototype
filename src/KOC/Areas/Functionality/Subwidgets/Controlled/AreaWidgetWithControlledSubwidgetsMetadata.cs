using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Controlled;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Controlled
{
    [CallStaticConstructorInEditor]
    public abstract class AreaWidgetWithControlledSubwidgetsMetadata<TSubwidget, TWidget,
                                                                TWidgetMetadata, TFeature, TFeatureMetadata,
                                                                TAreaManager, TAreaMetadata> :
        ApplicationWidgetWithControlledSubwidgetsMetadata<TSubwidget, TWidget, TWidgetMetadata,
            TFeature, TFeatureMetadata, AreaFeatureFunctionalitySet, IAreaService, IAreaWidget, TAreaManager>
        where TSubwidget : AreaControlledSubwidget<TSubwidget, TWidget, TWidgetMetadata, TFeature,
            TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TWidget : AreaWidgetWithControlledSubwidgets<TSubwidget, TWidget, TWidgetMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>, IAreaWidget
        where TWidgetMetadata : AreaWidgetWithControlledSubwidgetsMetadata<TSubwidget, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeature : AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
