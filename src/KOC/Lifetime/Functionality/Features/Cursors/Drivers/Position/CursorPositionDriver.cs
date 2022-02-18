using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Drivers.Contracts;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State.Contracts;
using Appalachia.Utility.Execution;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Drivers.Position
{
    [CallStaticConstructorInEditor]
    [NonSerializable]
    public abstract class CursorPositionDriver<T> : AppalachiaBase<T>, ICursorPositionDriver
        where T : CursorPositionDriver<T>, new()
    {
        static CursorPositionDriver()
        {
            When.Behaviour<LifetimeComponentManager>().IsAvailableThen(i => _lifetimeComponentManager = i);
        }

        protected CursorPositionDriver()
        {
        }

        protected CursorPositionDriver(Object owner) : base(owner)
        {
        }

        #region Static Fields and Autoproperties

        private static LifetimeComponentManager _lifetimeComponentManager;

        #endregion

        protected LifetimeComponentManager LifetimeComponentManager => _lifetimeComponentManager;

        protected bool CanAlterPosition(
            StickControl control,
            Vector2 currentPosition,
            out Vector2? position,
            bool allowed = true,
            float sensitivity = 1.0f)
        {
            using (_PRF_CanAlterPosition.Auto())
            {
                position = null;

                if ((control == null) || !allowed)
                {
                    return false;
                }

                var delta = control.ReadValue();

                if (delta.x is > -float.Epsilon and < float.Epsilon &&
                    delta.y is > -float.Epsilon and < float.Epsilon)
                {
                    return false;
                }

                position = currentPosition;

                position += sensitivity * control.ReadValue();

                return true;
            }
        }

        protected bool CanGetPosition(Pointer device, out Vector2? position)
        {
            using (_PRF_CanGetPosition.Auto())
            {
                position = null;

                if (device == null)
                {
                    return false;
                }

                if (AppalachiaApplication.IsPlayingOrWillPlay && !device.wasUpdatedThisFrame)
                {
                    return false;
                }

                var devicePosition = device.position.ReadValue();

                position = _lifetimeComponentManager.GetPositionInScaledCanvas(devicePosition);

                return true;
            }
        }

        #region ICursorPositionDriver Members

        public abstract void DriveCursorPosition(
            IReadOnlyCursorInstanceStateData stateData,
            float elapsed,
            float deltaTime,
            Rect bounds,
            Vector2 normalizedPositionInBounds,
            Vector2 size,
            Vector2 center,
            out Vector2? newPosition,
            out bool? shouldLock);

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_CanAlterPosition =
            new ProfilerMarker(_PRF_PFX + nameof(CanAlterPosition));

        private static readonly ProfilerMarker _PRF_CanGetPosition =
            new ProfilerMarker(_PRF_PFX + nameof(CanGetPosition));

        protected static readonly ProfilerMarker _PRF_DriveCursorPosition =
            new ProfilerMarker(_PRF_PFX + nameof(DriveCursorPosition));

        #endregion
    }
}
