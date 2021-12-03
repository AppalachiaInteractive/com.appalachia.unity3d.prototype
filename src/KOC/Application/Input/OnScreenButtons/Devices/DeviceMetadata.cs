using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Application.Input.OnScreenButtons.Controls;
using Appalachia.Prototype.KOC.Application.Scriptables;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Application.Input.OnScreenButtons.Devices
{
    [Serializable, SmartLabelChildren, SmartLabel]
    public abstract class DeviceMetadata : AppalachiaApplicationObject
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

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();
                
                PopulateAll();

                foreach (var control in _controls)
                {
                    control.device = this;
                }
            }
        }

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

        internal abstract void PopulateAll();

        #region Profiling

        private const string _PRF_PFX = nameof(DeviceMetadata) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_CanResolve =
            new ProfilerMarker(_PRF_PFX + nameof(CanResolve));

        private static readonly ProfilerMarker _PRF_Resolve = new ProfilerMarker(_PRF_PFX + nameof(Resolve));

        #endregion
    }
}
