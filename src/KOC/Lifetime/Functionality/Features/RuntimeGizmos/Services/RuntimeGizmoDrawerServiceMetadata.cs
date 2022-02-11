using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Services;
using Appalachia.UI.Core.Components.Data;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.RuntimeGizmos.Services
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

        protected override void SubscribeResponsiveComponents(RuntimeGizmoDrawerService target)
        {
            cameraData.Changed.Event += OnChanged;
        }

        protected override void UpdateFunctionality(RuntimeGizmoDrawerService functionality)
        {
            var drawCamera = functionality.DrawCamera;
            CameraData.UpdateComponent(ref cameraData, drawCamera, this);
        }

        #region IServiceMetadata Members

        public CameraData CameraData => cameraData;

        #endregion
    }
}
