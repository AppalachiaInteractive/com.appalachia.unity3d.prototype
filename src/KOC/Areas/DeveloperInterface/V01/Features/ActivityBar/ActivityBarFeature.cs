using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar
{
    [CallStaticConstructorInEditor]
    public class ActivityBarFeature : DeveloperInterfaceManager_V01.Feature<ActivityBarFeature,
        ActivityBarFeatureMetadata>
    {
        static ActivityBarFeature()
        {
            FunctionalitySet.RegisterWidget<ActivityBarWidget>(_dependencyTracker, i => _activityBarWidget = i);
        }

        #region Static Fields and Autoproperties

        private static ActivityBarWidget _activityBarWidget;

        #endregion
    }
}
