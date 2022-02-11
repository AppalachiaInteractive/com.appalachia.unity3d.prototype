using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Model;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State.Contracts
{
    public interface ILimitedWriteCursorInstanceStateData
    {
        void Hide();
        void Show();
        void UpdateTargetLocked(bool locked, string lockedBy);
        void UpdateTargetPosition(Vector2 targetPosition);
        void UpdateTargetState(CursorStates targetState);
        void UpdateTargetVisibility(bool visible);
    }
}
