using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Core.Features;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos.Services;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos.Widgets;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.RuntimeGizmos
{
    public class
        RuntimeGizmoDrawerFeature : LifetimeFeature<RuntimeGizmoDrawerFeature,
            RuntimeGizmoDrawerFeatureMetadata>
    {
        static RuntimeGizmoDrawerFeature()
        {
            FunctionalitySet.RegisterWidget<RuntimeGizmoDrawerWidget>(
                _dependencyTracker,
                i => _runtimeGizmoDrawerWidget = i
            );
            FunctionalitySet.RegisterService<RuntimeGizmoDrawerService>(
                _dependencyTracker,
                i => _runtimeGizmoDrawerService = i
            );
        }

        #region Static Fields and Autoproperties

        private static RuntimeGizmoDrawerService _runtimeGizmoDrawerService;

        private static RuntimeGizmoDrawerWidget _runtimeGizmoDrawerWidget;

        #endregion

        public RuntimeGizmoDrawerService RuntimeGizmoDrawerService => _runtimeGizmoDrawerService;
        public RuntimeGizmoDrawerWidget RuntimeGizmoDrawerWidget => _runtimeGizmoDrawerWidget;
    }
}
