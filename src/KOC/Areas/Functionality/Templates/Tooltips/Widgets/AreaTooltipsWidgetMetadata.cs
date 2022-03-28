using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.Functionality.Widgets;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Templates.Tooltips.Widgets
{
    [CallStaticConstructorInEditor]
    public abstract class TooltipsWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
                                                 TAreaMetadata> : AreaWidgetMetadata<TWidget, TWidgetMetadata, TFeature,
        TFeatureMetadata, TAreaManager, TAreaMetadata>
        where TWidget : TooltipsWidget<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
            TAreaMetadata>
        where TWidgetMetadata : TooltipsWidgetMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TAreaManager, TAreaMetadata>
        where TFeature : TooltipsFeature<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata, TAreaManager,
            TAreaMetadata>
        where TFeatureMetadata : TooltipsFeatureMetadata<TWidget, TWidgetMetadata, TFeature, TFeatureMetadata,
            TAreaManager, TAreaMetadata>
        where TAreaManager : AreaManager<TAreaManager, TAreaMetadata>
        where TAreaMetadata : AreaMetadata<TAreaManager, TAreaMetadata>
    {
        static TooltipsWidgetMetadata()
        {
        }

        
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
