using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Settings;
using UnityEngine.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Ram
{
    public class RuntimeGraphRamMonitor : RuntimeGraphInstanceMonitor<RuntimeGraphRamGraph,
        RuntimeGraphRamManager, RuntimeGraphRamMonitor, RuntimeGraphRamText, RuntimeGraphRamSettings>
    {
        #region Fields and Autoproperties

        public float AllocatedRam { get; private set; }
        public float ReservedRam { get; private set; }
        public float MonoRam { get; private set; }

        #endregion

        protected override RuntimeGraphRamSettings settings => allSettings.ram;

        #region Event Functions

        private void Update()
        {
            if (ShouldSkipUpdate)
            {
                return;
            }

            AllocatedRam = Profiler.GetTotalAllocatedMemoryLong() / 1048576f;
            ReservedRam = Profiler.GetTotalReservedMemoryLong() / 1048576f;
            MonoRam = Profiler.GetMonoUsedSizeLong() / 1048576f;
        }

        #endregion
    }
}
