using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Drivers.Contracts;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.State.Contracts;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Drivers.Position
{
    public class UINavigationCursorPositionDriver : CursorPositionDriver<UINavigationCursorPositionDriver>,
                                                    ICursorPositionDriver
    {
        public UINavigationCursorPositionDriver()
        {
        }

        public UINavigationCursorPositionDriver(Object owner) : base(owner)
        {
        }

        #region ICursorPositionDriver Members

        /// <inheritdoc />
        public override void DriveCursorPosition(
            IReadOnlyCursorInstanceStateData stateData,
            float elapsed,
            float deltaTime,
            Rect bounds,
            Vector2 normalizedPositionInBounds,
            Vector2 size,
            Vector2 center,
            out Vector2? newPosition,
            out bool? shouldLock)
        {
            using (_PRF_DriveCursorPosition.Auto())
            {
                newPosition = null;
                shouldLock = false;
            }
        }

        #endregion
    }
}
