using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Ram
{
    public class G_RamManager : RuntimeGraphInstanceManager<G_RamGraph, G_RamManager, G_RamMonitor, G_RamText,
        RuntimeGraphRamSettings>
    {
        protected override int BasicBackgroundImageIndex => 1;
        protected override int FullBackgroundImageIndex => 0;
        protected override int TextBackgroundImageIndex => 1;
        protected override RuntimeGraphRamSettings settings => allSettings.ram;
    }
}
