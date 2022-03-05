using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Appalachia.Utility.Strings;
using UnityEngine;
using UnityEngine.Profiling;

// ReSharper disable FormatStringProblem

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling.Services.Memory
{
    [CallStaticConstructorInEditor]
    public sealed class MemoryProfilerService : DeveloperInterfaceManager_V01.Service<MemoryProfilerService,
        MemoryProfilerServiceMetadata, PerformanceProfilingFeature, PerformanceProfilingFeatureMetadata>
    {
        #region Constants and Static Readonly

        private const string STRING_SCENE_NAME_AND_COUNT = "{0} ({1})";

        private static readonly Utf8PreparedFormat<string, int> FORMAT_SCENE_NAME_AND_COUNT =
            new(STRING_SCENE_NAME_AND_COUNT);

        #endregion

        #region Fields and Autoproperties

        [SerializeField] private float _allocatedRam;
        [SerializeField] private float _reservedRam;
        [SerializeField] private float _monoRam;

        #endregion

        public float AllocatedRam => _allocatedRam;
        public float MonoRAM => _monoRam;
        public float ReservedRam => _reservedRam;

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                _allocatedRam = Profiler.GetTotalAllocatedMemoryLong() / 1048576f;
                _reservedRam = Profiler.GetTotalReservedMemoryLong() / 1048576f;
                _monoRam = Profiler.GetMonoUsedSizeLong() / 1048576f;
            }
        }

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
            }
        }
    }
}
