using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Widgets;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips
{
    [CallStaticConstructorInEditor]
    public class DevTooltipsFeature : DeveloperInterfaceManager_V01.Feature<DevTooltipsFeature,
        DevTooltipsFeatureMetadata>
    {
        static DevTooltipsFeature()
        {
            FunctionalitySet.RegisterWidget<DevTooltipsWidget>(_dependencyTracker, i => _devTooltipsWidget = i);
        }

        #region Static Fields and Autoproperties

        private static DevTooltipsWidget _devTooltipsWidget;

        #endregion
    }
}
