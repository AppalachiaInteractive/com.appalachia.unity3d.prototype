using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State.Contracts
{
    public interface IPositioningCursorInstanceStateData
    {
        bool CurrentLocked { get; }
        Rect LastRect { get; }
        Rect CurrentRect { get; }
        Vector2 CurrentPosition { get; }

        Vector2 CurrentVelocity { get; }
        Vector2 TargetPosition { get; }
        void RecordRect(Rect rect);
        void RecordPosition(Vector2 currentPosition);
        void RecordVelocity(Vector2 currentVelocity);
    }
}
