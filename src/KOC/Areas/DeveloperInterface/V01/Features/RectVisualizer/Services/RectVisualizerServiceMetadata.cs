using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.UI.Controls.Common;
using Appalachia.UI.Core.Components.Data;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Services
{
    public class RectVisualizerServiceMetadata : DeveloperInterfaceMetadata_V01.ServiceMetadata<
                                                     RectVisualizerService, RectVisualizerServiceMetadata,
                                                     RectVisualizerFeature, RectVisualizerFeatureMetadata>,
                                                 GizmoDrawer.IServiceMetadata
    {
        #region Fields and Autoproperties

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public CameraData cameraData;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public UpdateTiming updates;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [PropertyRange(1, 100)]
        public int updateSteps;

        #endregion

        protected override void UpdateFunctionality(RectVisualizerService functionality)
        {
            using (_PRF_Apply.Auto())
            {
                var camera = functionality.DrawCamera;
                CameraData.UpdateComponent(ref cameraData, camera, this);
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(
                    this,
                    nameof(updates),
                    updates == null,
                    () => updates = UpdateTiming.Milliseconds(50, 30000, 5000)
                );

                initializer.Do(this, nameof(updateSteps), updateSteps == 0, () => updateSteps = 10);
            }
        }

        protected override void SubscribeResponsiveComponents(RectVisualizerService target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                cameraData.Changed.Event += OnChanged;
            }
        }

        #region IServiceMetadata Members

        public CameraData CameraData => cameraData;

        #endregion
    }
}
