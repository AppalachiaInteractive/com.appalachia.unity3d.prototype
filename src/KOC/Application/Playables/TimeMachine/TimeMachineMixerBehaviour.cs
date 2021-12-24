using System.Collections.Generic;
using Appalachia.Prototype.KOC.Application.Extensions;
using Appalachia.Utility.Execution;
using UnityEngine.Playables;

namespace Appalachia.Prototype.KOC.Application.Playables.TimeMachine
{
    public class TimeMachineMixerBehaviour : AppalachiaPlayable
    {
        #region Fields and Autoproperties

        public Dictionary<string, double> markerClips;
        private PlayableDirector director;

        #endregion

        public override void OnPlayableCreate(Playable playable)
        {
            director = playable.GetGraph().GetResolver() as PlayableDirector;
        }

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
    }
}
