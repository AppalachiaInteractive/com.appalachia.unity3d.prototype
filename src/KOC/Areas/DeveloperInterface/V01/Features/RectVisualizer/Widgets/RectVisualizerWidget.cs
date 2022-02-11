using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Services;
using Appalachia.UI.Controls.Sets.RawImage;
using Appalachia.Utility.Async;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Widgets
{
    [CallStaticConstructorInEditor]
    public class RectVisualizerWidget : DeveloperInterfaceManager_V01.Widget<RectVisualizerWidget,
                                            RectVisualizerWidgetMetadata, RectVisualizerFeature,
                                            RectVisualizerFeatureMetadata>,
                                        GizmoDrawer.IWidget
    {
        static RectVisualizerWidget()
        {
            When.Service<RectVisualizerService>()
                .IsAvailableThen(service => { _rectVisualizerService = service; });
        }

        #region Static Fields and Autoproperties

        private static RectVisualizerService _rectVisualizerService;

        #endregion

        #region Fields and Autoproperties

        public RawImageComponentSet rawImageSet;

        #endregion

        protected override async AppaTask DelayEnabling()
        {
            await base.DelayEnabling();

            await AppaTask.WaitUntil(() => _rectVisualizerService != null);
        }

        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                VisuallyChanged.Event += _rectVisualizerService.DiscoverTargets;

                rawImageSet.RawImage.texture = _rectVisualizerService.GetRenderTexture();
            }
        }

        protected override void OnUpdate()
        {
            using (_PRF_OnUpdate.Auto())
            {
                RawImage.texture = _rectVisualizerService.GetRenderTexture();

                var rectDatas = _rectVisualizerService.RectDatas;

                for (var rectDataIndex = 0; rectDataIndex < rectDatas.Count; rectDataIndex++)
                {
                    var rectData = rectDatas[rectDataIndex];

                    rectData.Draw(_rectVisualizerService.Draw);
                }
            }
        }

        #region IWidget Members

        public RawImage RawImage => rawImageSet.RawImage;

        #endregion
    }
}
