using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.UI.Functionality.Images.Controls.Raw;
using Appalachia.Utility.Async;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Widgets
{
    public class RectVisualizerWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<
                                                    RectVisualizerWidget, RectVisualizerWidgetMetadata,
                                                    RectVisualizerFeature, RectVisualizerFeatureMetadata>,
                                                GizmoDrawer.IWidgetMetadata
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("_rawImageSet")]
        [SerializeField]
        private RawImageControlConfig _rawImage;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                RawImageControlConfig.Refresh(ref _rawImage, this);
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(RectVisualizerWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                _rawImage.SubscribeToChanges(OnChanged);
            }
        }
        
        /// <inheritdoc />
        protected override void UnsuspendResponsiveComponents(RectVisualizerWidget target)
        {
            using (_PRF_UnsuspendResponsiveComponents.Auto())
            {
                base.UnsuspendResponsiveComponents(target);

                _rawImage.UnsuspendChanges();
            }
        }
        
        /// <inheritdoc />
        protected override void SuspendResponsiveComponents(RectVisualizerWidget target)
        {
            using (_PRF_SuspendResponsiveComponents.Auto())
            {
                base.SuspendResponsiveComponents(target);

                _rawImage.SuspendChanges();
            }
        }

        /// <inheritdoc />
        protected override void OnApply(RectVisualizerWidget widget)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(widget);

                _rawImage.Apply(widget.rawImage);
            }
        }

        #region IWidgetMetadata Members

        public RawImageControlConfig RawImageSet => _rawImage;

        #endregion
    }
}
