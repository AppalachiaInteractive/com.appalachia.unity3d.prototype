using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State
{
    public partial class CursorInstanceStateData<T, TMetadata>
    {
        public void CalculateCursorMovement(float deltaTime)
        {
            using (_PRF_CalculateCursorMovement.Auto())
            {
                if (CurrentLocked)
                {
                    return;
                }

                var metadata = Metadata;

                var currentVelocity = CurrentVelocity;

                var nextPosition = Vector2.SmoothDamp(
                    CurrentPosition,
                    TargetPosition,
                    ref currentVelocity,
                    metadata.smoothTime,
                    metadata.maxVelocity,
                    deltaTime
                );

                RecordVelocity(currentVelocity);
                RecordPosition(nextPosition);
            }
        }

        public void CalculateRenderingRect()
        {
            using (_PRF_CalculateRenderingRect.Auto())
            {
                RecordRect(_currentRect);

                if (CurrentLocked)
                {
                    _currentRect = LastRect;
                }
                else
                {
                    var targetPosition = CurrentPosition;

                    var screenSize = LifetimeComponentManager.REFERENCE_RESOLUTION;

                    var x = targetPosition.x;
                    var y = screenSize.y - targetPosition.y;

                    var hotspotOffset = Size * Metadata.hotspot;

                    x -= hotspotOffset.x;
                    y -= hotspotOffset.y;

                    _currentRect = new Rect(x, y, _size.x, _size.y);
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_CalculateCursorMovement =
            new ProfilerMarker(_PRF_PFX + nameof(CalculateCursorMovement));

        private static readonly ProfilerMarker _PRF_CalculateRenderingRect =
            new ProfilerMarker(_PRF_PFX + nameof(CalculateRenderingRect));

        #endregion
    }
}
