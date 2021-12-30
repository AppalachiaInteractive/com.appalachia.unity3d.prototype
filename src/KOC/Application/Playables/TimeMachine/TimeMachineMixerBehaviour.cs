using System.Collections.Generic;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Extensions;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using UnityEngine.Playables;

namespace Appalachia.Prototype.KOC.Application.Playables.TimeMachine
{
    public class TimeMachineMixerBehaviour : AppalachiaPlayable<TimeMachineMixerBehaviour>
    {
        #region Fields and Autoproperties

        public Dictionary<string, double> markerClips;
#pragma warning disable CS0649
        private PlayableDirector director;
#pragma warning restore CS0649

        #endregion

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            //ScriptPlayable<TimeMachineBehaviour> inputPlayable = (ScriptPlayable<TimeMachineBehaviour>)playable.GetInput(i);
            //Debug.Log(PlayableExtensions.GetTime<ScriptPlayable<TimeMachineBehaviour>>(inputPlayable));

            if (!AppalachiaApplication.IsPlayingOrWillPlay)
            {
                return;
            }

            var inputCount = playable.GetInputCount();

            for (var i = 0; i < inputCount; i++)
            {
                var inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<TimeMachineBehaviour>)playable.GetInput(i);
                var input = inputPlayable.GetBehaviour();

                if (inputWeight > 0f)
                {
                    if (!input.clipExecuted)
                    {
                        switch (input.action)
                        {
                            case TimeMachineBehaviour.TimeMachineAction.Pause:
                                if (input.ConditionMet())
                                {
                                    director.PauseTimeline();
                                    input.clipExecuted =
                                        true; //this prevents the command to be executed every frame of this clip
                                }

                                break;

                            case TimeMachineBehaviour.TimeMachineAction.JumpToTime:
                            case TimeMachineBehaviour.TimeMachineAction.JumpToMarker:
                                if (input.ConditionMet())
                                {
                                    //Rewind
                                    if (input.action == TimeMachineBehaviour.TimeMachineAction.JumpToTime)
                                    {
                                        //Jump to time
                                        (playable.GetGraph().GetResolver() as PlayableDirector).time =
                                            input.timeToJumpTo;
                                    }
                                    else
                                    {
                                        //Jump to marker
                                        var t = markerClips[input.markerToJumpTo];
                                        (playable.GetGraph().GetResolver() as PlayableDirector).time = t;
                                    }

                                    input.clipExecuted = false; //we want the jump to happen again!
                                }

                                break;
                        }
                    }
                }
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await AppaTask.CompletedTask;
        }

        protected override void OnPause(Playable playable, FrameData info)
        {
        }

        protected override void OnPlay(Playable playable, FrameData info)
        {
        }

        protected override void Update(Playable playable, FrameData info, object playerData)
        {
        }

        protected override void WhenDestroyed(Playable playable)
        {
        }

        protected override void WhenStarted(Playable playable)
        {
        }

        protected override void WhenStopped(Playable playable)
        {
        }
    }
}
