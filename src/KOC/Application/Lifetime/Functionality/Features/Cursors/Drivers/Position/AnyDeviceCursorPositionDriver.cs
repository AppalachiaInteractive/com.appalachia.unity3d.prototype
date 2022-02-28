using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Drivers.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.State.Contracts;
using Appalachia.Utility.Execution;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Drivers.Position
{
    public class AnyDeviceCursorPositionDriver : CursorPositionDriver<AnyDeviceCursorPositionDriver>,
                                                 ICursorPositionDriver
    {
        public AnyDeviceCursorPositionDriver()
        {
        }

        public AnyDeviceCursorPositionDriver(Object owner) : base(owner)
        {
        }

        #region ICursorPositionDriver Members

        /// <inheritdoc />
        public override void DriveCursorPosition(
            IReadOnlyCursorInstanceStateData stateData,
            float elapsed,
            float deltaTime,
            Rect bounds,
            Vector2 normalizedPositionInBounds,
            Vector2 size,
            Vector2 center,
            out Vector2? newPosition,
            out bool? shouldLock)
        {
            using (_PRF_DriveCursorPosition.Auto())
            {
                newPosition = null;
                shouldLock = false;

                for (var deviceIndex = 0; deviceIndex < InputSystem.devices.Count; deviceIndex++)
                {
                    var device = InputSystem.devices[deviceIndex];

                    if (AppalachiaApplication.IsPlaying && !device.wasUpdatedThisFrame)
                    {
                        continue;
                    }

                    if (device is Mouse m)
                    {
                        if (CanGetPosition(m, out newPosition))
                        {
                            return;
                        }
                    }
                    else if (device is Pointer p && device.wasUpdatedThisFrame)
                    {
                        if (CanGetPosition(p, out newPosition))
                        {
                            return;
                        }
                    }
                    else if (device is Gamepad g)
                    {
                        if (CanAlterPosition(g.leftStick, stateData.CurrentPosition, out newPosition))
                        {
                            return;
                        }

                        if (CanAlterPosition(g.rightStick, stateData.CurrentPosition, out newPosition))
                        {
                            return;
                        }
                    }
                    else if (device is Joystick j)
                    {
                        if (CanAlterPosition(j.stick, stateData.CurrentPosition, out newPosition))
                        {
                            return;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
