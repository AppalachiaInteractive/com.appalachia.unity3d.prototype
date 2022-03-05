using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Instance;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Settings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Fps
{
    public class RuntimeGraphFpsManager : RuntimeGraphInstanceManager<RuntimeGraphFpsGraph,
        RuntimeGraphFpsManager, RuntimeGraphFpsMonitor, RuntimeGraphFpsText, RuntimeGraphFpsSettings>
    {
        /// <inheritdoc />
        protected override int BasicBackgroundImageIndex => 2;

        /// <inheritdoc />
        protected override int FullBackgroundImageIndex => 0;

        /// <inheritdoc />
        protected override int TextBackgroundImageIndex => 1;

        /// <inheritdoc />
        protected override RuntimeGraphFpsSettings settings => allSettings.fps;
    }
}
