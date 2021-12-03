using Sirenix.OdinInspector;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen.Base
{
    public abstract class SplashScreenSubMetadata<T, TM> : AreaMetadata<T, TM>
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

        [BoxGroup("Timeline Signals")] public bool retroactive; 
        [BoxGroup("Timeline Signals")] public bool emitOnce;
        
        [BoxGroup("Timeline Director")]
        public int frameDelay;

        [BoxGroup("Timeline Director")]
        public bool enableDirector;
        
        [BoxGroup("Timeline Director")]
        public bool autoPlayTimeline;
        
        [BoxGroup("Timeline Director")]
        public DirectorWrapMode wrapMode;

        #endregion
    }
}
