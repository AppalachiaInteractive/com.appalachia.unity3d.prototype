using System;
using Appalachia.Prototype.KOC.Application.Extensions;
using Appalachia.Prototype.KOC.Application.Input;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Application.Areas.SplashScreen.Base
{
    public abstract class SplashScreenSubManager<T, TM> : AreaManager<T, TM>,
                                                          KOCInputActions.ISplashScreenActions
        where T : SplashScreenSubManager<T, TM>
        where TM : SplashScreenSubMetadata<T, TM>
    {
        #region Fields and Autoproperties

        protected SignalReceiver _signalReceiver;
        protected PlayableDirector _playableDirector;

        private bool _didPlayTimeline;

        [NonSerialized] private bool _alreadySkipping;

        protected bool hasSkipEnablingBeenSignaled;

        #endregion

        
        public override ApplicationArea ParentArea => ApplicationArea.SplashScreen;

        #region Event Functions

        protected override void Update()
        {
            using (_PRF_Update.Auto())
            {
                base.Update();

                if (AppalachiaApplication.IsPlaying)
                {
                    if (!_didPlayTimeline && (areaMetadata.timelinePlayMode == TimelinePlayMode.AfterDelay))
                    {
                        if (AwakeDuration > areaMetadata.frameDelay)
                        {
                            _didPlayTimeline = true;

                            _playableDirector.enabled = true;
                            _playableDirector.Play();
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
                Context.Log.Info(nameof(OnTimelineEnd), this);

                var parent = AreaRegistry.GetParentManager(this);

                if (parent is ISplashScreenManager splashScreenManager)
                {
                    splashScreenManager.NotifyTimelineCompleted(this);
                }
            }
        }

        public void OnTimelineStart()
        {
            using (_PRF_OnTimelineStart.Auto())
            {
                Context.Log.Info(nameof(OnTimelineStart), this);
            }
        }

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                var directorName = GetChildObjectName("Director");

                gameObject.GetOrCreateComponentInChild(ref _playableDirector, directorName);

                _playableDirector.gameObject.GetOrCreateComponent(ref _signalReceiver);

                _playableDirector.enabled = false;

                _playableDirector.playOnAwake = areaMetadata.timelinePlayMode == TimelinePlayMode.Auto;
                _playableDirector.playableAsset = areaMetadata.timelineAsset;
                _playableDirector.extrapolationMode = areaMetadata.wrapMode;

                _playableDirector.enabled = true;

#if UNITY_EDITOR
                CreateAreaAsset(ref areaMetadata.timelineAsset,            areaMetadata);
                CreateAreaAsset(ref areaMetadata.timelineStartSignalAsset, areaMetadata, "Start");
                CreateAreaAsset(ref areaMetadata.timelineEndSignalAsset,   areaMetadata, "Stop");

#endif
                var signalTrack = areaMetadata.timelineAsset.GetOrCreateSignalTrack();

                signalTrack.SetStartAndEndEmitters(
                    areaMetadata.timelineStartSignalAsset,
                    areaMetadata.timelineEndSignalAsset,
                    areaMetadata.retroactive,
                    areaMetadata.emitOnce
                );

#if UNITY_EDITOR
                _signalReceiver.ConnectMethodToSignal(areaMetadata.timelineStartSignalAsset, OnTimelineStart);
                _signalReceiver.ConnectMethodToSignal(areaMetadata.timelineEndSignalAsset,   OnTimelineEnd);

                _playableDirector.SetGenericBinding(signalTrack, _signalReceiver);
#endif
            }
        }

        protected override void OnActivation()
        {
            using (_PRF_Activate.Auto())
            {
                Context.Log.Info(nameof(OnActivation), this);
            }
        }

        protected override void OnDeactivation()
        {
            using (_PRF_Deactivate.Auto())
            {
                Context.Log.Info(nameof(OnDeactivation), this);
            }
        }

        protected override void ResetArea()
        {
            using (_PRF_ResetArea.Auto())
            {
                Context.Log.Info(nameof(ResetArea), this);
            }
        }

        private void SkipTimeline()
        {
            using (_PRF_SkipTimeline.Auto())
            {
                canvas.canvasFadeManager.ForceActive();

                canvas.canvasFadeManager.FadeOutCompleted += () =>
                {
                    _playableDirector.Stop();
                    _playableDirector.enabled = false;

                    enabled = false;
                };

                canvas.canvasFadeManager.FadeOut();
                _alreadySkipping = true;
            }
        }

        #region ISplashScreenActions Members

        public virtual void OnContinue(InputAction.CallbackContext context)
        {
            using (_PRF_OnContinue.Auto())
            {
                if (_alreadySkipping)
                {
                    return;
                }

                switch (areaMetadata.timelineSkipMode)
                {
                    case TimelineSkipMode.None:
                        return;

                    case TimelineSkipMode.AfterDelay:

                        if (WakeDuration > areaMetadata.skipFrameDelay)
                        {
                            SkipTimeline();
                        }

                        break;

                    case TimelineSkipMode.OnceSignalled:
                        if (hasSkipEnablingBeenSignaled)
                        {
                            SkipTimeline();
                        }

                        break;

                    case TimelineSkipMode.Immediately:

                        SkipTimeline();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(SplashScreenSubManager<T, TM>) + ".";

        private static readonly ProfilerMarker _PRF_SkipTimeline =
            new ProfilerMarker(_PRF_PFX + nameof(SkipTimeline));

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

        private static readonly ProfilerMarker _PRF_OnContinue =
            new ProfilerMarker(_PRF_PFX + nameof(OnContinue));

        #endregion
    }
}
