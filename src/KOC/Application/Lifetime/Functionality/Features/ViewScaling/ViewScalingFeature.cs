using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Features;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.ViewScaling.Models;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.ViewScaling.Services;
using Appalachia.UI.Functionality.Canvas.Components;
using Appalachia.UI.Functionality.Rendering.Cameras.Components.Viewport;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.ViewScaling
{
    [CallStaticConstructorInEditor]
    public class ViewScalingFeature : LifetimeFeature<ViewScalingFeature, ViewScalingFeatureMetadata>
    {
        static ViewScalingFeature()
        {
            FunctionalitySet.RegisterService<ViewScalingService>(
                _dependencyTracker,
                viewScalingService => { _viewScalingService = viewScalingService; }
            );

            When.Behaviour(instance)
                .IsAvailableThen(
                     myInstance =>
                     {
                         _viewScalingService.Broadcast.Event += myInstance.OnServiceBroadcast;

                         When.AnyInstance<AppaCanvasScaler>()
                             .IsEnabledThen(myInstance.OnCanvasScalerEnabled);

                         When.AnyInstance<AppaCameraViewportAdjuster>()
                             .IsEnabledThen(myInstance.OnCameraViewportAdjusterEnabled);
                     }
                 );
        }

        #region Static Fields and Autoproperties

        private static ViewScalingService _viewScalingService;

        #endregion

        #region Fields and Autoproperties

        public AppaEvent<ViewScalingArgs>.Data Broadcast;

        #endregion

        [Button]
        public void ReBroadcast()
        {
            using (_PRF_ReBroadcast.Auto())
            {
                _viewScalingService.ReBroadcast();
            }
        }

        private void OnCameraViewportAdjusterEnabled(AppaEvent<AppaCameraViewportAdjuster>.Args args)
        {
            using (_PRF_CameraViewportAdjusterEnabled.Auto())
            {
                var component = args.value;

                Broadcast.Event += broadcastArgs =>
                {
                    var dimensions = broadcastArgs.value.dimensionData;

                    component.Apply(dimensions);
                };
            }
        }

        private void OnCanvasScalerEnabled(AppaEvent<AppaCanvasScaler>.Args args)
        {
            using (_PRF_CanvasScalerEnabled.Auto())
            {
                var component = args.value;

                Broadcast.Event += broadcastArgs =>
                {
                    var dimensions = broadcastArgs.value.dimensionData;

                    component.Apply(dimensions);
                };
            }
        }

        private void OnServiceBroadcast(AppaEvent<ViewScalingArgs>.Args args)
        {
            using (_PRF_OnServiceBroadcast.Auto())
            {
                Broadcast.RaiseEvent(args.value);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_CameraViewportAdjusterEnabled =
            new ProfilerMarker(_PRF_PFX + nameof(OnCameraViewportAdjusterEnabled));

        private static readonly ProfilerMarker _PRF_CanvasScalerEnabled =
            new ProfilerMarker(_PRF_PFX + nameof(OnCanvasScalerEnabled));

        private static readonly ProfilerMarker _PRF_OnServiceBroadcast =
            new ProfilerMarker(_PRF_PFX + nameof(OnServiceBroadcast));

        private static readonly ProfilerMarker _PRF_ReBroadcast =
            new ProfilerMarker(_PRF_PFX + nameof(ReBroadcast));

        #endregion
    }
}
