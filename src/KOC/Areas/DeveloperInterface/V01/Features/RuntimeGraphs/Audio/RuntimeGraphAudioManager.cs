using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Settings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Audio
{
    public class RuntimeGraphAudioManager : RuntimeGraphInstanceManager<RuntimeGraphAudioGraph,
        RuntimeGraphAudioManager, RuntimeGraphAudioMonitor, RuntimeGraphAudioText, RuntimeGraphAudioSettings>
    {
        /// <inheritdoc />
        protected override int BasicBackgroundImageIndex => 1;

        /// <inheritdoc />
        protected override int FullBackgroundImageIndex => 0;

        /// <inheritdoc />
        protected override int TextBackgroundImageIndex => 1;

        /// <inheritdoc />
        protected override RuntimeGraphAudioSettings settings => allSettings.audio;
    }
}
