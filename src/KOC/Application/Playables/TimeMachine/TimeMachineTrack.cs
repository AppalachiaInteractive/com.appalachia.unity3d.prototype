using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Application.Playables.TimeMachine
{
    [TrackColor(0.7366781f, 0.3261246f, 0.8529412f)]
    [TrackClipType(typeof(TimeMachineClip))]
    public class TimeMachineTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var scriptPlayable = ScriptPlayable<TimeMachineMixerBehaviour>.Create(graph, inputCount);

            var b = scriptPlayable.GetBehaviour();
            b.markerClips = new System.Collections.Generic.Dictionary<string, double>();

            //This foreach will rename clips based on what they do, and collect the markers and put them into a dictionary
            //Since this happens when you enter Preview or Play mode, the object holding the Timeline must be enabled or you won't see any change in names
            foreach (var c in GetClips())
            {
                var clip = (TimeMachineClip)c.asset;
                var clipName = c.displayName;

                switch (clip.action)
                {
                    case TimeMachineBehaviour.TimeMachineAction.Pause:
                        clipName = "||";
                        break;

                    case TimeMachineBehaviour.TimeMachineAction.Marker:
                        clipName = "● " + clip.markerLabel;

                        //Insert the marker clip into the Dictionary of markers
                        if (!b.markerClips.ContainsKey(
                                clip.markerLabel
                            )) //happens when you duplicate a clip and it has the same markerLabel
                        {
                            b.markerClips.Add(clip.markerLabel, c.start);
                        }

                        break;

                    case TimeMachineBehaviour.TimeMachineAction.JumpToMarker:
                        clipName = "↩︎  " + clip.markerToJumpTo;
                        break;

                    case TimeMachineBehaviour.TimeMachineAction.JumpToTime:
                        clipName = "↩ " + clip.timeToJumpTo;
                        break;
                }

                c.displayName = clipName;
            }

            return scriptPlayable;
        }
    }
}
