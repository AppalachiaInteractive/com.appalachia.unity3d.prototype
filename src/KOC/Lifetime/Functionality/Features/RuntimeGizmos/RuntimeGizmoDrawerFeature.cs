using Appalachia.Prototype.KOC.Lifetime.Functionality.Core.Features;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.RuntimeGizmos.Services;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.RuntimeGizmos.Widgets;
using Appalachia.Utility.Async;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.RuntimeGizmos
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

        protected override async AppaTask BeforeDisable()
        {
            using (_PRF_BeforeDisable.Auto())
            {
                await HideFeature();
            }
        }

        protected override async AppaTask BeforeEnable()
        {
            using (_PRF_BeforeEnable.Auto())
            {
                await ShowFeature();
            }
        }

        protected override async AppaTask BeforeFirstEnable()
        {
            await AppaTask.CompletedTask;
        }

        protected override async AppaTask OnHide()
        {
            using (_PRF_OnHide.Auto())
            {
                _runtimeGizmoDrawerWidget.Hide();
                await AppaTask.CompletedTask;
            }
        }

        protected override async AppaTask OnShow()
        {
            using (_PRF_OnShow.Auto())
            {
                _runtimeGizmoDrawerWidget.Show();
                await AppaTask.CompletedTask;
            }
        }
    }
}
