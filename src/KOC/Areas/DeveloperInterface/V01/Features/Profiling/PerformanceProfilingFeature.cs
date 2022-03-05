using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Services.Audio;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Services.FPS;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Services.Memory;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling
{
    [CallStaticConstructorInEditor]
    public class PerformanceProfilingFeature : DeveloperInterfaceManager_V01.Feature<PerformanceProfilingFeature,
        PerformanceProfilingFeatureMetadata>
    {
        static PerformanceProfilingFeature()
        {
            FunctionalitySet.RegisterService<MemoryProfilerService>(
                _dependencyTracker,
                i => _developerInfoProviderService = i
            );
            FunctionalitySet.RegisterService<FPSProfilerService>(_dependencyTracker, i => _fpsProfilerService = i);
            FunctionalitySet.RegisterService<AudioProfilerService>(_dependencyTracker, i => _audioProfilerService = i);
        }

        #region Static Fields and Autoproperties

        private static AudioProfilerService _audioProfilerService;
        private static FPSProfilerService _fpsProfilerService;
        private static MemoryProfilerService _developerInfoProviderService;

        #endregion
    }
}
