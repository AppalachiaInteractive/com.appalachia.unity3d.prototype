using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Metadata;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.State.Contracts;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.State
{
    [Serializable]
    [DoNotReorderFields]
    public abstract partial class CursorInstanceStateData<T, TMetadata> : AppalachiaBase<T>,
        IReadLimitedWriteCursorInstanceStateData,
        IPositioningCursorInstanceStateData
        where T : CursorInstanceStateData<T, TMetadata>, new()
        where TMetadata : CursorMetadata<TMetadata>
    {
        protected CursorInstanceStateData()
        {
        }

        protected CursorInstanceStateData(Object owner) : base(owner)
        {
        }

        #region Fields and Autoproperties

        [NonSerialized, ShowInInspector, PreviewField(64)]
        [ReadOnly]
        private Texture2D _texture;

        [SerializeField]
        [ReadOnly]
        private Vector2 _size;

        [SerializeField]
        [ReadOnly]
        private Vector2 _currentVelocity;

        [SerializeField]
        [ReadOnly]
        private Vector2 _currentPosition;

        [SerializeField]
        [ReadOnly]
        private Vector2 _targetPosition;

        [SerializeField]
        [ReadOnly]
        private CursorStates _currentState;

        [SerializeField]
        [ReadOnly]
        private CursorStates _targetState;

        [SerializeField]
        [ToggleLeft]
        [ReadOnly]
        private bool _currentVisibility;

        [SerializeField]
        [ToggleLeft]
        [ReadOnly]
        private bool _targetVisibility;

        [SerializeField]
        [ReadOnly]
        [ToggleLeft]
        private bool _currentLocked;

        [SerializeField]
        [ReadOnly]
        private Color _color;

        [SerializeField]
        [ReadOnly]
        private Rect _currentRect;

        [SerializeField]
        [ReadOnly]
        private Rect _lastRect;

        [NonSerialized, ShowInInspector]
        [ReadOnly]
        private bool _hasSearchedSpeedLayer;

        [NonSerialized, ShowInInspector]
        [ReadOnly]
        private bool _hasFoundSpeedLayer;

        [NonSerialized, ShowInInspector]
        [ReadOnly]
        private int _speedLayerIndex;

        private HashSet<string> _lockers;

        #endregion

        public abstract TMetadata Metadata { get; }

        public float NormalizedVelocityMagnitude =>
            Mathf.Clamp01((CurrentVelocity / Metadata.maxVelocity).magnitude);

        public Rect CurrentRect => _currentRect;

        private HashSet<string> Lockers
        {
            get
            {
                _lockers ??= new HashSet<string>();
                return _lockers;
            }
        }

        public void MarkLockedChangeComplete()
        {
            using (_PRF_MarkLockedChangeComplete.Auto())
            {
                _currentLocked = TargetLocked;
            }
        }

        public void MarkStateChangeComplete()
        {
            using (_PRF_MarkStateChangeComplete.Auto())
            {
                _currentState = _targetState;
            }
        }

        public void MarkVisibilityChangeComplete()
        {
            using (_PRF_MarkVisibilityChangeComplete.Auto())
            {
                _currentVisibility = _targetVisibility;
            }
        }

        public bool ShouldExecute()
        {
            using (_PRF_ShouldExecute.Auto())
            {
                if (!CurrentVisibility && !TargetVisibility)
                {
                    return false;
                }

                return ShouldExecuteInternal();
            }
        }

        protected abstract bool ShouldExecuteInternal();

        #region IPositioningCursorInstanceStateData Members

        public void RecordRect(Rect rect)
        {
            using (_PRF_RecordLastRect.Auto())
            {
                _lastRect = rect;
            }
        }

        public void RecordPosition(Vector2 currentPosition)
        {
            using (_PRF_RecordPosition.Auto())
            {
                _currentPosition = currentPosition;
            }
        }

        public void RecordVelocity(Vector2 currentVelocity)
        {
            using (_PRF_RecordVelocity.Auto())
            {
                _currentVelocity = currentVelocity;
            }
        }

        private static readonly ProfilerMarker _PRF_RecordColor =
            new ProfilerMarker(_PRF_PFX + nameof(RecordColor));

        public void RecordColor(Color color)
        {
            using (_PRF_RecordColor.Auto())
            {
                _color = color;
            }
        }

        #endregion

        #region IReadLimitedWriteCursorInstanceStateData Members

        public void Hide()
        {
            using (_PRF_Hide.Auto())
            {
                UpdateTargetVisibility(false);
            }
        }

        public void Show()
        {
            using (_PRF_Show.Auto())
            {
                UpdateTargetVisibility(true);
            }
        }

        public void UpdateTargetLocked(bool locked, string lockedBy)
        {
            using (_PRF_UpdateTargetLocked.Auto())
            {
                if (locked)
                {
                    Lockers.Add(lockedBy);
                }
                else
                {
                    Lockers.Remove(lockedBy);
                }
            }
        }

        public void UpdateTargetPosition(Vector2 targetPosition)
        {
            using (_PRF_UpdateTargetPosition.Auto())
            {
                _targetPosition = targetPosition;
            }
        }

        public void UpdateTargetState(CursorStates targetState)
        {
            using (_PRF_UpdateTargetState.Auto())
            {
                _targetState = targetState;
            }
        }

        public void UpdateTargetVisibility(bool visible)
        {
            using (_PRF_UpdateTargetVisibility.Auto())
            {
                _targetVisibility = visible;
            }
        }

        public bool CurrentLocked => _currentLocked;

        public bool CurrentVisibility => _currentVisibility;

        public bool RequiresPositionChange => _currentPosition != _targetPosition;
        public bool RequiresVisibilityChange => _currentVisibility != _targetVisibility;
        public bool RequiresLockChange => _currentLocked != TargetLocked;
        public bool RequiresStateChange => _currentState != _targetState;
        public bool TargetLocked => Lockers.Count > 0;
        public bool TargetVisibility => _targetVisibility;
        public Color Color => _color;
        public CursorStates CurrentState => _currentState;
        public bool IsHovering => CurrentState == CursorStates.Hovering;
        public bool IsPressed => CurrentState == CursorStates.Pressed;
        public bool IsDisabled => CurrentState == CursorStates.Disabled;
        public bool IsNormal => CurrentState == CursorStates.Normal;

        public CursorStates TargetState => _targetState;
        public Rect LastRect => _lastRect;
        public Texture2D Texture => _texture;
        public Vector2 CurrentPosition => _currentPosition;

        public Vector2 CurrentVelocity => _currentVelocity;
        public Vector2 Size => _size;
        public Vector2 TargetPosition => _targetPosition;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_Hide = new ProfilerMarker(_PRF_PFX + nameof(Hide));

        private static readonly ProfilerMarker _PRF_MarkLockedChangeComplete =
            new ProfilerMarker(_PRF_PFX + nameof(MarkLockedChangeComplete));

        private static readonly ProfilerMarker _PRF_MarkStateChangeComplete =
            new ProfilerMarker(_PRF_PFX + nameof(MarkStateChangeComplete));

        private static readonly ProfilerMarker _PRF_MarkVisibilityChangeComplete =
            new ProfilerMarker(_PRF_PFX + nameof(MarkVisibilityChangeComplete));

        protected static readonly ProfilerMarker _PRF_Metadata =
            new ProfilerMarker(_PRF_PFX + nameof(Metadata));

        private static readonly ProfilerMarker _PRF_RecordLastRect =
            new ProfilerMarker(_PRF_PFX + nameof(RecordRect));

        private static readonly ProfilerMarker _PRF_RecordPosition =
            new ProfilerMarker(_PRF_PFX + nameof(RecordPosition));

        private static readonly ProfilerMarker _PRF_RecordVelocity =
            new ProfilerMarker(_PRF_PFX + nameof(RecordVelocity));

        protected static readonly ProfilerMarker _PRF_ShouldExecute =
            new ProfilerMarker(_PRF_PFX + nameof(ShouldExecute));

        protected static readonly ProfilerMarker _PRF_ShouldExecuteInternal =
            new ProfilerMarker(_PRF_PFX + nameof(ShouldExecuteInternal));

        private static readonly ProfilerMarker _PRF_Show = new ProfilerMarker(_PRF_PFX + nameof(Show));

        private static readonly ProfilerMarker _PRF_UpdatePosition =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateTargetPosition));

        private static readonly ProfilerMarker _PRF_UpdateState =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateTargetState));

        private static readonly ProfilerMarker _PRF_UpdateTargetLocked =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateTargetLocked));

        private static readonly ProfilerMarker _PRF_UpdateTargetPosition =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateTargetPosition));

        private static readonly ProfilerMarker _PRF_UpdateTargetState =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateTargetState));

        private static readonly ProfilerMarker _PRF_UpdateTargetVisibility =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateTargetVisibility));

        private static readonly ProfilerMarker _PRF_UpdateVisibility =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateTargetVisibility));

        #endregion
    }
}
