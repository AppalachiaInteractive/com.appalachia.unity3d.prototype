using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Playables.TimeMachine
{
    [Serializable]
    public class TimeMachineClip : PlayableAsset, ITimelineClipAsset
    {
        #region Fields and Autoproperties

        [HideIf(nameof(_hideCondition))]
        public TimeMachineBehaviour.Condition condition;

        [HideIf(nameof(_hideCondition))]
        [ShowIf(nameof(_showGameObject))]
        public ExposedReference<GameObject> gameObject;

        [HideIf(nameof(_hideCondition))]
        [ShowIf(nameof(_showComponent))]
        public ExposedReference<MonoBehaviour> component;

        [HideIf(nameof(_hideCondition))]
        [ShowIf(nameof(_showTimeToJumpTo))]
        public float timeToJumpTo;

        [HideIf(nameof(_hideCondition))]
        [ShowIf(nameof(_showMarkerLabel))]
        public string markerLabel = "";

        [HideIf(nameof(_hideCondition))]
        [ShowIf(nameof(_showMarkerToJumpTo))]
        public string markerToJumpTo = "";

        public TimeMachineBehaviour.TimeMachineAction action;
        [HideInInspector] public TimeMachineBehaviour template = new TimeMachineBehaviour();

        #endregion

        private bool _hideCondition => action == TimeMachineBehaviour.TimeMachineAction.Marker;

        private bool _showComponent =>
            condition is TimeMachineBehaviour.Condition.ComponentIsDisabled or TimeMachineBehaviour.Condition
               .ComponentIsEnabled;

        private bool _showGameObject =>
            condition is TimeMachineBehaviour.Condition.ObjectIsActive or TimeMachineBehaviour.Condition
               .ObjectIsNotActive;

        private bool _showMarkerLabel => action == TimeMachineBehaviour.TimeMachineAction.Marker;
        private bool _showMarkerToJumpTo => action == TimeMachineBehaviour.TimeMachineAction.JumpToMarker;

        private bool _showTimeToJumpTo => action == TimeMachineBehaviour.TimeMachineAction.JumpToTime;

        /// <inheritdoc />
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TimeMachineBehaviour>.Create(graph, template);
            var clone = playable.GetBehaviour();
            clone.gameObject = gameObject.Resolve(graph.GetResolver());
            clone.component = component.Resolve(graph.GetResolver());
            clone.markerToJumpTo = markerToJumpTo;
            clone.markerToJumpTo = markerToJumpTo;
            clone.action = action;
            clone.condition = condition;
            clone.markerLabel = markerLabel;
            clone.timeToJumpTo = timeToJumpTo;

            return playable;
        }

        #region ITimelineClipAsset Members

        public ClipCaps clipCaps => ClipCaps.None;

        #endregion
    }
}
