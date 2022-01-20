using System;
using Appalachia.UI.Controls.Components.Layout.Models;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Services.Broadcast.CanvasScaling
{
    public class CanvasScalingService : BroadcastService<CanvasScalingService, CanvasScalingServiceMetadata,
        CanvasScalingArgs>
    {
        #region Fields and Autoproperties

        [NonSerialized, ShowInInspector]
        public CanvasDimensionData current;

        #endregion

        public void UpdateCanvasScaling(CanvasDimensionData dimensionData)
        {
            using (_PRF_UpdateCanvasScaling.Auto())
            {
                current = dimensionData;

                var args = new CanvasScalingArgs(dimensionData);

                OnBroadcast(args);
            }
        }

        protected override void OnApplyMetadataInternal()
        {
            using (_PRF_OnApplyMetadataInternal.Auto())
            {
            }
        }

        protected override void SubscribeToAllFunctionalties()
        {
            using (_PRF_SubscribeToAllFunctionalties.Auto())
            {
            }
        }

        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_UpdateCanvasScaling =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateCanvasScaling));

        #endregion
    }
}
