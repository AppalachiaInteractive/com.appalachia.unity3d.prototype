using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Services;
using Appalachia.UI.Core.Components.Data;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos.Services
{
    public class RuntimeGizmoDrawerServiceMetadata : LifetimeServiceMetadata<RuntimeGizmoDrawerService,
                                                         RuntimeGizmoDrawerServiceMetadata,
                                                         RuntimeGizmoDrawerFeature,
                                                         RuntimeGizmoDrawerFeatureMetadata>,
                                                     GizmoDrawer.IServiceMetadata
    {
        #region Fields and Autoproperties

        public CameraData cameraData;

        #endregion

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(RuntimeGizmoDrawerService target)
        {
            cameraData.Changed.Event += OnChanged;
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(RuntimeGizmoDrawerService functionality)
        {
            var drawCamera = functionality.DrawCamera;
            CameraData.RefreshAndUpdateComponent(ref cameraData, this, drawCamera);
        }

        #region IServiceMetadata Members

        public CameraData CameraData => cameraData;

        #endregion
    }
}
