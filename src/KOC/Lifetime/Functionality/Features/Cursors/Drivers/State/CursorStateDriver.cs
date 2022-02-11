using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Drivers.Contracts;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State.Contracts;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Drivers.State
{
    public abstract class CursorStateDriver<T> : AppalachiaBase<T>, ICursorStateDriver
        where T : CursorStateDriver<T>, new()
    {
        protected CursorStateDriver()
        {
        }

        protected CursorStateDriver(Object owner) : base(owner)
        {
        }

        #region ICursorStateDriver Members

        public abstract void DriveCursorState(
            IReadOnlyCursorInstanceStateData stateData,
            float elapsed,
            float deltaTime,
            Rect bounds,
            Vector2 normalizedPositionInBounds,
            Vector2 size,
            Vector2 center,
            out CursorStates? newState,
            out bool triggerHide,
            out bool triggerShow,
            out bool? shouldLock);

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_DriveCursorState =
            new ProfilerMarker(_PRF_PFX + nameof(DriveCursorState));

        #endregion
    }
}
