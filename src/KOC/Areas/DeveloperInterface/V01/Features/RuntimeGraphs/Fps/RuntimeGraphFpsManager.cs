using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Settings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Fps
{
    public class RuntimeGraphFpsManager : RuntimeGraphInstanceManager<RuntimeGraphFpsGraph,
        RuntimeGraphFpsManager, RuntimeGraphFpsMonitor, RuntimeGraphFpsText, RuntimeGraphFpsSettings>
    {
        protected override int BasicBackgroundImageIndex => 2;
        protected override int FullBackgroundImageIndex => 0;
        protected override int TextBackgroundImageIndex => 1;
        protected override RuntimeGraphFpsSettings settings => allSettings.fps;
    }
}
