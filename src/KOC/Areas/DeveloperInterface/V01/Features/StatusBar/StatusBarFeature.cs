using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Entries.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar
{
    [CallStaticConstructorInEditor]
    public class StatusBarFeature : DeveloperInterfaceManager_V01.Feature<StatusBarFeature,
        StatusBarFeatureMetadata>
    {
        static StatusBarFeature()
        {
            FunctionalitySet.RegisterWidget<StatusBarWidget>(_dependencyTracker, i => _statusBarWidget = i);

            When.Any<IStatusBarEntry>().IsAvailableThen(RegisterStatus);
        }

        #region Static Fields and Autoproperties

        private static ActivityBarFeature _activityBarFeature;

        private static StatusBarWidget _statusBarWidget;

        #endregion

        private static void RegisterStatus(IStatusBarEntry status)
        {
            using (_PRF_RegisterStatus.Auto())
            {
                _statusBarWidget.RegisterStatus(status);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterStatus =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterStatus));

        #endregion
    }
}
