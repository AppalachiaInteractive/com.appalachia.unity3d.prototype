using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets
{
    [CallStaticConstructorInEditor]
    public abstract class AreaWidgetWithSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata, TWidget,
                                                           TWidgetMetadata, TFeature, TFeatureMetadata,
                                                           TAreaManager, TAreaMetadata> :
        ApplicationWidgetWithSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata, TFeature
            , TFeatureMetadata, AreaFeatureFunctionalitySet, IAreaService, IAreaWidget, TAreaManager>
        where TSubwidget : AreaSubwidget<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
            TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TSubwidgetMetadata : AreaSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TWidget : AreaWidgetWithSubwidgets<TSubwidget, TSubwidgetMetadata, TWidget, TWidgetMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>, IAreaWidget
        where TWidgetMetadata : AreaWidgetWithSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeature : AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
