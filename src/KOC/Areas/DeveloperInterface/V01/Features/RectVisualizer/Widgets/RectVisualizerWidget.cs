using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Services;
using Appalachia.UI.Functionality.Images.Controls.Raw;
using Appalachia.Utility.Async;
using UnityEngine;
using UnityEngine.Serialization;
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
            When.Service<RectVisualizerService>().IsAvailableThen(service => { _rectVisualizerService = service; });
        }

        #region Static Fields and Autoproperties

        private static RectVisualizerService _rectVisualizerService;

        #endregion

        #region Fields and Autoproperties

        [FormerlySerializedAs("rawImageSet")]
        [SerializeField]
        public RawImageControl rawImage;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask AfterEnabled()
        {
            await base.AfterEnabled();

            using (_PRF_AfterEnabled.Auto())
            {
                VisualUpdate.Event += _rectVisualizerService.DiscoverTargets;

                rawImage.RawImage.RawImage.texture = _rectVisualizerService.GetRenderTexture();
            }
        }

        /// <inheritdoc />
        protected override async AppaTask DelayEnabling()
        {
            await base.DelayEnabling();

            await AppaTask.WaitUntil(() => _rectVisualizerService != null);
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                RawImageControl.Refresh(ref rawImage, canvas.ChildContainer, nameof(RawImage));
            }
        }

        /// <inheritdoc />
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

        public RawImage RawImage => rawImage.RawImage.RawImage;

        #endregion
    }
}
