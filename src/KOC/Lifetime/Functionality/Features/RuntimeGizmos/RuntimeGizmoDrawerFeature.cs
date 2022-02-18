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

        /// <inheritdoc />
        protected override async AppaTask BeforeDisable()
        {
            await HideFeature();
        }

        /// <inheritdoc />
        protected override async AppaTask BeforeEnable()
        {
            await ShowFeature();
        }

        /// <inheritdoc />
        protected override async AppaTask BeforeFirstEnable()
        {
            await AppaTask.CompletedTask;
        }

        /// <inheritdoc />
        protected override async AppaTask OnHide()
        {
            _runtimeGizmoDrawerWidget.Hide();
            await AppaTask.CompletedTask;
        }

        /// <inheritdoc />
        protected override async AppaTask OnShow()
        {
            _runtimeGizmoDrawerWidget.Show();
            await AppaTask.CompletedTask;
        }
    }
}
