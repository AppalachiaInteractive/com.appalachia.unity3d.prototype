using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Components.OnScreenButtons.Controls;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Components.OnScreenButtons.Devices
{
    [DoNotReorderFields]
    [Serializable, SmartLabelChildren, SmartLabel]
    public sealed class MouseMetadata : DeviceMetadata
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("Scroll")]
        public ControlButtonMetadata scroll;

        public ControlButtonMetadata leftButton;
        public ControlButtonMetadata rightButton;
        public ControlButtonMetadata middleButton;
        public ControlButtonMetadata forwardButton;
        public ControlButtonMetadata backButton;

        #endregion

        public override bool CanResolve(InputControl control)
        {
            using (_PRF_CanResolve.Auto())
            {
                switch (control.name)
                {
                    case "scroll":
                    case "leftButton":
                    case "rightButton":
                    case "middleButton":
                    case "forwardButton":
                    case "backButton":
                        return true;
                    default:
                        return false;
                }
            }
        }

        public override IEnumerable<ControlButtonMetadata> GetAll()
        {
            if (_controls != null)
            {
                return _controls;
            }

            _controls = new[] { scroll, leftButton, rightButton, middleButton, forwardButton, backButton, };

            return _controls;
        }

        public override ControlButtonMetadata Resolve(InputControl control)
        {
            using (_PRF_Resolve.Auto())
            {
                switch (control.name)
                {
                    case "scroll":
                        return scroll;
                    case "leftButton":
                        return leftButton;
                    case "rightButton":
                        return rightButton;
                    case "middleButton":
                        return middleButton;
                    case "forwardButton":
                        return forwardButton;
                    case "backButton":
                        return backButton;

                    default:
                        throw new NotSupportedException(control.name);
                }
            }
        }
#if UNITY_EDITOR

        internal override void PopulateAll()
        {
            using (_PRF_SetAll.Auto())
            {
                _ = GetAll();

                if (scroll == null)
                {
                    scroll = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(scroll))
                    );
                    MarkAsModified();
                    _controls[0] = scroll;
                }

                if (leftButton == null)
                {
                    leftButton = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(leftButton))
                    );
                    MarkAsModified();
                    _controls[1] = leftButton;
                }

                if (rightButton == null)
                {
                    rightButton = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(rightButton))
                    );
                    MarkAsModified();
                    _controls[2] = rightButton;
                }

                if (middleButton == null)
                {
                    middleButton = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(middleButton))
                    );
                    MarkAsModified();
                    _controls[3] = middleButton;
                }

                if (forwardButton == null)
                {
                    forwardButton = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(forwardButton))
                    );
                    MarkAsModified();
                    _controls[4] = forwardButton;
                }

                if (backButton == null)
                {
                    backButton = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(backButton))
                    );
                    MarkAsModified();
                    _controls[5] = backButton;
                }
            }
        }
#endif

        #region Profiling

        private const string _PRF_PFX = nameof(MouseMetadata) + ".";

        private static readonly ProfilerMarker _PRF_SetAll =
            new ProfilerMarker(_PRF_PFX + nameof(PopulateAll));

        private static readonly ProfilerMarker _PRF_CanResolve =
            new ProfilerMarker(_PRF_PFX + nameof(CanResolve));

        private static readonly ProfilerMarker _PRF_Resolve = new ProfilerMarker(_PRF_PFX + nameof(Resolve));

        #endregion

#if UNITY_EDITOR

#endif
    }
}
