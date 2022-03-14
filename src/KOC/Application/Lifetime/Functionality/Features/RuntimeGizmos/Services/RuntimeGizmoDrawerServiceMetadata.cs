using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services;
using Appalachia.UI.Functionality.Rendering.Cameras.Components;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos.Services
{
    public class RuntimeGizmoDrawerServiceMetadata : LifetimeServiceMetadata<RuntimeGizmoDrawerService,
                                                         RuntimeGizmoDrawerServiceMetadata,
                                                         RuntimeGizmoDrawerFeature,
                                                         RuntimeGizmoDrawerFeatureMetadata>,
                                                     GizmoDrawer.IServiceMetadata
    {
        #region Fields and Autoproperties

        public CameraConfig cameraData;

        #endregion

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(RuntimeGizmoDrawerService target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                cameraData.Changed.Event += OnChanged;
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(RuntimeGizmoDrawerService functionality)
        {
            var drawCamera = functionality.DrawCamera;
            CameraConfig.RefreshAndApply(ref cameraData, this, drawCamera);
        }

        #region IServiceMetadata Members

        public CameraConfig CameraConfig => cameraData;

        #endregion
    }
}
