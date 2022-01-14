using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugConditions.Model
{
    [Serializable]
    public class DebugConditionPacketSettings : AppalachiaObject<DebugConditionPacketSettings>
    {
        #region Fields and Autoproperties

        /// <summary>
        ///     Controls whether or not the package is activated or deactivated based on timing.
        /// </summary>
        [BoxGroup("Activation")]
        [PropertyTooltip("Controls whether or not the package is activated or deactivated based on timing.")]
        public PacketActivationType activation;

        /// <summary>
        ///     Time to wait before checking if conditions are met (use this to avoid low fps drops triggering the conditions when loading the game).
        /// </summary>
        [BoxGroup("Activation")]
        [PropertyTooltip(
            "Time to wait before checking if conditions are met (use this to avoid low fps drops triggering the conditions when loading the game)."
        )]
        [ShowIf(nameof(_showEnableTimeAfterFields))]
        public float EnableAfterSeconds = 2f;

        /// <summary>
        ///     Time to wait before disabling condition checks for the packet.
        /// </summary>
        [BoxGroup("Activation")]
        [PropertyTooltip("Time to wait before disabling condition checks for the packet.")]
        [ShowIf(nameof(_showDisableTimeAfterFields))]
        public float DisablefterSeconds = 600f;

        /// <summary>
        ///     The logical constraint for evaluating conditions.
        /// </summary>
        [BoxGroup("Conditions")]
        [PropertyTooltip("The logical constraint for evaluating conditions.")]
        public EvaluationType evaluationType = EvaluationType.AllConditionsMet;

        /// <summary>
        ///     The conditions that define the packet.  When these are evaluated successfully based on their criteria, the <see cref="action" /> will occur.
        /// </summary>
        [BoxGroup("Conditions")]
        [PropertyTooltip("List of conditions that will be checked each frame.")]
        public List<DebugCondition> conditions = new();

        /// <summary>
        ///     The actions that should occur when the conditions are met.
        /// </summary>
        [BoxGroup("Action")]
        [PropertyTooltip("The actions that should occur when the conditions are met.")]
        [Required]
        public ExecutionAction action;

        /// <summary>
        ///     The time scale to use when the packet conditions are met.
        /// </summary>
        [BoxGroup("Action")]
        [PropertyRange(0.0f, 1.0f)]
        [ShowIf(nameof(_showModifyTimeActionFields))]
        [Required]
        public float timeScale = 1.0f;

        /// <summary>
        ///     The name of the screenshot that will be saved by this packet.
        /// </summary>
        [BoxGroup("Action")]
        [ShowIf(nameof(_showScreenshotActionFields))]
        [Required]
        public string screenshotFileName = "DebugCondition_X_Screenshot";

        /// <summary>
        ///     The log message to show when this packet's conditions are met.
        /// </summary>
        [BoxGroup("Action")]
        [ShowIf(nameof(_showLogMessageActionFields))]
        [Required]
        public LogMessageType logMessageType;

        [BoxGroup("Action")]
        [Multiline]
        [Required]
        [ShowIf(nameof(_showLogMessageActionFields))]
        public string logMessage = string.Empty;

        /// <summary>
        ///     Whether or not the condition should be enabled by default.
        /// </summary>
        [BoxGroup("Metadata")]
        [PropertyTooltip(
            "If false, it won't be enabled unless manually turned on via the Developer Interface."
        )]
        [ToggleLeft]
        public bool EnabledByDefault = true;

        /// <summary>
        ///     A unique identifier for this packet type. It's used to get or remove packets in runtime.
        /// </summary>
        [BoxGroup("Metadata")]
        [PropertyTooltip(
            "A unique identifier for this packet type. It's used to get or remove packets in runtime."
        )]
        [Required]
        public string key;

        /// <summary>
        ///     Time to wait before checking if conditions are met successively (once they have already been met and if ExecuteOnce is false).
        /// </summary>
        [PropertyTooltip(
            "Time to wait before checking if conditions are met successively (once they have already been met and if ExecuteOnce is false)"
        )]
        [PropertyRange(0.1f, 600f)]
        public float preventReexecutionForSeconds = 2f;

        [BoxGroup("Metadata")]
        [PropertyTooltip(
            "If non-null, the package can only be executed this number of times.  After that, it will be disabled."
        )]
        public int? executionLimit;

        #endregion

        private bool _showDebuggerBreakActionFields => action.Has(ExecutionAction.DebuggerBreak);
        private bool _showDisableTimeAfterFields => activation.Has(PacketActivationType.DisableAfterSeconds);

        private bool _showEnableTimeAfterFields => activation.Has(PacketActivationType.EnableAfterSeconds);
        private bool _showLogMessageActionFields => action.Has(ExecutionAction.LogMessage);
        private bool _showModifyTimeActionFields => action.Has(ExecutionAction.ModifyTimeScale);

        private bool _showOpenDeveloperInterfaceActionFields =>
            action.Has(ExecutionAction.OpenDeveloperInterface);

        private bool _showPauseEditorActionFields => action.Has(ExecutionAction.PauseEditor);

        private bool _showScreenshotActionFields => action.Has(ExecutionAction.Screenshot);
    }
}
