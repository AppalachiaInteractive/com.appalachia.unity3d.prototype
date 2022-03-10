using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Singleton;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Singleton
{
    [CallStaticConstructorInEditor]
    public abstract class AreaWidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata, TWidget,
                                                                    TWidgetMetadata, TFeature, TFeatureMetadata,
                                                                    TAreaManager, TAreaMetadata> :
        ApplicationWidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata,
            TFeature, TFeatureMetadata, AreaFeatureFunctionalitySet, IAreaService, IAreaWidget, TAreaManager>
        where TISubwidget : class, IAreaSingletonSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, IAreaSingletonSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TWidget : AreaWidgetWithSingletonSubwidgets<TISubwidget, TISubwidgetMetadata, TWidget, TWidgetMetadata,
            TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>, IAreaWidget
        where TWidgetMetadata : AreaWidgetWithSingletonSubwidgetsMetadata<TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeature : AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
    }
}
