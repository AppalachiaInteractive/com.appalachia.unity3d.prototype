using Sirenix.OdinInspector;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen.Base
{
    public abstract class SplashScreenSubMetadata<T, TM> : AreaMetadata<T, TM>, ISplashScreenSubMetadata
        where T : AreaManager<T, TM>
        where TM : AreaMetadata<T, TM>
    {
        #region Fields and Autoproperties

        [BoxGroup("Timeline Execution")]
        public int order;

        [BoxGroup("Timeline Execution")]
        public bool canSkip;

        [BoxGroup("Timeline Execution")]
        public bool unloadSceneImmediately;

        [BoxGroup("Timeline")] public TimelineAsset timelineAsset;
        [BoxGroup("Timeline")] public SignalAsset timelineStartSignalAsset;
        [BoxGroup("Timeline")] public SignalAsset timelineEndSignalAsset;

        [BoxGroup("Timeline Signals")]
        public bool retroactive;

        [BoxGroup("Timeline Signals")]
        public bool emitOnce;

        [BoxGroup("Timeline Director")]
        public int frameDelay;

        [BoxGroup("Timeline Director")]
        public bool enableDirector;

        [BoxGroup("Timeline Director")]
        public bool autoPlayTimeline;

        [BoxGroup("Timeline Director")]
        public DirectorWrapMode wrapMode;

        #endregion

        #region ISplashScreenSubMetadata Members

        public bool AutoPlayTimeline => autoPlayTimeline;
        public bool CanSkip => canSkip;
        public bool EmitOnce => emitOnce;
        public bool EnableDirector => enableDirector;
        public bool Retroactive => retroactive;
        public bool UnloadSceneImmediately => unloadSceneImmediately;
        public DirectorWrapMode WrapMode => wrapMode;
        public int FrameDelay => frameDelay;
        public int Order => order;
        public SignalAsset TimelineEndSignalAsset => timelineEndSignalAsset;
        public SignalAsset TimelineStartSignalAsset => timelineStartSignalAsset;
        public TimelineAsset TimelineAsset => timelineAsset;

        #endregion
    }
}
