using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos.Widgets
{
    public class RuntimeGizmoDrawerWidgetMetadata : LifetimeWidgetMetadata<RuntimeGizmoDrawerWidget,
        RuntimeGizmoDrawerWidgetMetadata, RuntimeGizmoDrawerFeature, RuntimeGizmoDrawerFeatureMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField] public Appalachia.UI.Controls.Sets2.Images.RawImage.RawImageComponentSetData rawImageSet;

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

                Appalachia.UI.Controls.Sets2.Images.RawImage.RawImageComponentSetData.RefreshAndUpdate(
                    ref rawImageSet,
                    ref widget.rawImageSet,
                    widget.gameObject,
                    nameof(RuntimeGizmoDrawerWidget)
                );
            }
        }
    }
}
