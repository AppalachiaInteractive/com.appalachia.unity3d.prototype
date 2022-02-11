using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Model;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets;
using Appalachia.Utility.Async;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar
{
    [CallStaticConstructorInEditor]
    public class ActivityBarFeature : DeveloperInterfaceManager_V01.Feature<ActivityBarFeature,
        ActivityBarFeatureMetadata>
    {
        static ActivityBarFeature()
        {
            FunctionalitySet.RegisterWidget<ActivityBarWidget>(
                _dependencyTracker,
                i => _activityBarWidget = i
            );

            When.Any<IActivityBarEntryProvider>().IsAvailableThen(RegisterActivityProvider);
        }

        #region Static Fields and Autoproperties

        private static ActivityBarWidget _activityBarWidget;
        [NonSerialized] private static List<ActivityBarEntry> _bottomEntries;

        [NonSerialized] private static List<ActivityBarEntry> _topEntries;

        #endregion

        public IReadOnlyList<ActivityBarEntry> BottomEntries
        {
            get
            {
                _bottomEntries ??= new List<ActivityBarEntry>();
                return _bottomEntries;
            }
        }

        public IReadOnlyList<ActivityBarEntry> TopEntries
        {
            get
            {
                _topEntries ??= new List<ActivityBarEntry>();
                return _topEntries;
            }
        }

        private static void RegisterActivity(ActivityBarEntry activity)
        {
            using (_PRF_RegisterActivity.Auto())
            {
                if (activity.clampToTop)
                {
                    _topEntries.Add(activity);
                }
                else
                {
                    _bottomEntries.Add(activity);
                    _bottomEntries.Sort((entry1, entry2) => entry1.priority.CompareTo(entry2));
                }
            }
        }

        private static void RegisterActivityProvider(IActivityBarEntryProvider provider)
        {
            using (_PRF_RegisterActivityProvider.Auto())
            {
                var activities = provider.GetActivityBarEntries();

                if (activities == null)
                {
                    return;
                }

                foreach (var activity in activities)
                {
                    RegisterActivity(activity);
                }
            }
        }

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
            using (_PRF_BeforeFirstEnable.Auto())
            {
                await AppaTask.CompletedTask;
            }
        }

        protected override void OnApplyMetadataInternal()
        {
            using (_PRF_OnApplyMetadataInternal.Auto())
            {
                base.OnApplyMetadataInternal();

                ValidateEntries(ref _topEntries);
                ValidateEntries(ref _bottomEntries);
            }
        }

        protected override async AppaTask OnHide()
        {
            using (_PRF_OnHide.Auto())
            {
                _activityBarWidget.Hide();
                await AppaTask.CompletedTask;
            }
        }

        protected override async AppaTask OnShow()
        {
            using (_PRF_OnShow.Auto())
            {
                _activityBarWidget.Show();
                await AppaTask.CompletedTask;
            }
        }

        private void ValidateEntries(ref List<ActivityBarEntry> entries)
        {
            using (_PRF_ValidateEntries.Auto())
            {
                entries ??= new List<ActivityBarEntry>();

                for (var index = entries.Count - 1; index >= 0; index--)
                {
                    var entry = entries[index];

                    if (entry == null)
                    {
                        entries.RemoveAt(index);
                    }
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterActivity =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterActivity));

        private static readonly ProfilerMarker _PRF_RegisterActivityProvider =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterActivityProvider));

        private static readonly ProfilerMarker _PRF_ValidateEntries =
            new ProfilerMarker(_PRF_PFX + nameof(ValidateEntries));

        #endregion
    }
}
