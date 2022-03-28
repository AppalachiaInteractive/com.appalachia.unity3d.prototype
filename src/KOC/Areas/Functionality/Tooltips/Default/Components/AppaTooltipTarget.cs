using Appalachia.Core.Objects.Root;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Extensions;
using Unity.Profiling;
using UnityEngine.EventSystems;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Components
{
    public class AppaTooltipTarget : AppalachiaSelectable<AppaTooltipTarget>
    {
        #region Fields and Autoproperties

        protected AppaEvent<PointerEventData>.Data HoverBegin;
        protected AppaEvent<PointerEventData>.Data HoverEnd;

        #endregion

        #region Event Functions

        protected override void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                base.OnEnable();

                _HoverBegin.Event += OnHoverBegin;
                _HoverEnd.Event += OnHoverEnd;
            }
        }

        #endregion

        private void OnHoverBegin(AppaEvent<PointerEventData>.Args args)
        {
            using (_PRF_OnHoverBegin.Auto())
            {
                HoverBegin.RaiseEvent(args);
            }
        }

        private void OnHoverEnd(AppaEvent<PointerEventData>.Args args)
        {
            using (_PRF_OnHoverEnd.Auto())
            {
                HoverEnd.RaiseEvent(args);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_OnHoverBegin = new ProfilerMarker(_PRF_PFX + nameof(OnHoverBegin));

        private static readonly ProfilerMarker _PRF_OnHoverEnd = new ProfilerMarker(_PRF_PFX + nameof(OnHoverEnd));

        #endregion
    }
}
