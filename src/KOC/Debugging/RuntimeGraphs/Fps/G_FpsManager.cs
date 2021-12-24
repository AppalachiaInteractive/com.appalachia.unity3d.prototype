using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Fps
{
    public class G_FpsManager : RuntimeGraphInstanceManager<G_FpsGraph, G_FpsManager, G_FpsMonitor, G_FpsText,
        RuntimeGraphFpsSettings>
    {
        protected override int BasicBackgroundImageIndex => 2;
        protected override int FullBackgroundImageIndex => 0;
        protected override int TextBackgroundImageIndex => 1;
        protected override RuntimeGraphFpsSettings settings => allSettings.fps;
    }
}
