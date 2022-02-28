using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.State.Contracts;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Drivers.Contracts
{
    public interface ICursorStateDriver
    {
        void DriveCursorState(
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
    }
}
