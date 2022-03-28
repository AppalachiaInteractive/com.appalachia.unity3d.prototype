using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Tooltips.Widgets
{
    public sealed class TooltipsWidgetMetadata : LifetimeWidgetMetadata<TooltipsWidget, TooltipsWidgetMetadata,
        TooltipsFeature, TooltipsFeatureMetadata>
    {
        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
            }
        }

        protected override void OnApply(TooltipsWidget widget)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(widget);
            }
        }
    }
}
