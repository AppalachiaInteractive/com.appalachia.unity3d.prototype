using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Widgets;
using Appalachia.UI.Controls.Sets.RawImage;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.RuntimeGizmos.Widgets
{
    public class RuntimeGizmoDrawerWidgetMetadata : LifetimeWidgetMetadata<RuntimeGizmoDrawerWidget,
        RuntimeGizmoDrawerWidgetMetadata, RuntimeGizmoDrawerFeature, RuntimeGizmoDrawerFeatureMetadata>
    {
        #region Fields and Autoproperties

        public RawImageComponentSetData rawImageSet;

        #endregion

        protected override void SubscribeResponsiveComponents(RuntimeGizmoDrawerWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                rawImageSet.Changed.Event += OnChanged;
            }
        }

        protected override void UpdateFunctionality(RuntimeGizmoDrawerWidget widget)
        {
            using (_PRF_Apply.Auto())
            {
                base.UpdateFunctionality(widget);

                RawImageComponentSetData.UpdateComponentSet(
                    ref rawImageSet,
                    ref widget.rawImageSet,
                    widget.gameObject,
                    nameof(RuntimeGizmoDrawerWidget)
                );
            }
        }
    }
}
