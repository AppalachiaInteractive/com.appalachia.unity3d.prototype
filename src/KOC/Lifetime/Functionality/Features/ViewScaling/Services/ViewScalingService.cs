using System;
using Appalachia.Utility.Events;
using Appalachia.Prototype.KOC.Application.Features.Aspects;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Services;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.ViewScaling.Models;
using Appalachia.UI.Controls.Components.Layout.Models;
using Appalachia.Utility.Events.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.ViewScaling.Services
{
    public class ViewScalingService :
        LifetimeService<ViewScalingService, ViewScalingServiceMetadata, ViewScalingFeature,
            ViewScalingFeatureMetadata>,
        Broadcaster.IService<ViewScalingService, ViewScalingServiceMetadata, ViewScalingArgs>
    {
        #region Fields and Autoproperties

        public ValueEvent<ViewScalingArgs>.Data Broadcast;

        [NonSerialized, ShowInInspector, InlineProperty, HideLabel, BoxGroup("Current")]
        public ViewDimensionData current;

        #endregion

        public void DisableCanvasScaling()
        {
            using (_PRF_DisableCanvasScaling.Auto())
            {
                current = new ViewDimensionData();
                var args = new ViewScalingArgs(current);

                OnBroadcast(args);
            }
        }

        [Button]
        public void ReBroadcast()
        {
            using (_PRF_ReBroadcast.Auto())
            {
                var args = new ViewScalingArgs(current);

                OnBroadcast(args);
            }
        }

        public void UpdateViewScaling(ViewDimensionData dimensionData)
        {
            using (_PRF_UpdateCanvasScaling.Auto())
            {
                if (current == dimensionData)
                {
                    return;
                }

                current = dimensionData;

                var args = new ViewScalingArgs(dimensionData);

                OnBroadcast(args);
            }
        }

        private void OnBroadcast(ViewScalingArgs args)
        {
            using (_PRF_OnBroadcast.Auto())
            {
                Broadcast.RaiseEvent(args);
            }
        }

        #region IService<ViewScalingService,ViewScalingServiceMetadata,ViewScalingArgs> Members

        public int Subscribers => Broadcast.SubscriberCount;

        void Broadcaster.IService<ViewScalingService, ViewScalingServiceMetadata, ViewScalingArgs>.
            OnBroadcast(ViewScalingArgs args)
        {
            using (_PRF_OnBroadcast.Auto())
            {
                OnBroadcast(args);
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_DisableCanvasScaling =
            new ProfilerMarker(_PRF_PFX + nameof(DisableCanvasScaling));

        protected static readonly ProfilerMarker _PRF_OnBroadcast =
            new ProfilerMarker(_PRF_PFX + nameof(OnBroadcast));

        private static readonly ProfilerMarker _PRF_ReBroadcast =
            new ProfilerMarker(_PRF_PFX + nameof(ReBroadcast));

        private static readonly ProfilerMarker _PRF_UpdateCanvasScaling =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateViewScaling));

        #endregion
    }
}
