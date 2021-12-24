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

        [BoxGroup("Timeline Skipping")]
        public TimelineSkipMode timelineSkipMode;

        [BoxGroup("Timeline Skipping")]
        [ShowIf(nameof(_showSkipDelay))]
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
        [ShowIf(nameof(_showFrameDelay))]
        public int frameDelay;

        [BoxGroup("Timeline Director")]
        public DirectorWrapMode wrapMode;

        #endregion

        private bool _showSkipDelay => timelineSkipMode == TimelineSkipMode.AfterDelay;
        private bool _showFrameDelay => timelinePlayMode == TimelinePlayMode.AfterDelay;

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
