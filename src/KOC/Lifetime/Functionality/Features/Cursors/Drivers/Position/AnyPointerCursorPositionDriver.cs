using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Drivers.Contracts;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State.Contracts;
using Appalachia.Utility.Execution;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Drivers.Position
{
    public class AnyPointerCursorPositionDriver : CursorPositionDriver<AnyPointerCursorPositionDriver>,
                                                  ICursorPositionDriver
    {
        public AnyPointerCursorPositionDriver()
        {
        }

        public AnyPointerCursorPositionDriver(Object owner) : base(owner)
        {
        }

        #region ICursorPositionDriver Members

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
                }
            }
        }

        #endregion
    }
}
