using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Components.OnScreenButtons.Controls;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Components.OnScreenButtons.Devices
{
    [DoNotReorderFields]
    [Serializable, SmartLabelChildren, SmartLabel]
    public sealed class GamepadMetadata : DeviceMetadata
    {
        #region Fields and Autoproperties

        public ControlButtonMetadata dpad;
        public ControlButtonMetadata dpad_up;
        public ControlButtonMetadata dpad_down;
        public ControlButtonMetadata dpad_left;
        public ControlButtonMetadata dpad_right;
        public ControlButtonMetadata start;
        public ControlButtonMetadata select;
        public ControlButtonMetadata leftStick;
        public ControlButtonMetadata rightStick;
        public ControlButtonMetadata leftStickPress;
        public ControlButtonMetadata rightStickPress;
        public ControlButtonMetadata leftShoulder;
        public ControlButtonMetadata rightShoulder;
        public ControlButtonMetadata leftTrigger;
        public ControlButtonMetadata rightTrigger;
        public ControlButtonMetadata buttonSouth;
        public ControlButtonMetadata buttonEast;
        public ControlButtonMetadata buttonWest;
        public ControlButtonMetadata buttonNorth;

        #endregion

        /// <inheritdoc />
        public override bool CanResolve(InputControl control)
        {
            switch (control.name)
            {
                case "dpad":
                case "dpad/up":
                case "dpad/down":
                case "dpad/left":
                case "dpad/right":
                case "start":
                case "select":
                case "leftStick":
                case "rightStick":
                case "leftStickPress":
                case "rightStickPress":
                case "leftShoulder":
                case "rightShoulder":
                case "leftTrigger":
                case "rightTrigger":
                case "buttonSouth":
                case "buttonEast":
                case "buttonWest":
                case "buttonNorth":
                    return true;
                default:
                    return false;
            }
        }

        /// <inheritdoc />
        public override IEnumerable<ControlButtonMetadata> GetAll()
        {
            /*if (_controls != null)
            {
                return _controls;
            }*/

            _controls = new[]
            {
                dpad,
                dpad_up,
                dpad_down,
                dpad_left,
                dpad_right,
                start,
                select,
                leftStickPress,
                rightStickPress,
                leftShoulder,
                rightShoulder,
                buttonSouth,
                buttonEast,
                buttonWest,
                buttonNorth,
                leftStick,
                rightStick,
                leftTrigger,
                rightTrigger,
            };

            return _controls;
        }

        /// <inheritdoc />
        public override ControlButtonMetadata Resolve(InputControl control)
        {
            return control.name switch
            {
                "dpad"            => dpad,
                "dpad/up"         => dpad_up,
                "dpad/down"       => dpad_down,
                "dpad/left"       => dpad_left,
                "dpad/right"      => dpad_right,
                "start"           => start,
                "select"          => select,
                "leftStickPress"  => leftStickPress,
                "rightStickPress" => rightStickPress,
                "leftShoulder"    => leftShoulder,
                "rightShoulder"   => rightShoulder,
                "buttonSouth"     => buttonSouth,
                "buttonEast"      => buttonEast,
                "buttonWest"      => buttonWest,
                "buttonNorth"     => buttonNorth,
                "leftStick"       => leftStick,
                "rightStick"      => rightStick,
                "leftTrigger"     => leftTrigger,
                "rightTrigger"    => rightTrigger,
                _                 => throw new NotSupportedException(control.name)
            };
        }
#if UNITY_EDITOR

        /// <inheritdoc />
        internal override void PopulateAll()
        {
            using (_PRF_SetAll.Auto())
            {
                _ = GetAll();

                if (deviceName == null)
                {
                    Context.Log.Warn("This metadata is not yet setup!");
                    return;
                }

                if (dpad == null)
                {
                    dpad = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(dpad))
                    );
                    MarkAsModified();
                    _controls[0] = dpad;
                }

                if (dpad_up == null)
                {
                    dpad_up = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(dpad_up))
                    );
                    MarkAsModified();
                    _controls[1] = dpad_up;
                }

                if (dpad_down == null)
                {
                    dpad_down = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(dpad_down))
                    );
                    MarkAsModified();
                    _controls[2] = dpad_down;
                }

                if (dpad_left == null)
                {
                    dpad_left = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(dpad_left))
                    );
                    MarkAsModified();
                    _controls[3] = dpad_left;
                }

                if (dpad_right == null)
                {
                    dpad_right = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(dpad_right))
                    );
                    MarkAsModified();
                    _controls[4] = dpad_right;
                }

                if (start == null)
                {
                    start = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(start))
                    );
                    MarkAsModified();
                    _controls[5] = start;
                }

                if (select == null)
                {
                    select = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(select))
                    );
                    MarkAsModified();
                    _controls[6] = select;
                }

                if (leftStickPress == null)
                {
                    leftStickPress = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(leftStickPress))
                    );
                    MarkAsModified();
                    _controls[7] = leftStickPress;
                }

                if (rightStickPress == null)
                {
                    rightStickPress = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(rightStickPress))
                    );
                    MarkAsModified();
                    _controls[8] = rightStickPress;
                }

                if (leftShoulder == null)
                {
                    leftShoulder = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(leftShoulder))
                    );
                    MarkAsModified();
                    _controls[9] = leftShoulder;
                }

                if (rightShoulder == null)
                {
                    rightShoulder = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(rightShoulder))
                    );
                    MarkAsModified();
                    _controls[10] = rightShoulder;
                }

                if (buttonSouth == null)
                {
                    buttonSouth = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(buttonSouth))
                    );
                    MarkAsModified();
                    _controls[11] = buttonSouth;
                }

                if (buttonEast == null)
                {
                    buttonEast = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(buttonEast))
                    );
                    MarkAsModified();
                    _controls[12] = buttonEast;
                }

                if (buttonWest == null)
                {
                    buttonWest = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(buttonWest))
                    );
                    MarkAsModified();
                    _controls[13] = buttonWest;
                }

                if (buttonNorth == null)
                {
                    buttonNorth = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(buttonNorth))
                    );
                    MarkAsModified();
                    _controls[14] = buttonNorth;
                }

                if (leftStick == null)
                {
                    leftStick = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(leftStick))
                    );
                    MarkAsModified();
                    _controls[15] = leftStick;
                }

                if (rightStick == null)
                {
                    rightStick = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(rightStick))
                    );
                    MarkAsModified();
                    _controls[16] = rightStick;
                }

                if (leftTrigger == null)
                {
                    leftTrigger = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(leftTrigger))
                    );
                    MarkAsModified();
                    _controls[17] = leftTrigger;
                }

                if (rightTrigger == null)
                {
                    rightTrigger = ControlButtonMetadata.LoadOrCreateNew(
                        ZString.Format("{0}/{1}", deviceName, nameof(rightTrigger))
                    );
                    MarkAsModified();
                    _controls[18] = rightTrigger;
                }
            }
        }
#endif

        #region Menu Items

#if UNITY_EDITOR
        [UnityEditor.MenuItem(
            PKG.Menu.Assets.Base + nameof(GamepadMetadata),
            priority = PKG.Menu.Assets.Priority
        )]
        public static void CreateAsset()
        {
            CreateNew<GamepadMetadata>();
        }
#endif

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(GamepadMetadata) + ".";

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
