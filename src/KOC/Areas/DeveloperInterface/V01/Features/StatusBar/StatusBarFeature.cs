using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Model;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Model;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets;
using Appalachia.Utility.Async;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar
{
    [CallStaticConstructorInEditor]
    public class StatusBarFeature : DeveloperInterfaceManager_V01.Feature<StatusBarFeature,
                                        StatusBarFeatureMetadata>,
                                    IActivityBarEntryProvider
    {
        static StatusBarFeature()
        {
            FunctionalitySet.RegisterWidget<StatusBarWidget>(_dependencyTracker, i => _statusBarWidget = i);

            When.Any<IStatusBarEntryProvider>().IsAvailableThen(RegisterStatusProvider);
        }

        #region Static Fields and Autoproperties

        private static ActivityBarFeature _activityBarFeature;

        [NonSerialized] private static List<StatusBarEntry> _leftEntries;
        [NonSerialized] private static List<StatusBarEntry> _rightEntries;

        private static StatusBarWidget _statusBarWidget;

        #endregion

        public IReadOnlyList<StatusBarEntry> LeftEntries
        {
            get
            {
                _leftEntries ??= new List<StatusBarEntry>();
                return _leftEntries;
            }
        }

        public IReadOnlyList<StatusBarEntry> RightEntries
        {
            get
            {
                _rightEntries ??= new List<StatusBarEntry>();
                return _rightEntries;
            }
        }

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
            _statusBarWidget.Hide();
            await AppaTask.CompletedTask;
        }

        /// <inheritdoc />
        protected override async AppaTask OnShow()
        {
            _statusBarWidget.Show();
            await AppaTask.CompletedTask;
        }

        private static void RegisterStatus(StatusBarEntry status)
        {
            using (_PRF_RegisterStatus.Auto())
            {
                if (status.clampToLeft)
                {
                    _leftEntries.Add(status);
                }
                else
                {
                    _rightEntries.Add(status);
                    _rightEntries.Sort((entry1, entry2) => entry1.priority.CompareTo(entry2));
                }
            }
        }

        private static void RegisterStatusProvider(IStatusBarEntryProvider provider)
        {
            using (_PRF_RegisterStatusProvider.Auto())
            {
                var statusEntries = provider.GetStatusBarEntries();

                if (statusEntries == null)
                {
                    return;
                }

                foreach (var status in statusEntries)
                {
                    RegisterStatus(status);
                }
            }
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

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterStatus =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterStatus));

        private static readonly ProfilerMarker _PRF_RegisterStatusProvider =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterStatusProvider));

        private static readonly ProfilerMarker _PRF_GetActivityBarEntries =
            new ProfilerMarker(_PRF_PFX + nameof(GetActivityBarEntries));

        #endregion
    }
}
