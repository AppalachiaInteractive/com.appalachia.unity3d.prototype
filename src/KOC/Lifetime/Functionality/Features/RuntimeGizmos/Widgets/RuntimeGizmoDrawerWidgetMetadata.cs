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

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(RuntimeGizmoDrawerWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                rawImageSet.Changed.Event += OnChanged;
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(RuntimeGizmoDrawerWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);

                RawImageComponentSetData.RefreshAndUpdateComponentSet(
                    ref rawImageSet,
                    ref widget.rawImageSet,
                    widget.gameObject,
                    nameof(RuntimeGizmoDrawerWidget)
                );
            }
        }
    }
}
