using System.Collections.Generic;
using System.Linq;
using Appalachia.Core.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Audio;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Fps;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Ram;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Settings;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs.Util;
using Appalachia.Utility.Async;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Execution;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Debugging.RuntimeGraphs
{
    [CallStaticConstructorInEditor]
    [ExecutionOrder(ExecutionOrders.RuntimeGraph)]
    public class GraphyManager : SingletonAppalachiaBehaviour<GraphyManager>
    {
        static GraphyManager()
        {
            RuntimeGraphSettings.InstanceAvailable += i => _runtimeGraphSettings = i;
        }

        #region Static Fields and Autoproperties

        private static List<ModulePreset> modulePresets;

        private static RuntimeGraphSettings _runtimeGraphSettings;

        #endregion

        #region Fields and Autoproperties

        private bool _initialized;
        private bool _active = true;
        private bool _focused = true;

        private G_FpsManager _fpsManager;
        private G_RamManager _ramManager;
        private G_AudioManager _audioManager;

        private G_FpsMonitor _fpsMonitor;
        private G_RamMonitor _ramMonitor;
        private G_AudioMonitor _audioMonitor;

        [InlineEditor, SmartLabel, SmartLabelChildren]
        public RuntimeGraphSettings settings;

        public AudioListener AudioListener;

        public ModulePreset currentPreset;

        public ModuleState fpsModuleState;
        public ModuleState ramModuleState;
        public ModuleState audioModuleState;

        #endregion

        #region Event Functions

        private void OnApplicationFocus(bool isFocused)
        {
            using (_PRF_OnApplicationFocus.Auto())
            {
                _focused = isFocused;

                if (_initialized && isFocused)
                {
                    RefreshAllParameters();
                }
            }
        }

        #endregion

        public void CycleThroughPresets()
        {
            using (_PRF_ToggleModes.Auto())
            {
                if (modulePresets == null)
                {
                    modulePresets = EnumValueManager.GetAllValues<ModulePreset>().ToList();
                }

                var currentPresetIndex = modulePresets.IndexOf(currentPreset);

                currentPresetIndex += 1;

                if (currentPresetIndex >= modulePresets.Count)
                {
                    currentPresetIndex = 0;
                }

                currentPreset = modulePresets[currentPresetIndex];

                SetPreset();
            }
        }

        public void SetPreset()
        {
            using (_PRF_SetPreset.Auto())
            {
                switch (currentPreset)
                {
                    case ModulePreset.FPS_BASIC:
                        fpsModuleState = ModuleState.BASIC;
                        ramModuleState = ModuleState.OFF;
                        audioModuleState = ModuleState.OFF;
                        break;

                    case ModulePreset.FPS_TEXT:
                        fpsModuleState = ModuleState.TEXT;
                        ramModuleState = ModuleState.OFF;
                        audioModuleState = ModuleState.OFF;
                        break;

                    case ModulePreset.FPS_FULL:
                        fpsModuleState = ModuleState.FULL;
                        ramModuleState = ModuleState.OFF;
                        audioModuleState = ModuleState.OFF;
                        break;

                    case ModulePreset.FPS_TEXT_RAM_TEXT:
                        fpsModuleState = ModuleState.TEXT;
                        ramModuleState = ModuleState.TEXT;
                        audioModuleState = ModuleState.OFF;
                        break;

                    case ModulePreset.FPS_FULL_RAM_TEXT:
                        fpsModuleState = ModuleState.FULL;
                        ramModuleState = ModuleState.TEXT;
                        audioModuleState = ModuleState.OFF;
                        break;

                    case ModulePreset.FPS_FULL_RAM_FULL:
                        fpsModuleState = ModuleState.FULL;
                        ramModuleState = ModuleState.FULL;
                        audioModuleState = ModuleState.OFF;
                        break;

                    case ModulePreset.FPS_TEXT_RAM_TEXT_AUDIO_TEXT:
                        fpsModuleState = ModuleState.TEXT;
                        ramModuleState = ModuleState.TEXT;
                        audioModuleState = ModuleState.TEXT;
                        break;

                    case ModulePreset.FPS_FULL_RAM_TEXT_AUDIO_TEXT:
                        fpsModuleState = ModuleState.FULL;
                        ramModuleState = ModuleState.TEXT;
                        audioModuleState = ModuleState.TEXT;
                        break;

                    case ModulePreset.FPS_FULL_RAM_FULL_AUDIO_TEXT:
                        fpsModuleState = ModuleState.FULL;
                        ramModuleState = ModuleState.FULL;
                        audioModuleState = ModuleState.TEXT;
                        break;

                    case ModulePreset.FPS_FULL_RAM_FULL_AUDIO_FULL:
                        fpsModuleState = ModuleState.FULL;
                        ramModuleState = ModuleState.FULL;
                        audioModuleState = ModuleState.FULL;
                        break;

                    default:
                        Context.Log.Warn("Tried to set a preset that is not supported.");
                        break;
                }

                UpdateManagerState();
            }
        }

        public void ToggleActive()
        {
            using (_PRF_ToggleActive.Auto())
            {
                if (!_active)
                {
                    ToggleOn();
                }
                else
                {
                    ToggleOff();
                }
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                if (settings == null)
                {
                    settings = _runtimeGraphSettings;
                }

                currentPreset = settings.general.modulePresetState;

                if (settings.general.keepAlive && AppalachiaApplication.IsPlaying)
                {
                    DontDestroyOnLoad(transform.root.gameObject);
                }

                _fpsMonitor = GetComponentInChildren(typeof(G_FpsMonitor),     true) as G_FpsMonitor;
                _ramMonitor = GetComponentInChildren(typeof(G_RamMonitor),     true) as G_RamMonitor;
                _audioMonitor = GetComponentInChildren(typeof(G_AudioMonitor), true) as G_AudioMonitor;

                _fpsManager = GetComponentInChildren(typeof(G_FpsManager),     true) as G_FpsManager;
                _ramManager = GetComponentInChildren(typeof(G_RamManager),     true) as G_RamManager;
                _audioManager = GetComponentInChildren(typeof(G_AudioManager), true) as G_AudioManager;

                _fpsManager.SetPosition(settings.general.graphModulePosition);
                _ramManager.SetPosition(settings.general.graphModulePosition);
                _audioManager.SetPosition(settings.general.graphModulePosition);

                UpdateManagerState();

                if (settings.general.enableOnStartup)
                {
                    GetComponent<Canvas>().enabled = true;
                }

                _initialized = true;
            }
        }

        protected override async AppaTask WhenDestroyed()
        {
            await base.WhenDestroyed();
            G_IntString.Dispose();
            G_FloatString.Dispose();
        }

        private void RefreshAllParameters()
        {
            using (_PRF_RefreshAllParameters.Auto())
            {
                _fpsManager.UpdateParameters();
                _ramManager.UpdateParameters();
                _audioManager.UpdateParameters();
            }
        }

        private void ToggleOff()
        {
            using (_PRF_ToggleOff.Auto())
            {
                if (_active)
                {
                    fpsModuleState = ModuleState.OFF;
                    ramModuleState = ModuleState.OFF;
                    audioModuleState = ModuleState.OFF;

                    UpdateManagerState();

                    _active = false;
                }
            }
        }

        private void ToggleOn()
        {
            using (_PRF_ToggleOn.Auto())
            {
                _active = true;

                if (_initialized)
                {
                    _fpsManager.RestorePreviousState();
                    _ramManager.RestorePreviousState();
                    _audioManager.RestorePreviousState();
                }
            }
        }

        private void UpdateManagerState()
        {
            using (_PRF_UpdateManagerState.Auto())
            {
                _fpsManager.SetState(fpsModuleState);
                _ramManager.SetState(ramModuleState);
                _audioManager.SetState(audioModuleState);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(GraphyManager) + ".";

        private static readonly ProfilerMarker _PRF_ToggleOff =
            new ProfilerMarker(_PRF_PFX + nameof(ToggleOff));

        private static readonly ProfilerMarker
            _PRF_ToggleOn = new ProfilerMarker(_PRF_PFX + nameof(ToggleOn));

        private static readonly ProfilerMarker _PRF_UpdateManagerState =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateManagerState));

        private static readonly ProfilerMarker _PRF_OnApplicationFocus =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplicationFocus));

        private static readonly ProfilerMarker _PRF_ToggleActive =
            new ProfilerMarker(_PRF_PFX + nameof(ToggleActive));

        private static readonly ProfilerMarker _PRF_ToggleModes =
            new ProfilerMarker(_PRF_PFX + nameof(CycleThroughPresets));

        private static readonly ProfilerMarker _PRF_SetPreset =
            new ProfilerMarker(_PRF_PFX + nameof(SetPreset));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_RefreshAllParameters =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshAllParameters));

        #endregion
    }
}
