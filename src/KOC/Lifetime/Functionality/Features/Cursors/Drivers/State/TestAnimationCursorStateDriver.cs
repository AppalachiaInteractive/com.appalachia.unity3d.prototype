using System;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Drivers.Contracts;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.State.Contracts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Drivers.State
{
    public sealed class TestAnimationCursorStateDriver : CursorStateDriver<TestAnimationCursorStateDriver>,
                                                         ICursorStateDriver
    {
        public TestAnimationCursorStateDriver()
        {
        }

        public TestAnimationCursorStateDriver(Object owner) : base(owner)
        {
        }

        #region ICursorStateDriver Members

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

                if (!stateData.Animate || !stateData.AnimateState)
                {
                    return;
                }

                var previousFrameDuration = deltaTime;
                var previousTime = elapsed - previousFrameDuration;
                if (stateData.AnimateState)
                {
                    var previousStateChangeProgressPercentage =
                        (previousTime % stateData.AnimationStateChangeDuration) /
                        stateData.AnimationStateChangeDuration;

                    var stateChangeProgressPercentage = (elapsed % stateData.AnimationStateChangeDuration) /
                                                        stateData.AnimationStateChangeDuration;

                    if (stateChangeProgressPercentage < previousStateChangeProgressPercentage)
                    {
                        newState = stateData.CurrentState + 1;

                        if (!Enum.IsDefined(typeof(CursorStates), stateData.CurrentState))
                        {
                            newState = CursorStates.Normal;
                            triggerHide = true;
                        }
                    }

                    if ((stateData.CurrentState == CursorStates.Normal) && !stateData.CurrentVisibility)
                    {
                        triggerShow = true;
                    }
                }
            }
        }

        #endregion
    }
}
