using System;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Subwidgets.Instanced;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Services;
using Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Instanced.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Subwidgets.Instanced
{
    [CallStaticConstructorInEditor]
    public abstract class AreaWidgetWithInstancedSubwidgets<TSubwidget, TSubwidgetMetadata, TISubwidget,
                                                            TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature,
                                                            TFeatureMetadata, TAreaManager, TAreaMetadata> :
        ApplicationWidgetWithInstancedSubwidgets<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata,
            TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, AreaFeatureFunctionalitySet, IAreaService, IAreaWidget
            , TAreaManager>, IAreaWidget
        where TSubwidget :
        AreaInstancedSubwidget<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>, TISubwidget,
        IAreaInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
        where TSubwidgetMetadata :
        AreaInstancedSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>, TISubwidgetMetadata,
        IAreaInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TISubwidget : class, IAreaInstancedSubwidget<TISubwidget, TISubwidgetMetadata>
        where TISubwidgetMetadata : class, IAreaInstancedSubwidgetMetadata<TISubwidget, TISubwidgetMetadata>
        where TWidget :
        AreaWidgetWithInstancedSubwidgets<TSubwidget, TSubwidgetMetadata, TISubwidget, TISubwidgetMetadata, TWidget,
            TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>, IAreaWidget
        where TWidgetMetadata : AreaWidgetWithInstancedSubwidgetsMetadata<TSubwidget, TSubwidgetMetadata, TISubwidget,
            TISubwidgetMetadata, TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeature : AreaFeature<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TFeatureMetadata : AreaFeatureMetadata<TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        public void ForEachSubwidget(Action<IAreaSubwidget> forEachAction)
        {
            foreach (var subwidget in _subwidgets)
            {
                forEachAction(subwidget);
            }
        }
    }
}
