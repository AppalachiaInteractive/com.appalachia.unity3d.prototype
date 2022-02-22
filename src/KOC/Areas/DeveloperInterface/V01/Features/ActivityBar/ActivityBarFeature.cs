using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Entries.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets;
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

            When.Any<IActivityBarEntry>().IsAvailableThen(RegisterActivity);
        }

        #region Static Fields and Autoproperties

        private static ActivityBarWidget _activityBarWidget;

        #endregion

        private static void RegisterActivity(IActivityBarEntry activity)
        {
            using (_PRF_RegisterActivity.Auto())
            {
                if (activity.Metadata.Enabled)
                {
                    _activityBarWidget.RegisterActivity(activity);
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterActivity =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterActivity));

        #endregion
    }
}
