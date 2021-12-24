using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Instance;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Audio
{
    public class G_AudioManager : RuntimeGraphInstanceManager<G_AudioGraph, G_AudioManager, G_AudioMonitor,
        G_AudioText, RuntimeGraphAudioSettings>
    {
        protected override int BasicBackgroundImageIndex => 1;
        protected override int FullBackgroundImageIndex => 0;
        protected override int TextBackgroundImageIndex => 1;
        protected override RuntimeGraphAudioSettings settings => allSettings.audio;
    }
}
