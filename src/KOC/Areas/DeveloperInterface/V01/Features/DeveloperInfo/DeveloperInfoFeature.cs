using System;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Model;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Services;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Model;
using Appalachia.Utility.Async;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo
{
    [CallStaticConstructorInEditor]
    public class DeveloperInfoFeature : DeveloperInterfaceManager_V01.Feature<DeveloperInfoFeature,
                                            DeveloperInfoFeatureMetadata>,
                                        IActivityBarEntryProvider,
                                        IStatusBarEntryProvider
    {
        static DeveloperInfoFeature()
        {
            FunctionalitySet.RegisterService<DeveloperInfoProviderService>(
                _dependencyTracker,
                i => _developerInfoProviderService = i
            );
            FunctionalitySet.RegisterWidget<DeveloperInfoOverlayWidget>(
                _dependencyTracker,
                i => _developerInfoOverlayWidget = i
            );
            FunctionalitySet.RegisterFeature<ActivityBarFeature>(
                _dependencyTracker,
                i => _activityBarFeature = i
            );
        }

        #region Static Fields and Autoproperties

        private static ActivityBarFeature _activityBarFeature;

        private static DeveloperInfoOverlayWidget _developerInfoOverlayWidget;

        private static DeveloperInfoProviderService _developerInfoProviderService;

        #endregion

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
            _developerInfoOverlayWidget.Hide();
            await AppaTask.CompletedTask;
        }

        /// <inheritdoc />
        protected override async AppaTask OnShow()
        {
            _developerInfoOverlayWidget.Show();
            await AppaTask.CompletedTask;
        }

        #region IActivityBarEntryProvider Members

        public ActivityBarEntry[] GetActivityBarEntries()
        {
            using (_PRF_GetActivityBarEntries.Auto())
            {
                return Array.Empty<ActivityBarEntry>();
            }
        }

        #endregion

        #region IStatusBarEntryProvider Members

        public StatusBarEntry[] GetStatusBarEntries()
        {
            using (_PRF_GetStatusBarEntries.Auto())
            {
                return Array.Empty<StatusBarEntry>();
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetActivityBarEntries =
            new ProfilerMarker(_PRF_PFX + nameof(GetActivityBarEntries));

        private static readonly ProfilerMarker _PRF_GetStatusBarEntries =
            new ProfilerMarker(_PRF_PFX + nameof(GetStatusBarEntries));

        #endregion
    }
}
