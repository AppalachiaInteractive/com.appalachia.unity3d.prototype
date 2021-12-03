using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Application.Areas;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Logging;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.State
{
    [Serializable]
    [SmartLabelChildren]
    [HideReferenceObjectPicker]
    public class ApplicationAreaState
    {
        public ApplicationAreaState(ApplicationArea area)
        {
            _area = area;
            _nextState = ApplicationAreaStates.None;
            _state = ApplicationAreaStates.None;
            _substate = ApplicationAreaSubstates.None;
        }

        #region Fields and Autoproperties

        [ShowInInspector, ReadOnly, SerializeField]
        private ApplicationArea _area;

        [ShowInInspector, ReadOnly, SerializeField, GUIColor(nameof(GetNextStateColor))]
        private ApplicationAreaStates _nextState;

        [ShowInInspector, ReadOnly, SerializeField, GUIColor(nameof(GetStateColor))]
        private ApplicationAreaStates _state;

        [ShowInInspector, ReadOnly, SerializeField, GUIColor(nameof(GetStateColor))]
        private ApplicationAreaSubstates _substate;

        #endregion

        public ApplicationArea Area => _area;
        public ApplicationAreaStates NextState => _nextState;
        public ApplicationAreaStates State => _state;
        public ApplicationAreaSubstates Substate => _substate;

        public bool HasStateChangedTriggered => _nextState != _state;

        public bool IsAtRest =>
            !HasStateChangedTriggered && (_substate != ApplicationAreaSubstates.InProgress);

        private Color GetNextStateColor =>
            GetStateColorInternal(_nextState, ApplicationAreaSubstates.InProgress);

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
                    AppaLog.Error(
                        $"You should not call {nameof(MarkStateTransitionCompleted)} when no state change is triggered."
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
                        subset = ColorPalette.Default.disabled;
                        break;
                    case ApplicationAreaSubstates.InProgress:
                        subset = ColorPalette.Default.highlight;
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
                        return subset.Quarter;
                    case ApplicationAreaStates.Activate:
                        return subset.Half;
                    case ApplicationAreaStates.Unload:
                        return subset.ThreeQuarters;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(state), state, null);
                }
            }
        }

        private void LogIllegalStateTransition(ApplicationAreaStates from, ApplicationAreaStates to)
        {
            using (_PRF_LogIllegalStateTransition.Auto())
            {
                AppaLog.Error($"Can not set area [{_area}] to state [{from}] from current state [{to}]");
            }
        }

        private void LogIllegalSubstateTransition(ApplicationAreaSubstates from, ApplicationAreaSubstates to)
        {
            using (_PRF_LogIllegalSubstateTransition.Auto())
            {
                AppaLog.Error(
                    $"Can not set area [{_area}] to substate [{from}] from current substate [{to}]"
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
