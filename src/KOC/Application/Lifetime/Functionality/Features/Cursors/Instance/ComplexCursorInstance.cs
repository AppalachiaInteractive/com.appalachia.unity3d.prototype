using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Animation;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Metadata;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets.Complex;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.State;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Instance
{
    public sealed class ComplexCursorInstance : CursorInstance<ComplexCursorInstance,
        ComplexCursorInstanceState, ComplexCursorMetadata, ComplexCursorComponentSet,
        ComplexCursorComponentSetData>
    {
        #region Fields and Autoproperties

        public CursorAnimationController controller;

        #endregion

        /// <inheritdoc />
        protected override void BeforeRendering()
        {
            using (_PRF_BeforeRendering.Auto())
            {
                if (controller == null)
                {
                    controller = new CursorAnimationController(components.Animator, this);
                }

                var shouldHide = stateData.RequiresVisibilityChange && stateData.CurrentVisibility;
                var shouldShow = stateData.RequiresVisibilityChange && !stateData.CurrentVisibility;

                if (shouldHide)
                {
                    controller.Hide.Set();
                }

                if (shouldShow)
                {
                    controller.Show.Set();
                }

                controller.Hovering.Current = stateData.IsHovering;
                controller.Pressed.Current = stateData.IsPressed;
                controller.Disabled.Current = stateData.IsDisabled;

                controller.BaseLayer.Weight = 1.0f;
                controller.SpeedLayer.Weight = stateData.NormalizedVelocityMagnitude;
            }
        }
    }
}
