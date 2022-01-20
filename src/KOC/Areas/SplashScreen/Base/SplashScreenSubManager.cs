using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Extensions;
using Appalachia.Prototype.KOC.Input;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Areas.SplashScreen.Base
{
    public abstract class SplashScreenSubManager<TManager, TMetadata> : AreaManager<TManager, TMetadata>,
                                                                        KOCInputActions.ISplashScreenActions
        where TManager : SplashScreenSubManager<TManager, TMetadata>
        where TMetadata : SplashScreenSubMetadata<TManager, TMetadata>
    {
        #region Fields and Autoproperties

        protected SignalReceiver _signalReceiver;
        protected PlayableDirector _playableDirector;

        private bool _didPlayTimeline;

        [NonSerialized] private bool _alreadySkipping;

        // ReSharper disable once UnassignedField.Global
        protected bool hasSkipEnablingBeenSignaled;

        #endregion

        public override ApplicationArea ParentArea => ApplicationArea.SplashScreen;

        #region Event Functions

        protected override void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

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

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                var directorName = GetChildObjectName("Director");

                gameObject.GetOrAddComponentInChild(ref _playableDirector, directorName);

                _playableDirector.gameObject.GetOrAddComponent(ref _signalReceiver);

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
                rootCanvas.CanvasFadeManager.ForceActive();

                rootCanvas.CanvasFadeManager.FadeOutCompleted += () =>
                {
                    _playableDirector.Stop();
                    _playableDirector.enabled = false;

                    enabled = false;
                };

                rootCanvas.CanvasFadeManager.FadeOut();
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

                        if (AwakeDuration > areaMetadata.skipFrameDelay)
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

        private static readonly ProfilerMarker _PRF_OnContinue =
            new ProfilerMarker(_PRF_PFX + nameof(OnContinue));

        private static readonly ProfilerMarker _PRF_OnTimelineEnd =
            new ProfilerMarker(_PRF_PFX + nameof(OnTimelineEnd));

        private static readonly ProfilerMarker _PRF_OnTimelineStart =
            new ProfilerMarker(_PRF_PFX + nameof(OnTimelineStart));

        private static readonly ProfilerMarker _PRF_SkipTimeline =
            new ProfilerMarker(_PRF_PFX + nameof(SkipTimeline));

        #endregion
    }
}
