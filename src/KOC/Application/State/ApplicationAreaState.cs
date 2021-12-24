using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Strings;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.State
{
    [Serializable]
    [SmartLabelChildren]
    [HideReferenceObjectPicker]
    public class ApplicationAreaState : AppalachiaSimpleBase
    {
        #region Constants and Static Readonly

        private const int BASE_SIZE = 100;
        private const int STATE_SIZE = BASE_SIZE + 4;
        private const int SUBSTATE_SIZE = BASE_SIZE + 2;
        private const int NEXT_SIZE = BASE_SIZE + 0;

        #endregion

        public ApplicationAreaState(ApplicationArea area)
        {
            _area = area;
            _nextState = ApplicationAreaStates.None;
            _state = ApplicationAreaStates.None;
            _substate = ApplicationAreaSubstates.None;
        }

        #region Fields and Autoproperties

        [ReadOnly, SerializeField, HorizontalGroup("1"), PropertyOrder(0)]
        [HideLabel]
        private ApplicationArea _area;

        [ReadOnly, SerializeField, HorizontalGroup("1", NEXT_SIZE), PropertyOrder(30)]
        [LabelText("Next")]
        [GUIColor(nameof(GetNextStateColor))]
        private ApplicationAreaStates _nextState;

        [ReadOnly, SerializeField, HorizontalGroup("1", STATE_SIZE), PropertyOrder(10)]
        [GUIColor(nameof(GetStateColor))]
        private ApplicationAreaStates _state;

        [ReadOnly, SerializeField, HorizontalGroup("1", SUBSTATE_SIZE), PropertyOrder(20)]
        [LabelText("Subs.")]
        [GUIColor(nameof(GetStateColor))]
        private ApplicationAreaSubstates _substate;

        #endregion

        public ApplicationArea Area => _area;
        public ApplicationAreaStates NextState => _nextState;
        public ApplicationAreaStates State => _state;
        public ApplicationAreaSubstates Substate => _substate;

        public bool HasStateChangedTriggered => _nextState != _state;

        public bool IsAtRest =>
            !HasStateChangedTriggered && (_substate != ApplicationAreaSubstates.InProgress);

        private Color GetNextStateColor => GetStateColorInternal(_nextState, _substate);

        private Color GetStateColor => GetStateColorInternal(_state, _substate);

        public void MarkComplete()
        {
            using (_PRF_MarkComplete.Auto())
            {
                var target = ApplicationAreaSubstates.Complete;

                switch (_substate)
                {
                    case ApplicationAreaSubstates.InProgress:
                        _substate = target;
                        break;
                    case ApplicationAreaSubstates.None:
                    case ApplicationAreaSubstates.Failed:
                    case ApplicationAreaSubstates.Complete:
                        LogIllegalSubstateTransition(_substate, target);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void MarkFailed()
        {
            using (_PRF_MarkFailed.Auto())
            {
                var target = ApplicationAreaSubstates.Failed;

                switch (_substate)
                {
                    case ApplicationAreaSubstates.InProgress:
                        _substate = target;
                        break;
                    case ApplicationAreaSubstates.None:
                    case ApplicationAreaSubstates.Failed:
                    case ApplicationAreaSubstates.Complete:
                        LogIllegalSubstateTransition(_substate, target);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void MarkStateTransitionCompleted()
        {
            using (_PRF_MarkStateTransitionCompleted.Auto())
            {
                if (HasStateChangedTriggered)
                {
                    _state = _nextState;
                    _substate = ApplicationAreaSubstates.InProgress;
                }
                else
                {
                    Context.Log.Error(
                        ZString.Format(
                            "You should not call {0} when no state change is triggered.",
                            nameof(MarkStateTransitionCompleted)
                        )
                    );
                }
            }
        }

        public void QueueActivate()
        {
            using (_PRF_InitiateLoad.Auto())
            {
                var target = ApplicationAreaStates.Activate;

                switch (_state)
                {
                    case ApplicationAreaStates.Load:
                        _nextState = target;
                        break;
                    case ApplicationAreaStates.None:
                    case ApplicationAreaStates.Activate:
                    case ApplicationAreaStates.Unload:
                        LogIllegalStateTransition(_state, target);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void QueueLoad()
        {
            using (_PRF_InitiateLoad.Auto())
            {
                var target = ApplicationAreaStates.Load;

                switch (_state)
                {
                    case ApplicationAreaStates.None:
                        _nextState = target;
                        break;
                    case ApplicationAreaStates.Load:
                    case ApplicationAreaStates.Activate:
                    case ApplicationAreaStates.Unload:
                        LogIllegalStateTransition(_state, target);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void QueueUnload()
        {
            using (_PRF_InitiateLoad.Auto())
            {
                var target = ApplicationAreaStates.Unload;

                switch (_state)
                {
                    case ApplicationAreaStates.Activate:
                        _nextState = target;
                        break;
                    case ApplicationAreaStates.None:
                    case ApplicationAreaStates.Load:
                    case ApplicationAreaStates.Unload:
                        LogIllegalStateTransition(_state, target);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        internal Color GetStateColorInternal(ApplicationAreaStates state, ApplicationAreaSubstates substate)
        {
            using (_PRF_GetStateColorInternal.Auto())
            {
                ColorPaletteSubset subset;

                switch (substate)
                {
                    case ApplicationAreaSubstates.None:
                        subset = ColorPalette.Default.disabledLight;
                        break;
                    case ApplicationAreaSubstates.InProgress:
                        subset = ColorPalette.Default.highlightLight;
                        break;
                    case ApplicationAreaSubstates.Failed:
                        subset = ColorPalette.Default.bad;
                        break;
                    case ApplicationAreaSubstates.Complete:
                        subset = ColorPalette.Default.good;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(substate), substate, null);
                }

                switch (state)
                {
                    case ApplicationAreaStates.None:
                        return subset.First;
                    case ApplicationAreaStates.Load:
                        return subset.OneThird;
                    case ApplicationAreaStates.Activate:
                        return subset.TwoThirds;
                    case ApplicationAreaStates.Unload:
                        return subset.Last;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(state), state, null);
                }
            }
        }

        private void LogIllegalStateTransition(ApplicationAreaStates from, ApplicationAreaStates to)
        {
            using (_PRF_LogIllegalStateTransition.Auto())
            {
                Context.Log.Error(
                    ZString.Format("Can not set area [{0}] from state [{1}] to state [{2}]", _area, from, to)
                );
            }
        }

        private void LogIllegalSubstateTransition(ApplicationAreaSubstates from, ApplicationAreaSubstates to)
        {
            using (_PRF_LogIllegalSubstateTransition.Auto())
            {
                Context.Log.Error(
                    ZString.Format(
                        "Can not set area [{0}] from substate [{1}] to substate [{2}]",
                        _area,
                        from,
                        to
                    )
                );
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ApplicationAreaState) + ".";

        private static readonly ProfilerMarker _PRF_LogIllegalStateTransition =
            new ProfilerMarker(_PRF_PFX + nameof(LogIllegalStateTransition));

        private static readonly ProfilerMarker _PRF_InitiateLoad =
            new ProfilerMarker(_PRF_PFX + nameof(QueueLoad));

        private static readonly ProfilerMarker _PRF_MarkComplete =
            new ProfilerMarker(_PRF_PFX + nameof(MarkComplete));

        private static readonly ProfilerMarker _PRF_MarkFailed =
            new ProfilerMarker(_PRF_PFX + nameof(MarkFailed));

        private static readonly ProfilerMarker _PRF_MarkStateTransitionCompleted =
            new ProfilerMarker(_PRF_PFX + nameof(MarkStateTransitionCompleted));

        private static readonly ProfilerMarker _PRF_GetStateColorInternal =
            new ProfilerMarker(_PRF_PFX + nameof(GetStateColorInternal));

        private static readonly ProfilerMarker _PRF_LogIllegalSubstateTransition =
            new ProfilerMarker(_PRF_PFX + nameof(LogIllegalSubstateTransition));

        #endregion
    }
}
