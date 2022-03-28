using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.Functionality.Features;
using Appalachia.Prototype.KOC.Areas.Functionality.Templates.Tooltips.Widgets;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Templates.Tooltips
{
    [CallStaticConstructorInEditor]
    public abstract class AreaTooltipsFeature<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
                                          TAreaMetadata> : AreaFeature<TFeature, TFeatureMetadata, TAreaManager,
        TAreaMetadata>
        where TWidget : AreaTooltipsWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
            TAreaMetadata>
        where TWidgetMetadata : AreaTooltipsWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TAreaManager, TAreaMetadata>
        where TFeature : AreaTooltipsFeature<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
            TAreaMetadata>
        where TFeatureMetadata : AreaTooltipsFeatureMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        static AreaTooltipsFeature()
        {
            FunctionalitySet.RegisterWidget<TWidget>(_dependencyTracker, i => _tooltipsWidget = i);
        }

        #region Static Fields and Autoproperties

        private static TWidget _tooltipsWidget;

        #endregion

        protected static TWidget TooltipsWidget => _tooltipsWidget;
    }
}
