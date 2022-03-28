using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Templates.Tooltips.Widgets
{
    [CallStaticConstructorInEditor]
    public abstract class AreaTooltipsWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
                                                     TAreaMetadata> : AreaWidgetMetadata<TWidget, TWidgetMetadata,
        TFeature, TFeatureMetadata, TAreaManager, TAreaMetadata>
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
        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
            }
        }

        protected override void OnApply(TWidget widget)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(widget);
            }
        }
    }
}
