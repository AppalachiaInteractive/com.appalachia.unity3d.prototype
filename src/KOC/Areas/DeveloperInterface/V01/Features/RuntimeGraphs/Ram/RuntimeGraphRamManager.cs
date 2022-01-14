using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Settings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Ram
{
    public class RuntimeGraphRamManager : RuntimeGraphInstanceManager<RuntimeGraphRamGraph,
        RuntimeGraphRamManager, RuntimeGraphRamMonitor, RuntimeGraphRamText, RuntimeGraphRamSettings>
    {
        protected override int BasicBackgroundImageIndex => 1;
        protected override int FullBackgroundImageIndex => 0;
        protected override int TextBackgroundImageIndex => 1;
        protected override RuntimeGraphRamSettings settings => allSettings.ram;
    }
}
