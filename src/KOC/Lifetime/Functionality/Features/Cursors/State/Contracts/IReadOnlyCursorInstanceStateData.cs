using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Model;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State.Contracts
{
    public partial interface IReadOnlyCursorInstanceStateData
    {
        bool CurrentLocked { get; }
        bool CurrentVisibility { get; }
        bool RequiresVisibilityChange { get; }
        bool RequiresLockChange { get; }
        bool RequiresPositionChange { get; }
        bool RequiresStateChange { get; }
        bool TargetLocked { get; }
        bool TargetVisibility { get; }
        Color Color { get; }
        CursorStates CurrentState { get; }
        CursorStates TargetState { get; }
        Rect LastRect { get; }
        Rect CurrentRect { get; }
        Texture2D Texture { get; }
        Vector2 CurrentPosition { get; }
        Vector2 CurrentVelocity { get; }
        Vector2 Size { get; }
        Vector2 TargetPosition { get; }
    }
}
