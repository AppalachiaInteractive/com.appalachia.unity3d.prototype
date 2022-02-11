using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State.Contracts;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Drivers.Contracts
{
    public interface ICursorPositionDriver
    {
        void DriveCursorPosition(
            IReadOnlyCursorInstanceStateData stateData,
            float elapsed,
            float deltaTime,
            Rect bounds,
            Vector2 normalizedPositionInBounds,
            Vector2 size,
            Vector2 center,
            out Vector2? newPosition,
            out bool? shouldLock);
    }
}
