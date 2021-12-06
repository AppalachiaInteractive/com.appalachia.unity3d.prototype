using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen.Base
{
    public interface ISplashScreenSubMetadata
    {
        public bool AutoPlayTimeline { get; }
        public bool CanSkip { get; }
        public bool EmitOnce { get; }
        public bool EnableDirector { get; }
        public bool Retroactive { get; }
        public bool UnloadSceneImmediately { get; }
        public DirectorWrapMode WrapMode { get; }
        public int FrameDelay { get; }
        public int Order { get; }
        SignalAsset TimelineEndSignalAsset { get; }
        SignalAsset TimelineStartSignalAsset { get; }
        TimelineAsset TimelineAsset { get; }
    }
}
