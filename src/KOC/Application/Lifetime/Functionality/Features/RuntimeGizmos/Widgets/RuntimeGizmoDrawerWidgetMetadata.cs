using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;
using Appalachia.UI.Functionality.Images.Controls.Raw;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos.Widgets
{
    public class RuntimeGizmoDrawerWidgetMetadata : LifetimeWidgetMetadata<RuntimeGizmoDrawerWidget,
        RuntimeGizmoDrawerWidgetMetadata, RuntimeGizmoDrawerFeature, RuntimeGizmoDrawerFeatureMetadata>
    {
        #region Fields and Autoproperties

        [SerializeField] public RawImageControlConfig rawImage;

        #endregion

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(RuntimeGizmoDrawerWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                rawImage.SubscribeToChanges(OnChanged);
            }
        }

        
        /// <inheritdoc />
        protected override void UnsuspendResponsiveComponents(RuntimeGizmoDrawerWidget target)
        {
            using (_PRF_UnsuspendResponsiveComponents.Auto())
            {
                base.UnsuspendResponsiveComponents(target);

                rawImage.UnsuspendChanges();
            }
        }

        /// <inheritdoc />
        protected override void SuspendResponsiveComponents(RuntimeGizmoDrawerWidget target)
        {
            using (_PRF_SuspendResponsiveComponents.Auto())
            {
                base.SuspendResponsiveComponents(target);

                rawImage.SuspendChanges();
            }
        }

        /// <inheritdoc />
        protected override void OnApply(RuntimeGizmoDrawerWidget widget)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(widget);

                rawImage.Apply(widget.rawImage);
            }
        }
    }
}
