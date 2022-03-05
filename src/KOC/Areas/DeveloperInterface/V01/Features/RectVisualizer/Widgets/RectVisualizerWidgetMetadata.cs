using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.UI.Controls.Sets2.Images.RawImage;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Widgets
{
    public class RectVisualizerWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<
                                                    RectVisualizerWidget, RectVisualizerWidgetMetadata,
                                                    RectVisualizerFeature, RectVisualizerFeatureMetadata>,
                                                GizmoDrawer.IWidgetMetadata
    {
        #region Fields and Autoproperties

        [SerializeField] public RawImageComponentSetData _rawImageSet;
        [SerializeField] public RawImageComponentSetData _rawImageSet2;

        #endregion

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(RectVisualizerWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                _rawImageSet.Changed.Event += OnChanged;
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(RectVisualizerWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);

                _rawImageSet = _rawImageSet2;
                
                RawImageComponentSetData.RefreshAndUpdate(
                    ref _rawImageSet,
                    ref widget.rawImageSet,
                    widget.canvas.GameObject,
                    nameof(RectVisualizerWidget)
                );
            }
        }

        #region IWidgetMetadata Members

        public RawImageComponentSetData RawImageSet => _rawImageSet;

        #endregion
    }
}
