using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services;
using Appalachia.UI.Functionality.Rendering.Cameras.Components;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos.Services
{
    public class RuntimeGizmoDrawerServiceMetadata : LifetimeServiceMetadata<RuntimeGizmoDrawerService,
                                                         RuntimeGizmoDrawerServiceMetadata, RuntimeGizmoDrawerFeature,
                                                         RuntimeGizmoDrawerFeatureMetadata>,
                                                     GizmoDrawer.IServiceMetadata
    {
        #region Fields and Autoproperties

        public CameraConfig cameraData;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                CameraConfig.Refresh(ref cameraData, this);
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(RuntimeGizmoDrawerService target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);
                
                cameraData.SubscribeToChanges(OnChanged);
            }
        }
        
        /// <inheritdoc />
        protected override void SuspendResponsiveComponents(RuntimeGizmoDrawerService target)
        {
            using (_PRF_SuspendResponsiveComponents.Auto())
            {
                base.SuspendResponsiveComponents(target);
                
                cameraData.SuspendChanges();
            }
        }
        /// <inheritdoc />
        protected override void UnsuspendResponsiveComponents(RuntimeGizmoDrawerService target)
        {
            using (_PRF_UnsuspendResponsiveComponents.Auto())
            {
                base.UnsuspendResponsiveComponents(target);
                
                cameraData.UnsuspendChanges();
            }
        }

        /// <inheritdoc />
        protected override void OnApply(RuntimeGizmoDrawerService functionality)
        {
            var drawCamera = functionality.DrawCamera;
            cameraData.Apply(drawCamera);
        }

        #region IServiceMetadata Members

        public CameraConfig CameraConfig => cameraData;

        #endregion
    }
}
