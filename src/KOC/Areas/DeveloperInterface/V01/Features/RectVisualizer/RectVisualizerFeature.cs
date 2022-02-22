using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Services;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Widgets;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer
{
    [CallStaticConstructorInEditor]
    [ExecuteAlways]
    public class RectVisualizerFeature : DeveloperInterfaceManager_V01.Feature<RectVisualizerFeature,
        RectVisualizerFeatureMetadata>
    {
        static RectVisualizerFeature()
        {
            FunctionalitySet.RegisterService<RectVisualizerService>(
                _dependencyTracker,
                i => _rectVisualizerService = i
            );

            FunctionalitySet.RegisterWidget<RectVisualizerWidget>(
                _dependencyTracker,
                i => _rectVisualizerWidget = i
            );
        }

        #region Static Fields and Autoproperties

        private static RectVisualizerService _rectVisualizerService;
        private static RectVisualizerWidget _rectVisualizerWidget;

        #endregion

        public RectVisualizerService RectVisualizerService => _rectVisualizerService;
        public RectVisualizerWidget RectVisualizerWidget => _rectVisualizerWidget;

        [ButtonGroup(GROUP_NAME)]
        public void DiscoverTargets()
        {
            using (_PRF_RefreshTargets.Auto())
            {
                _rectVisualizerService.DiscoverTargets();
            }
        }

        [ButtonGroup(GROUP_NAME)]
        public void UpdateTargets()
        {
            using (_PRF_UpdateTargets.Auto())
            {
                _rectVisualizerService.UpdateTargets().Forget();
            }
        }

        /// <inheritdoc />
        protected override async AppaTask BeforeFirstEnable()
        {
            metadata.Changed.Event += _rectVisualizerService.DiscoverTargets;

            await AppaTask.CompletedTask;
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_RefreshTargets =
            new ProfilerMarker(_PRF_PFX + nameof(DiscoverTargets));

        private static readonly ProfilerMarker _PRF_UpdateTargets =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateTargets));

        #endregion
    }
}
