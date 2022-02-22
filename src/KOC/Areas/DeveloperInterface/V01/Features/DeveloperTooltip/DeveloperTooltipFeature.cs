using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperTooltip.Widgets;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperTooltip
{
    [CallStaticConstructorInEditor]
    public class DeveloperTooltipFeature : DeveloperInterfaceManager_V01.Feature<DeveloperTooltipFeature,
        DeveloperTooltipFeatureMetadata>
    {
        static DeveloperTooltipFeature()
        {
            FunctionalitySet.RegisterWidget<DeveloperTooltipWidget>(
                _dependencyTracker,
                i => _developerTooltipWidget = i
            );
        }

        #region Static Fields and Autoproperties

        private static DeveloperTooltipWidget _developerTooltipWidget;

        #endregion
    }
}
