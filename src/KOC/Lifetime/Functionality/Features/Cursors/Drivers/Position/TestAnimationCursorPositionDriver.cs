using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Drivers.Contracts;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State.Contracts;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Drivers.Position
{
    public class TestAnimationCursorPositionDriver : CursorPositionDriver<TestAnimationCursorPositionDriver>,
                                                     ICursorPositionDriver
    {
        public TestAnimationCursorPositionDriver()
        {
        }

        public TestAnimationCursorPositionDriver(Object owner) : base(owner)
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

                if (!stateData.Animate)
                {
                    return;
                }

                if (!stateData.AnimateMovement)
                {
                    return;
                }

                var radius = size * stateData.AnimationRadius;

                var progressPercentage = (elapsed % stateData.AnimationMovementDuration) /
                                         stateData.AnimationMovementDuration;

                if (stateData.AnimateMovement)
                {
                    shouldLock = true;

                    var angleTime = Mathf.SmoothStep(0f, 1f, progressPercentage);

                    var angle = angleTime * (Mathf.PI / 180f) * 360f;

                    var animatedPosition = stateData.CurrentPosition;

                    animatedPosition.x = radius.x * Mathf.Cos(angle);
                    animatedPosition.y = radius.y * Mathf.Sin(angle);

                    newPosition = center + animatedPosition;
                }
            }
        }

        #endregion
    }
}
