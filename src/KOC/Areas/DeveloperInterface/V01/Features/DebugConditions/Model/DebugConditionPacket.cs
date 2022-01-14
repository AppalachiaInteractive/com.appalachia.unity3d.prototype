using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Events;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugConditions.Model
{
    [Serializable]
    public class DebugConditionPacket
    {
        public DebugConditionPacket(DebugConditionPacketSettings settings)
        {
            this.settings = settings;
            active = settings.EnabledByDefault;
        }

        #region Fields and Autoproperties

        [PropertyTooltip("If false, it won't be evaluated.")]
        public bool active;

        public DebugConditionPacketSettings settings;

        public List<Action> callbacks = new();

        public UnityEvent unityEvents;

        [NonSerialized] private bool _eligibleForEvaluation;
        [NonSerialized] private bool _hasExecutedPreviously;
        [NonSerialized] private bool _shouldDestroy;
        [NonSerialized] private float _timePassed;
        [NonSerialized] private int _executionCount;

        #endregion

        public bool EligibleForEvaluation => active && !_shouldDestroy && _eligibleForEvaluation;

        public bool ShouldDestroy => _shouldDestroy;

        public void RecordExecution()
        {
            using (_PRF_RecordExecution.Auto())
            {
                _eligibleForEvaluation = false;
                _hasExecutedPreviously = true;
                _timePassed = 0f;
                _executionCount += 1;
            }
        }

        public void UpdateState()
        {
            using (_PRF_Update.Auto())
            {
                if (_eligibleForEvaluation || _shouldDestroy)
                {
                    return;
                }

                if (settings.executionLimit.HasValue && (_executionCount >= settings.executionLimit.Value))
                {
                    _eligibleForEvaluation = false;
                    _shouldDestroy = true;
                    return;
                }

                _timePassed += Time.deltaTime;

                if (Time.realtimeSinceStartup >= settings.DisablefterSeconds)
                {
                    _eligibleForEvaluation = false;
                    return;
                }

                if (_hasExecutedPreviously && (_timePassed >= settings.preventReexecutionForSeconds))
                {
                    _eligibleForEvaluation = true;
                }
                else if (!_hasExecutedPreviously && (_timePassed >= settings.EnableAfterSeconds))
                {
                    _eligibleForEvaluation = true;
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(DebugConditionPacket) + ".";

        private static readonly ProfilerMarker _PRF_RecordExecution =
            new ProfilerMarker(_PRF_PFX + nameof(RecordExecution));

        private static readonly ProfilerMarker _PRF_Update =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateState));

        #endregion
    }
}
