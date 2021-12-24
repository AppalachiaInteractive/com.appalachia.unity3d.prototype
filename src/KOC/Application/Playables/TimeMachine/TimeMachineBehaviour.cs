using System;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Playables.TimeMachine
{
    [Serializable]
    public class TimeMachineBehaviour : AppalachiaPlayable
    {
        public enum Condition
        {
            Always,
            Never,
            ObjectIsActive,
            ObjectIsNotActive,
            ComponentIsEnabled,
            ComponentIsDisabled,
            SignalReceived,
        }

        public enum TimeMachineAction
        {
            Marker,
            JumpToTime,
            JumpToMarker,
            Pause,
        }

        #region Fields and Autoproperties

        [HideInInspector] public bool clipExecuted; //the user shouldn't author this, the Mixer does
        public Condition condition;
        public float timeToJumpTo;
        public GameObject gameObject;

        public MonoBehaviour component;
        public string markerToJumpTo;
        public string markerLabel;
        public TimeMachineAction action;
        private bool _receivedSignal;

        #endregion

        public bool ConditionMet()
        {
            switch (condition)
            {
                case Condition.Always:
                    return true;

                case Condition.ObjectIsActive:
                    return gameObject.activeInHierarchy;
                case Condition.ObjectIsNotActive:
                    return !gameObject.activeInHierarchy;
                case Condition.ComponentIsEnabled:
                    return component.enabled;
                case Condition.ComponentIsDisabled:
                    return !component.enabled;
                case Condition.SignalReceived:
                    return _receivedSignal;
                case Condition.Never:
                default:
                    return false;
            }
        }

        public void SignalCondition()
        {
            _receivedSignal = true;
        }
    }
}
