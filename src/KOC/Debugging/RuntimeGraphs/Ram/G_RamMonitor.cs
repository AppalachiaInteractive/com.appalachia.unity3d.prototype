using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;
using UnityEngine.Profiling;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Ram
{
    public class G_RamMonitor : RuntimeGraphInstanceMonitor<G_RamGraph, G_RamManager, G_RamMonitor, G_RamText,
        RuntimeGraphRamSettings>
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
            if (!DependenciesAreReady || !FullyInitialized)
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
