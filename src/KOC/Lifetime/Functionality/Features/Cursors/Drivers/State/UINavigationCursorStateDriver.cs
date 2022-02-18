using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State.Contracts;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Drivers.State
{
    public class UINavigationCursorStateDriver : CursorStateDriver<UINavigationCursorStateDriver>
    {
        public UINavigationCursorStateDriver()
        {
        }

        public UINavigationCursorStateDriver(Object owner) : base(owner)
        {
        }

        /// <inheritdoc />
        public override void DriveCursorState(
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
            out bool? shouldLock)
        {
            using (_PRF_DriveCursorState.Auto())
            {
                newState = null;
                triggerHide = false;
                triggerShow = false;
                shouldLock = false;
            }
        }
    }
}
