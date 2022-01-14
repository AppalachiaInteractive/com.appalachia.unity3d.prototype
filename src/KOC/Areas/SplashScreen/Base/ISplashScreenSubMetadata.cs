using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Areas.SplashScreen.Base
{
    public interface ISplashScreenSubMetadata
    {
        public bool EmitOnce { get; }
        public bool Retroactive { get; }
        public bool UnloadSceneImmediately { get; }
        public DirectorWrapMode WrapMode { get; }
        public int FrameDelay { get; }
        public int Order { get; }
        public int SkipFrameDelay { get; }
        SignalAsset TimelineEndSignalAsset { get; }
        SignalAsset TimelineStartSignalAsset { get; }
        TimelineAsset TimelineAsset { get; }
        public TimelineSkipMode TimelineSkipMode { get; }
    }
}
