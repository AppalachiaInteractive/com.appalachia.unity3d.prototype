using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Drivers.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Drivers.Position;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Drivers.State;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.State.Contracts;
using Appalachia.Utility.Enums;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Drivers
{
    [Serializable]
    [CallStaticConstructorInEditor]
    public sealed class CursorDriver : AppalachiaBase<CursorDriver>, ICursorDriver
    {
        static CursorDriver()
        {
            When.Behaviour<LifetimeComponentManager>().IsAvailableThen(i => _lifetimeComponentManager = i);
        }

        public CursorDriver()
        {
            BuildCollections();
        }

        public CursorDriver(Object owner) : base(owner)
        {
            BuildCollections();
        }

        #region Static Fields and Autoproperties

        private static LifetimeComponentManager _lifetimeComponentManager;

        #endregion

        #region Fields and Autoproperties

        [SerializeField] private CursorPositionDrivers _cursorPositionDriver;

        private Dictionary<CursorPositionDrivers, ICursorPositionDriver> _cursorPositionDriverInstances;

        [SerializeField] private CursorStateDrivers _cursorStateDriver;

        private Dictionary<CursorStateDrivers, ICursorStateDriver> _cursorStateDriverInstances;

        #endregion

        private static LifetimeComponentManager LifetimeComponentManager => _lifetimeComponentManager;

        public CursorPositionDrivers CursorPositionDriver
        {
            get => _cursorPositionDriver;
            set => _cursorPositionDriver = value;
        }

        public CursorStateDrivers CursorStateDriver
        {
            get => _cursorStateDriver;
            set => _cursorStateDriver = value;
        }

        private void BuildCollections()
        {
            _cursorPositionDriverInstances ??= new();

            _cursorPositionDriverInstances.PopulateEnumKeys(
                positionDriver =>
                {
                    switch (positionDriver)
                    {
                        case CursorPositionDrivers.None:
                            return null;
                        case CursorPositionDrivers.TestAnimation:
                            return new TestAnimationCursorPositionDriver(Owner);
                        case CursorPositionDrivers.AnyPointer:
                            return new AnyPointerCursorPositionDriver(Owner);
                        case CursorPositionDrivers.AnyDevice:
                            return new AnyDeviceCursorPositionDriver(Owner);
                        case CursorPositionDrivers.UINavigation:
                            return new UINavigationCursorPositionDriver(Owner);
                        default:
                            throw new ArgumentOutOfRangeException(
                                nameof(positionDriver),
                                positionDriver,
                                null
                            );
                    }
                }
            );

            _cursorStateDriverInstances ??= new();

            _cursorStateDriverInstances.PopulateEnumKeys(
                stateDriver =>
                {
                    switch (stateDriver)
                    {
                        case CursorStateDrivers.None:
                            return null;
                        case CursorStateDrivers.TestAnimation:
                            return new TestAnimationCursorStateDriver(Owner);
                        case CursorStateDrivers.UnitySelectable:
                            return new UnitySelectableCursorStateDriver(Owner);
                        case CursorStateDrivers.UINavigation:
                            return new UINavigationCursorStateDriver(Owner);
                        default:
                            throw new ArgumentOutOfRangeException(nameof(stateDriver), stateDriver, null);
                    }
                }
            );
        }

        #region ICursorDriver Members

        public void DriveCursorInstance(
            IReadLimitedWriteCursorInstanceStateData stateData,
            float time,
            float deltaTime,
            Rect bounds)
        {
            using (_PRF_DriveCursor.Auto())
            {
                var normalizedPositionInBounds =
                    bounds.GetNormalizedPositionWithin(stateData.CurrentPosition);

                var positionDriver = _cursorPositionDriverInstances[_cursorPositionDriver];
                var stateDriver = _cursorStateDriverInstances[_cursorStateDriver];

                positionDriver.DriveCursorPosition(
                    stateData,
                    time,
                    deltaTime,
                    bounds,
                    normalizedPositionInBounds,
                    bounds.size,
                    bounds.center,
                    out var newPosition,
                    out var shouldLockViaPositionDriver
                );

                stateDriver.DriveCursorState(
                    stateData,
                    time,
                    deltaTime,
                    bounds,
                    normalizedPositionInBounds,
                    bounds.size,
                    bounds.center,
                    out var newState,
                    out var triggerHide,
                    out var triggerShow,
                    out var shouldLockViaStateDriver
                );

                if (shouldLockViaPositionDriver.HasValue && shouldLockViaPositionDriver.Value)
                {
                    stateData.UpdateTargetLocked(true, nameof(_cursorPositionDriver));
                }
                else
                {
                    stateData.UpdateTargetLocked(false, nameof(_cursorPositionDriver));
                }

                if (shouldLockViaStateDriver.HasValue && shouldLockViaStateDriver.Value)
                {
                    stateData.UpdateTargetLocked(true, nameof(_cursorStateDriver));
                }
                else
                {
                    stateData.UpdateTargetLocked(false, nameof(_cursorStateDriver));
                }

                if (triggerHide)
                {
                    stateData.Hide();
                }

                if (triggerShow)
                {
                    stateData.Show();
                }

                if (newState.HasValue)
                {
                    stateData.UpdateTargetState(newState.Value);
                }

                if (newPosition.HasValue && (newPosition.Value != stateData.CurrentPosition))
                {
                    var resultingPosition = newPosition.Value;

                    var clampedPosition = resultingPosition.ClampValue(bounds.min, bounds.max);

                    stateData.UpdateTargetPosition(clampedPosition);
                }

#if UNITY_EDITOR
                if (newState.HasValue || newPosition.HasValue)
                {
                    UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
                    UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                }
#endif
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_DriveCursor =
            new ProfilerMarker(_PRF_PFX + nameof(DriveCursorInstance));

        #endregion
    }
}
