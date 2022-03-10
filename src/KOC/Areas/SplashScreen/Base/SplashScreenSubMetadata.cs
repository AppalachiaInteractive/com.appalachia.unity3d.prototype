using Sirenix.OdinInspector;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Areas.SplashScreen.Base
{
    public abstract class SplashScreenSubMetadata<TManager, TMetadata> : AreaMetadata<TManager, TMetadata>,
                                                                         ISplashScreenSubMetadata
        where TManager : AreaManager<TManager, TMetadata>
        where TMetadata : AreaMetadata<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        [BoxGroup("Timeline Execution")]
        public int order;

        [BoxGroup("Timeline Skipping")]
        public TimelineSkipMode timelineSkipMode;

        [BoxGroup("Timeline Skipping")]
        [HideIf(nameof(_hideSkipDelay))]
        public int skipFrameDelay;

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
        public TimelinePlayMode timelinePlayMode;

        [BoxGroup("Timeline Director")]
        [HideIf(nameof(_hideFrameDelay))]
        public int frameDelay;

        [BoxGroup("Timeline Director")]
        public DirectorWrapMode wrapMode;

        #endregion

        private bool _hideFrameDelay => timelinePlayMode != TimelinePlayMode.AfterDelay;

        private bool _hideSkipDelay => timelineSkipMode != TimelineSkipMode.AfterDelay;

        #region ISplashScreenSubMetadata Members

        public TimelineSkipMode TimelineSkipMode => timelineSkipMode;
        public int SkipFrameDelay => skipFrameDelay;
        public bool EmitOnce => emitOnce;
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
