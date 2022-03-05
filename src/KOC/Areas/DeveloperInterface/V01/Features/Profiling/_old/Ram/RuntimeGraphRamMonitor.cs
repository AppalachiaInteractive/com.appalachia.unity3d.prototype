using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Instance;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Settings;
using UnityEngine.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Ram
{
    public class RuntimeGraphRamMonitor : RuntimeGraphInstanceMonitor<RuntimeGraphRamGraph,
        RuntimeGraphRamManager, RuntimeGraphRamMonitor, RuntimeGraphRamText, RuntimeGraphRamSettings>
    {
        #region Fields and Autoproperties

        public float AllocatedRam { get; private set; }
        public float ReservedRam { get; private set; }
        public float MonoRam { get; private set; }

        #endregion

        /// <inheritdoc />
        protected override RuntimeGraphRamSettings settings => allSettings.ram;

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                AllocatedRam = Profiler.GetTotalAllocatedMemoryLong() / 1048576f;
                ReservedRam = Profiler.GetTotalReservedMemoryLong() / 1048576f;
                MonoRam = Profiler.GetMonoUsedSizeLong() / 1048576f;
            }
        }

        #endregion
    }
}
