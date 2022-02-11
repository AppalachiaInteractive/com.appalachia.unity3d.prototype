using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Components.OnScreenButtons.Controls;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Components.OnScreenButtons.Devices
{
    [Serializable]
    public abstract class DeviceMetadata : AppalachiaObject
    {
        #region Fields and Autoproperties

        [SerializeField] public string deviceName;
        [SerializeField] public List<string> supportedDeviceSubstrings;
        [SerializeField] public ControlButtonSpriteSet controller;
        [SerializeField] public ControlButtonSpriteSet lines;
        [SerializeField] public ControlButtonSpriteSet buttons;

        [NonSerialized] protected ControlButtonMetadata[] _controls;

        #endregion

        public abstract bool CanResolve(InputControl control);

        public abstract IEnumerable<ControlButtonMetadata> GetAll();

        public abstract ControlButtonMetadata Resolve(InputControl control);

        public bool CanResolve(InputDevice device)
        {
            if (deviceName == device.name)
            {
                return true;
            }

            supportedDeviceSubstrings ??= new List<string>();

            foreach (var supportedDeviceSubstring in supportedDeviceSubstrings)
            {
                if (device.name.Contains(supportedDeviceSubstring))
                {
                    return true;
                }
            }

            return false;
        }
#if UNITY_EDITOR

        internal abstract void PopulateAll();

#endif

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

#if UNITY_EDITOR
            PopulateAll();
#endif
            foreach (var control in _controls)
            {
                control.device = this;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(DeviceMetadata) + ".";

        private static readonly ProfilerMarker _PRF_CanResolve =
            new ProfilerMarker(_PRF_PFX + nameof(CanResolve));

        private static readonly ProfilerMarker _PRF_Resolve = new ProfilerMarker(_PRF_PFX + nameof(Resolve));

        #endregion
    }
}
