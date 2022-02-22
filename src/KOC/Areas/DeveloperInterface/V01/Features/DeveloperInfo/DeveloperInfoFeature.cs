using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Services;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Widgets;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo
{
    [CallStaticConstructorInEditor]
    public class DeveloperInfoFeature : DeveloperInterfaceManager_V01.Feature<DeveloperInfoFeature,
        DeveloperInfoFeatureMetadata>
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
    }
}
