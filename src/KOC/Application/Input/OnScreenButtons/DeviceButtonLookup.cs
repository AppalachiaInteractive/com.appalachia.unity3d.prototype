using Appalachia.CI.Integration.Assets;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Scriptables;
using Appalachia.Prototype.KOC.Application.Extensions;
using Appalachia.Prototype.KOC.Application.Input.OnScreenButtons.Collections;
using Appalachia.Prototype.KOC.Application.Input.OnScreenButtons.Controls;
using Appalachia.Prototype.KOC.Application.Input.OnScreenButtons.Devices;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Application.Input.OnScreenButtons
{
    [SmartLabelChildren, SmartLabel]
    public class DeviceButtonLookup : SingletonAppalachiaObject<DeviceButtonLookup>
    {
        #region Fields and Autoproperties

        public KeyboardMetadata keyboard;
        public MouseMetadata mouse;
        public AppaList_GamepadMetadata gamepads;

#if UNITY_EDITOR
        public string assetRootPath;
#endif

        #endregion

#if UNITY_EDITOR
        private bool _canFindSprites => assetRootPath != null;

#endif

        #region Event Functions

        protected override void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                base.Awake();

                Initialize();
            }
        }

        protected override void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                base.OnEnable();

                Initialize();
            }
        }

        #endregion

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();
            }
        }

        public bool CanResolve(OnScreenButtonMetadata metadata, out InputControl control, out DeviceMetadata resolver)
        {
            using (_PRF_CanResolve.Auto())
            {
                foreach(var iterator in metadata.actionReference.action.controls)
                {
                    control = iterator;

                    if (CanResolve(control, out resolver))
                    {
                        return true;
                    }
                }

                control = null;
                resolver = null;
                return false;
            }
        }

        public bool CanResolve(InputControl control, out DeviceMetadata resolver)
        {
            using (_PRF_CanResolve.Auto())
            {
                if (keyboard.CanResolve(control.device) && keyboard.CanResolve(control))
                {
                    resolver = keyboard;
                    return true;
                }

                if (mouse.CanResolve(control.device) && mouse.CanResolve(control))
                {
                    resolver = mouse;
                    return true;
                }

                for (var i = 0; i < gamepads.Count; i++)
                {
                    var gamepad = gamepads[i];

                    if (gamepad.CanResolve(control.device) && gamepad.CanResolve(control))
                    {
                        resolver = gamepad;
                        return true;
                    }
                }

                LogFailure(control);
                resolver = null;
                return false;
            }
        }

        public ControlButtonMetadata GetBestControl(OnScreenButtonMetadata metadata)
        {
            using (_PRF_GetBestControl.Auto())
            {
                foreach (var binding in metadata.actionReference.action.bindings)
                {
                    var path = binding.effectivePath;

                    var control = InputSystem.FindControl(path);

                    if (control == null)
                    {
                        continue;
                    }

                    if (!control.device.enabled)
                    {
                        continue;
                    }

                    if (!CanResolve(control, out _))
                    {
                        continue;
                    }

                    return Resolve(control);
                }

                return null;
            }
        }

        public ControlButtonMetadata Resolve(OnScreenButtonMetadata buttonMetadata)
        {
            using (_PRF_Resolve.Auto())
            {
                if (CanResolve(buttonMetadata, out var control, out var device))
                {
                    return Resolve(control, device);
                }

                LogFailure(buttonMetadata);

                return null;
            }
        }

        public ControlButtonMetadata Resolve(InputControl control)
        {
            using (_PRF_Resolve.Auto())
            {
                if (CanResolve(control, out var device))
                {
                    return Resolve(control, device);
                }

                LogFailure(control);

                return null;
            }
        }

        public ControlButtonMetadata Resolve(InputControl control, DeviceMetadata device)
        {
            using (_PRF_Resolve.Auto())
            {
                return device.Resolve(control);
            }
        }

        private void LogFailure(OnScreenButtonMetadata control)
        {
            using (_PRF_LogFailure.Auto())
            {
                var actionName = control.actionReference.ToFormattedName();
                
                AppaLog.Error(
                    $"Could not resolve {{ action: {actionName} }}"
                );
            }
        }

        private void LogFailure(InputControl control)
        {
            using (_PRF_LogFailure.Auto())
            {
                var deviceName = control?.device?.name ?? "<null>";
                var controlName = control?.name ?? "<null>";
                var controlPath = control?.path ?? "<null>";
                
                AppaLog.Error(
                    $"Could not resolve {{ device: {deviceName}, control:{controlName}, path:{controlPath} }}"
                );
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetBestControl = new ProfilerMarker(_PRF_PFX + nameof(GetBestControl));

        private const string _PRF_PFX = nameof(DeviceButtonLookup) + ".";

        private static readonly ProfilerMarker _PRF_CanResolve =
            new ProfilerMarker(_PRF_PFX + nameof(CanResolve));

        private static readonly ProfilerMarker _PRF_LogFailure =
            new ProfilerMarker(_PRF_PFX + nameof(LogFailure));

        private static readonly ProfilerMarker _PRF_Resolve = new ProfilerMarker(_PRF_PFX + nameof(Resolve));

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion

#if UNITY_EDITOR
        
        [Button]
        public void Sync()
        {
            using (_PRF_Sync.Auto())
            {
                using (new AssetEditingScope())
                {
                    foreach (var control in Keyboard.current.allControls)
                    {
                        if (!keyboard.CanResolve(control))
                        {
                            AppaLog.Warn($"Keyboard can not resolve key {control.name}");
                            continue;
                        }

                        var metadata = keyboard.Resolve(control);
                        metadata.data = new ControlInfoData(control);
                        metadata.device = keyboard;
                    }

                    foreach (var control in Mouse.current.allControls)
                    {
                        if (!mouse.CanResolve(control))
                        {
                            AppaLog.Warn($"Keyboard can not resolve key {control.name}");
                            continue;
                        }
                        
                        var metadata = mouse.Resolve(control);
                        metadata.data = new ControlInfoData(control);
                        metadata.device = mouse;
                    }

                    foreach (var realGamepad in Gamepad.all)
                    {
                        foreach (var gamepad in gamepads)
                        {
                            foreach (var control in realGamepad.allControls)
                            {
                                if (gamepad.CanResolve(control))
                                {
                                    var metadata = gamepad.Resolve(control);
                                    metadata.data = new ControlInfoData(control);
                                    metadata.device = gamepad;
                                }
                            }
                        }
                    }
                }
            }
        }
        
        [Button]
        [EnableIf(nameof(_canFindSprites))]
        public void FindSprites()
        {
            using (_PRF_FindSprites.Auto())
            {
                Sync();

                foreach (var control in keyboard.GetAll())
                {
                    control.device = keyboard;
                    control.FindSprites(assetRootPath);
                }

                foreach (var control in mouse.GetAll())
                {
                    control.device = mouse;
                    control.FindSprites(assetRootPath);
                }

                foreach (var gamepad in gamepads)
                {
                    foreach (var control in gamepad.GetAll())
                    {
                        control.device = gamepad;
                        control.FindSprites(assetRootPath);
                    }
                }
            }
        }

        [Button]
        public void Populate()
        {
            using (_PRF_Populate.Auto())
            {
                using (new AssetEditingScope())
                {
                    if (keyboard == null)
                    {
                        keyboard = LoadOrCreateNew<KeyboardMetadata>(nameof(KeyboardMetadata));
                    }

                    keyboard.deviceName = Keyboard.current.device.name;

                    if (mouse == null)
                    {
                        mouse = LoadOrCreateNew<MouseMetadata>(nameof(MouseMetadata));
                    }

                    mouse.deviceName = Mouse.current.device.name;

                    keyboard.PopulateAll();
                    mouse.PopulateAll();

                    if (gamepads == null)
                    {
                        gamepads = new AppaList_GamepadMetadata();
                    }

                    foreach (var gamepad in gamepads)
                    {
                        gamepad.PopulateAll();
                    }

                    this.MarkAsModified();
                }
            }
        }

        private static readonly ProfilerMarker
            _PRF_Populate = new ProfilerMarker(_PRF_PFX + nameof(Populate));

        private static readonly ProfilerMarker _PRF_Sync = new ProfilerMarker(_PRF_PFX + nameof(Sync));

        private static readonly ProfilerMarker _PRF_FindSprites =
            new ProfilerMarker(_PRF_PFX + nameof(FindSprites));
#endif
    }
}
