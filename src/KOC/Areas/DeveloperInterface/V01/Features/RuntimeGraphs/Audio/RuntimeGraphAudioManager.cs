using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Settings;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Audio
{
    public class RuntimeGraphAudioManager : RuntimeGraphInstanceManager<RuntimeGraphAudioGraph,
        RuntimeGraphAudioManager, RuntimeGraphAudioMonitor, RuntimeGraphAudioText, RuntimeGraphAudioSettings>
    {
        protected override int BasicBackgroundImageIndex => 1;
        protected override int FullBackgroundImageIndex => 0;
        protected override int TextBackgroundImageIndex => 1;
        protected override RuntimeGraphAudioSettings settings => allSettings.audio;
    }
}
