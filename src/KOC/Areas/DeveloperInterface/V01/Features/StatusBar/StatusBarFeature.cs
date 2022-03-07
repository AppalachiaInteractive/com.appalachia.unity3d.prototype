using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar
{
    [CallStaticConstructorInEditor]
    public class StatusBarFeature : DeveloperInterfaceManager_V01.Feature<StatusBarFeature, StatusBarFeatureMetadata>
    {
        static StatusBarFeature()
        {
            FunctionalitySet.RequireFeature<PerformanceProfilingFeature>(
                _dependencyTracker,
                i => _performanceProfilingFeature = i
            );
        }

        #region Static Fields and Autoproperties

        private static PerformanceProfilingFeature _performanceProfilingFeature;

        #endregion
    }
}
