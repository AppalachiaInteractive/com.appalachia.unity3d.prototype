using System.Linq;
using Appalachia.Prototype.KOC.Application.Extensions;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Unity.Profiling;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen.Base
{
    public abstract class SplashScreenSubManager<T, TM> : AreaManager<T, TM>
        where T : SplashScreenSubManager<T, TM>
        where TM : SplashScreenSubMetadata<T, TM>
    {
        #region Fields and Autoproperties

        private SignalReceiver _signalReceiver;
        private PlayableDirector _playableDirector;

        #endregion

        public override ApplicationArea ParentArea => ApplicationArea.SplashScreen;

        #region Event Functions

        protected override void Update()
        {
            using (_PRF_Update.Auto())
            {
                base.Update();
                
                if (!_playableDirector.enabled)
                {
                    if (metadata.enableDirector)
                    {
                        if (WakeDuration > metadata.frameDelay)
                        {
                            _playableDirector.enabled = true;                            
                        }
                    }
                }
            }
        }

        #endregion

        public void OnTimelineEnd()
        {
            using (_PRF_OnTimelineEnd.Auto())
            {
                AppaLog.Context.Area.Info(nameof(OnTimelineEnd));
            }
        }

        public void OnTimelineStart()
        {
            using (_PRF_OnTimelineStart.Auto())
            {
                AppaLog.Context.Area.Info(nameof(OnTimelineStart));
            }
        }

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                var directorName = GetChildObjectName("Director");

                gameObject.CreateOrGetComponentInChild(ref _playableDirector, directorName);

                _playableDirector.gameObject.CreateOrGetComponent(ref _signalReceiver);
                _playableDirector.enabled = false;

#if UNITY_EDITOR
                CreateAreaAsset(ref metadata.timelineAsset,            metadata);
                CreateAreaAsset(ref metadata.timelineStartSignalAsset, metadata, "Start");
                CreateAreaAsset(ref metadata.timelineEndSignalAsset,  metadata, "Stop");

#endif
                var signalTrack = metadata.timelineAsset.GetOrCreateSignalTrack();

                signalTrack.SetStartAndEndEmitters(
                    metadata.timelineStartSignalAsset,
                    metadata.timelineEndSignalAsset,
                    metadata.retroactive,
                    metadata.emitOnce
                );

                _signalReceiver.ConnectMethodToSignal(metadata.timelineStartSignalAsset, OnTimelineStart);
                _signalReceiver.ConnectMethodToSignal(metadata.timelineEndSignalAsset, OnTimelineEnd);

                _playableDirector.playableAsset = metadata.timelineAsset;
                _playableDirector.playOnAwake = metadata.autoPlayTimeline;
                _playableDirector.extrapolationMode = metadata.wrapMode;
                _playableDirector.SetGenericBinding(signalTrack, _signalReceiver);
            }
        }

        protected override void OnActivation()
        {
            using (_PRF_Activate.Auto())
            {
                AppaLog.Context.Area.Info(nameof(OnActivation));
            }
        }

        protected override void OnDeactivation()
        {
            using (_PRF_Deactivate.Auto())
            {
                AppaLog.Context.Area.Info(nameof(OnDeactivation));
            }
        }

        protected override void ResetArea()
        {
            using (_PRF_ResetArea.Auto())
            {
                AppaLog.Context.Area.Info(nameof(ResetArea));
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(SplashScreenSubManager<T, TM>) + ".";

        private static readonly ProfilerMarker _PRF_OnTimelineStart =
            new ProfilerMarker(_PRF_PFX + nameof(OnTimelineStart));

        private static readonly ProfilerMarker _PRF_OnTimelineEnd =
            new ProfilerMarker(_PRF_PFX + nameof(OnTimelineEnd));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(Update));

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        private static readonly ProfilerMarker _PRF_Activate =
            new ProfilerMarker(_PRF_PFX + nameof(OnActivation));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeactivation));

        #endregion
    }
}
