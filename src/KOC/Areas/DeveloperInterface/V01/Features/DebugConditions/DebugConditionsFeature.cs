using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugConditions.Model;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Audio;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Fps;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Ram;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Screenshot.Services;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Strings;
using Appalachia.Utility.Timing;
using Unity.Profiling;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugConditions
{
    [CallStaticConstructorInEditor]
    public sealed class DebugConditionsFeature : DeveloperInterfaceManager_V01.Feature<DebugConditionsFeature,
        DebugConditionsFeatureMetadata>
    {
        static DebugConditionsFeature()
        {
            /*RegisterDependency<RuntimeGraphFpsMonitor>(i => _runtimeGraphFpsMonitor = i);
            RegisterDependency<RuntimeGraphRamMonitor>(i => _runtimeGraphRamMonitor = i);
            RegisterDependency<RuntimeGraphAudioMonitor>(i => _runtimeGraphAudioMonitor = i);
            RegisterDependency<ScreenshotService>(i => _screenshotService = i);*/

            When.Feature(instance).AndFeatureMetadata(metadata).AreAvailableThen((f, m) => f.SyncPackets(m));
        }

        #region Static Fields and Autoproperties

        private static RuntimeGraphAudioMonitor _runtimeGraphAudioMonitor;
        private static RuntimeGraphFpsMonitor _runtimeGraphFpsMonitor;
        private static RuntimeGraphRamMonitor _runtimeGraphRamMonitor;
        private static ScreenshotService _screenshotService;

        #endregion

        #region Fields and Autoproperties

        [SerializeField] private List<DebugConditionPacket> debugPackets = new();

        #endregion

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                CheckDebugPackets();
            }
        }

        #endregion

        /// <summary>
        ///     Add an Action callback to all the Packets with the specified ID in the DebugPacket list.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="key"></param>
        public void AddCallbackToAllDebugPacketWithId(Action callback, string key)
        {
            using (_PRF_AddCallbackToAllDebugPacketWithId.Auto())
            {
                if (GetAllDebugPacketsWithId(key) != null)
                {
                    foreach (var debugPacket in GetAllDebugPacketsWithId(key))
                    {
                        if (callback != null)
                        {
                            debugPacket.callbacks.Add(callback);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Add an Action callback to the first Packet with the specified ID in the DebugPacket list.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="key"></param>
        public void AddCallbackToFirstDebugPacketWithId(Action callback, string key)
        {
            using (_PRF_AddCallbackToFirstDebugPacketWithId.Auto())
            {
                if (GetFirstDebugPacketWithId(key) != null)
                {
                    GetFirstDebugPacketWithId(key).callbacks.Add(callback);
                }
            }
        }

        /// <summary>
        ///     Add a new DebugPacket.
        /// </summary>
        public void AddNewDebugPacket(DebugConditionPacket newDebugConditionPacket)
        {
            using (_PRF_AddNewDebugPacket.Auto())
            {
                debugPackets?.Add(newDebugConditionPacket);
            }
        }

        /// <summary>
        ///     Returns a list with all the Packets with the specified ID in the DebugPacket list.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<DebugConditionPacket> GetAllDebugPacketsWithId(string key)
        {
            using (_PRF_GetAllDebugPacketsWithId.Auto())
            {
                return debugPackets.FindAll(x => x.settings.key == key);
            }
        }

        /// <summary>
        ///     Returns the first Packet with the specified ID in the DebugPacket list.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DebugConditionPacket GetFirstDebugPacketWithId(string key)
        {
            using (_PRF_GetFirstDebugPacketWithId.Auto())
            {
                return debugPackets.First(x => x.settings.key == key);
            }
        }

        /// <summary>
        ///     Removes all the Packets with the specified ID in the DebugPacket list.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void RemoveAllDebugPacketsWithId(string key)
        {
            using (_PRF_RemoveAllDebugPacketsWithId.Auto())
            {
                if (debugPackets != null)
                {
                    debugPackets.RemoveAll(x => x.settings.key == key);
                }
            }
        }

        /// <summary>
        ///     Removes the first Packet with the specified ID in the DebugPacket list.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public void RemoveFirstDebugPacketWithId(string key)
        {
            using (_PRF_RemoveFirstDebugPacketWithId.Auto())
            {
                if ((debugPackets != null) && (GetFirstDebugPacketWithId(key) != null))
                {
                    debugPackets.Remove(GetFirstDebugPacketWithId(key));
                }
            }
        }

        protected override async AppaTask BeforeDisable()
        {
            await AppaTask.CompletedTask;
        }

        protected override async AppaTask BeforeEnable()
        {
            await AppaTask.CompletedTask;
        }

        protected override async AppaTask BeforeFirstEnable()
        {
            await AppaTask.CompletedTask;
        }

        protected override async AppaTask OnHide()
        {
            using (_PRF_OnHide.Auto())
            {
                await AppaTask.CompletedTask;
            }
        }

        protected override async AppaTask OnShow()
        {
            using (_PRF_OnShow.Auto())
            {
                await AppaTask.CompletedTask;
            }
        }

        private bool AreDebugConditionsMet(DebugConditionPacket packet)
        {
            using (_PRF_AreDebugConditionsMet.Auto())
            {
                foreach (var condition in packet.settings.conditions)
                {
                    if (EvaluateDebugCondition(condition))
                    {
                        switch (packet.settings.evaluationType)
                        {
                            case EvaluationType.AllConditionsMet:
                                continue;
                            case EvaluationType.AnyConditionMet:
                                return true;
                            case EvaluationType.NoConditionsMet:
                                return false;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    switch (packet.settings.evaluationType)
                    {
                        case EvaluationType.AllConditionsMet:
                            return false;
                        case EvaluationType.AnyConditionMet:
                            continue;
                        case EvaluationType.NoConditionsMet:
                            continue;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                switch (packet.settings.evaluationType)
                {
                    case EvaluationType.AllConditionsMet:
                        return true;
                    case EvaluationType.AnyConditionMet:
                        return false;
                    case EvaluationType.NoConditionsMet:
                        return true;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        ///     Checks all the Debug Packets to see if they have to be executed.
        /// </summary>
        private void CheckDebugPackets()
        {
            using (_PRF_CheckDebugPackets.Auto())
            {
                if (debugPackets == null)
                {
                    return;
                }

                for (var i = debugPackets.Count - 1; i >= 0; i--)
                {
                    var packet = debugPackets[i];

                    packet.UpdateState();

                    if (packet is not { active: true })
                    {
                        continue;
                    }

                    if (packet.ShouldDestroy)
                    {
                        debugPackets.RemoveAt(i);
                        continue;
                    }

                    if (!packet.EligibleForEvaluation)
                    {
                        continue;
                    }

                    if (AreDebugConditionsMet(packet))
                    {
                        ExecuteOperationsInDebugPacket(packet);
                    }

                    if (packet.ShouldDestroy)
                    {
                        debugPackets.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        ///     Returns true if a condition is met.
        /// </summary>
        /// <param name="debugCondition">The condition to evaluate.</param>
        /// <returns>The true/false evaluation result.  True means the condition is met.</returns>
        private bool EvaluateDebugCondition(DebugCondition debugCondition)
        {
            using (_PRF_CheckIfConditionIsMet.Auto())
            {
                switch (debugCondition.comparisonType)
                {
                    case ComparisonType.LessThan:
                        return GetRequestedValueFromDebugVariable(debugCondition.evaluationTarget) <
                               debugCondition.value;
                    case ComparisonType.LessThanOrEqualTo:
                        return GetRequestedValueFromDebugVariable(debugCondition.evaluationTarget) <=
                               debugCondition.value;
                    case ComparisonType.Equals:
                        return Mathf.Approximately(
                            GetRequestedValueFromDebugVariable(debugCondition.evaluationTarget),
                            debugCondition.value
                        );
                    case ComparisonType.GreaterThanOrEqualTo:
                        return GetRequestedValueFromDebugVariable(debugCondition.evaluationTarget) >=
                               debugCondition.value;
                    case ComparisonType.GreaterThan:
                        return GetRequestedValueFromDebugVariable(debugCondition.evaluationTarget) >
                               debugCondition.value;

                    default:
                        return false;
                }
            }
        }

        /// <summary>
        ///     Executes the operations in the DebugPacket specified.
        /// </summary>
        /// <param name="packet"></param>
        private void ExecuteOperationsInDebugPacket(DebugConditionPacket packet)
        {
            using (_PRF_ExecuteOperationsInDebugPacket.Auto())
            {
                if (packet == null)
                {
                    return;
                }

                var settings = packet.settings;

                if (settings == null)
                {
                    return;
                }

                if (packet.settings.action.Has(ExecutionAction.Screenshot))
                {
                    _screenshotService.RequestScreenshot(_ => { });
                }

                if (packet.settings.action.Has(ExecutionAction.DebuggerBreak))
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }
                }

                if (packet.settings.action.Has(ExecutionAction.PauseEditor))
                {
                    Debug.Break();
                }

                if (packet.settings.action.Has(ExecutionAction.ModifyTimeScale))
                {
                    CoreClock.Instance.TimeScale = packet.settings.timeScale;
                }

                if (packet.settings.action.Has(ExecutionAction.LogMessage))
                {
                    var logMessage = packet.settings.logMessage;

                    switch (packet.settings.logMessageType)
                    {
                        case LogMessageType.Debug:
                            Context.Log.Debug(logMessage);
                            break;
                        case LogMessageType.Info:
                            Context.Log.Info(logMessage);
                            break;
                        case LogMessageType.Warning:
                            Context.Log.Warn(logMessage);
                            break;
                        case LogMessageType.Error:
                            Context.Log.Error(logMessage);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (packet.settings.action.Has(ExecutionAction.OpenDeveloperInterface))
                {
                    Manager.ShowAreaInterface().Forget();
                }

                packet.unityEvents?.Invoke();

                if (packet.callbacks != null)
                {
                    for (var callbackIndex = packet.callbacks.Count - 1; callbackIndex >= 0; callbackIndex--)
                    {
                        var callback = packet.callbacks[callbackIndex];

                        if (callback == null)
                        {
                            packet.callbacks.RemoveAt(callbackIndex);
                            continue;
                        }

                        try
                        {
                            callback();
                        }
                        catch (Exception ex)
                        {
                            Context.Log.Error(
                                ZString.Format(
                                    "Failed to execute callback at index {0} for debug packet {1}.",
                                    callbackIndex.FormatForLogging(),
                                    packet.settings.key.FormatNameForLogging()
                                ),
                                this,
                                ex
                            );
                        }
                    }
                }

                packet.RecordExecution();
            }
        }

        /// <summary>
        ///     Obtains the requested value from the specified variable.
        /// </summary>
        /// <param name="debugEvaluationTarget"></param>
        /// <returns></returns>
        private float GetRequestedValueFromDebugVariable(DebugEvaluationTarget debugEvaluationTarget)
        {
            using (_PRF_GetRequestedValueFromDebugVariable.Auto())
            {
                switch (debugEvaluationTarget)
                {
                    case DebugEvaluationTarget.Fps:
                        return _runtimeGraphFpsMonitor != null ? _runtimeGraphFpsMonitor.CurrentFPS : 0;
                    case DebugEvaluationTarget.Fps_Min:
                        return _runtimeGraphFpsMonitor != null ? _runtimeGraphFpsMonitor.OnePercentFPS : 0;
                    case DebugEvaluationTarget.Fps_Max:
                        return _runtimeGraphFpsMonitor != null ? _runtimeGraphFpsMonitor.Zero1PercentFps : 0;
                    case DebugEvaluationTarget.Fps_Avg:
                        return _runtimeGraphFpsMonitor != null ? _runtimeGraphFpsMonitor.AverageFPS : 0;

                    case DebugEvaluationTarget.Ram_Allocated:
                        return _runtimeGraphRamMonitor != null ? _runtimeGraphRamMonitor.AllocatedRam : 0;
                    case DebugEvaluationTarget.Ram_Reserved:
                        return _runtimeGraphRamMonitor != null ? _runtimeGraphRamMonitor.AllocatedRam : 0;
                    case DebugEvaluationTarget.Ram_Mono:
                        return _runtimeGraphRamMonitor != null ? _runtimeGraphRamMonitor.AllocatedRam : 0;

                    case DebugEvaluationTarget.Audio_DB:
                        return _runtimeGraphAudioMonitor != null ? _runtimeGraphAudioMonitor.MaxDB : 0;

                    default:
                        return 0;
                }
            }
        }

        private void SyncPackets(DebugConditionsFeatureMetadata data)
        {
            using (_PRF_SyncPackets.Auto())
            {
                for (var defaultPacketSettingIndex = 0;
                     defaultPacketSettingIndex < data.defaultPackets.Count;
                     defaultPacketSettingIndex++)
                {
                    var defaultPacketSettings = data.defaultPackets[defaultPacketSettingIndex];
                    var found = false;
                    for (var packetIndex = 0; packetIndex < debugPackets.Count; packetIndex++)
                    {
                        var existingPacket = debugPackets[packetIndex];
                        if (existingPacket.settings == defaultPacketSettings)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                    {
                        continue;
                    }

                    var newPacket = new DebugConditionPacket(defaultPacketSettings);
                    debugPackets.Add(newPacket);
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_AddCallbackToAllDebugPacketWithId =
            new ProfilerMarker(_PRF_PFX + nameof(AddCallbackToAllDebugPacketWithId));

        private static readonly ProfilerMarker _PRF_AddCallbackToFirstDebugPacketWithId =
            new ProfilerMarker(_PRF_PFX + nameof(AddCallbackToFirstDebugPacketWithId));

        private static readonly ProfilerMarker _PRF_AddNewDebugPacket =
            new ProfilerMarker(_PRF_PFX + nameof(AddNewDebugPacket));

        private static readonly ProfilerMarker _PRF_AreDebugConditionsMet =
            new ProfilerMarker(_PRF_PFX + nameof(AreDebugConditionsMet));

        private static readonly ProfilerMarker _PRF_CheckDebugPackets =
            new ProfilerMarker(_PRF_PFX + nameof(CheckDebugPackets));

        private static readonly ProfilerMarker _PRF_CheckIfConditionIsMet =
            new ProfilerMarker(_PRF_PFX + nameof(EvaluateDebugCondition));

        private static readonly ProfilerMarker _PRF_ExecuteOperationsInDebugPacket =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteOperationsInDebugPacket));

        private static readonly ProfilerMarker _PRF_GetAllDebugPacketsWithId =
            new ProfilerMarker(_PRF_PFX + nameof(GetAllDebugPacketsWithId));

        private static readonly ProfilerMarker _PRF_GetFirstDebugPacketWithId =
            new ProfilerMarker(_PRF_PFX + nameof(GetFirstDebugPacketWithId));

        private static readonly ProfilerMarker _PRF_GetRequestedValueFromDebugVariable =
            new ProfilerMarker(_PRF_PFX + nameof(GetRequestedValueFromDebugVariable));

        private static readonly ProfilerMarker _PRF_RemoveAllDebugPacketsWithId =
            new ProfilerMarker(_PRF_PFX + nameof(RemoveAllDebugPacketsWithId));

        private static readonly ProfilerMarker _PRF_RemoveFirstDebugPacketWithId =
            new ProfilerMarker(_PRF_PFX + nameof(RemoveFirstDebugPacketWithId));

        private static readonly ProfilerMarker _PRF_SyncPackets =
            new ProfilerMarker(_PRF_PFX + nameof(SyncPackets));

        #endregion
    }
}
