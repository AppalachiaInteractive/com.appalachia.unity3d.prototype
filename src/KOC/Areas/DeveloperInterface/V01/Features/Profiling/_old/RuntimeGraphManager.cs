using System.Collections.Generic;
using System.Linq;
using Appalachia.Core.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Audio;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Fps;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Ram;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Settings;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old.Util;
using Appalachia.Utility.Async;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Execution;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Profiling._old
{
    [CallStaticConstructorInEditor]
    [ExecutionOrder(ExecutionOrders.RuntimeGraph)]
    public class RuntimeGraphManager : SingletonAppalachiaBehaviour<RuntimeGraphManager>
    {
        static RuntimeGraphManager()
        {
            RegisterDependency<RuntimeGraphSettings>(i => _runtimeGraphSettings = i);
            RegisterDependency<RuntimeGraphFpsManager>(i => _fpsManager = i);
            RegisterDependency<RuntimeGraphRamManager>(i => _ramManager = i);
            RegisterDependency<RuntimeGraphAudioManager>(i => _audioManager = i);
            RegisterDependency<RuntimeGraphFpsMonitor>(i => _fpsMonitor = i);
            RegisterDependency<RuntimeGraphRamMonitor>(i => _ramMonitor = i);
            RegisterDependency<RuntimeGraphAudioMonitor>(i => _audioMonitor = i);
        }

        #region Static Fields and Autoproperties

        private static List<ModulePreset> modulePresets;

        private static RuntimeGraphAudioManager _audioManager;
        private static RuntimeGraphAudioMonitor _audioMonitor;

        private static RuntimeGraphFpsManager _fpsManager;
        private static RuntimeGraphFpsMonitor _fpsMonitor;
        private static RuntimeGraphRamManager _ramManager;
        private static RuntimeGraphRamMonitor _ramMonitor;

        private static RuntimeGraphSettings _runtimeGraphSettings;

        #endregion

        #region Fields and Autoproperties

        private bool _active = true;
        private bool _focused = true;

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

                if (DependenciesAreReady && FullyInitialized && isFocused)
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

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
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

            _fpsManager.SetPosition(settings.general.graphModulePosition);
            _ramManager.SetPosition(settings.general.graphModulePosition);
            _audioManager.SetPosition(settings.general.graphModulePosition);

            UpdateManagerState();

            if (settings.general.enableOnStartup)
            {
                GetComponent<Canvas>().enabled = true;
            }

            _fpsManager.InitializeParameters();
            _ramManager.InitializeParameters();
            _audioManager.InitializeParameters();
        }

        /// <inheritdoc />
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
                if (_fpsManager != null)
                {
                    _fpsManager.UpdateParameters();
                }

                if (_ramManager != null)
                {
                    _ramManager.UpdateParameters();
                }

                if (_audioManager != null)
                {
                    _audioManager.UpdateParameters();
                }
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

                if (DependenciesAreReady && FullyInitialized)
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

        private static readonly ProfilerMarker _PRF_OnApplicationFocus =
            new ProfilerMarker(_PRF_PFX + nameof(OnApplicationFocus));

        private static readonly ProfilerMarker _PRF_RefreshAllParameters =
            new ProfilerMarker(_PRF_PFX + nameof(RefreshAllParameters));

        private static readonly ProfilerMarker _PRF_SetPreset =
            new ProfilerMarker(_PRF_PFX + nameof(SetPreset));

        private static readonly ProfilerMarker _PRF_ToggleActive =
            new ProfilerMarker(_PRF_PFX + nameof(ToggleActive));

        private static readonly ProfilerMarker _PRF_ToggleModes =
            new ProfilerMarker(_PRF_PFX + nameof(CycleThroughPresets));

        private static readonly ProfilerMarker _PRF_ToggleOff =
            new ProfilerMarker(_PRF_PFX + nameof(ToggleOff));

        private static readonly ProfilerMarker
            _PRF_ToggleOn = new ProfilerMarker(_PRF_PFX + nameof(ToggleOn));

        private static readonly ProfilerMarker _PRF_UpdateManagerState =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateManagerState));

        #endregion
    }
}
