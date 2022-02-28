using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Services.Audio;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Services.Memory;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling.Services.Rendering;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.PerformanceProfiling
{
    [CallStaticConstructorInEditor]
    public class PerformanceProfiingFeature : DeveloperInterfaceManager_V01.Feature<PerformanceProfiingFeature
        , PerformanceProfiingFeatureMetadata>
    {
        static PerformanceProfiingFeature()
        {
            FunctionalitySet.RegisterService<MemoryProfilerService>(
                _dependencyTracker,
                i => _developerInfoProviderService = i
            );
            FunctionalitySet.RegisterService<RenderingProfilerService>(
                _dependencyTracker,
                i => _renderingProfilerService = i
            );
            FunctionalitySet.RegisterService<AudioProfilerService>(
                _dependencyTracker,
                i => _audioProfilerService = i
            );
        }

        #region Static Fields and Autoproperties

        private static AudioProfilerService _audioProfilerService;
        private static MemoryProfilerService _developerInfoProviderService;
        private static RenderingProfilerService _renderingProfilerService;

        #endregion
    }
}
