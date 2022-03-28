using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Widgets;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos.Services;
using Appalachia.UI.Functionality.Images.Controls.Raw;
using Appalachia.Utility.Async;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos.Widgets
{
    [CallStaticConstructorInEditor]
    public class RuntimeGizmoDrawerWidget : LifetimeWidget<RuntimeGizmoDrawerWidget,
        RuntimeGizmoDrawerWidgetMetadata, RuntimeGizmoDrawerFeature, RuntimeGizmoDrawerFeatureMetadata>
    {
        static RuntimeGizmoDrawerWidget()
        {
            When.Service<RuntimeGizmoDrawerService>()
                .IsAvailableThen(service => { _runtimeGizmoDrawerService = service; });
        }

        #region Static Fields and Autoproperties

        private static RuntimeGizmoDrawerService _runtimeGizmoDrawerService;

        #endregion

        #region Fields and Autoproperties

        [FormerlySerializedAs("rawImageSet")] public RawImageControl rawImage;

        #endregion


        /// <inheritdoc />
        protected override async AppaTask Initialize(
            Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                RawImageControl.Refresh(ref rawImage, gameObject, nameof(rawImage));
            }
        }

        /// <inheritdoc />
        protected override async AppaTask AfterEnabled()
        {
            await base.AfterEnabled();

            await AppaTask.WaitUntil(() => (rawImage != null) && (_runtimeGizmoDrawerService != null));

            rawImage.RawImage.rawImage.texture = _runtimeGizmoDrawerService.GetRenderTexture();
        }
    }
}
