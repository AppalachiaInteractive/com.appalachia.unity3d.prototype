using System;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Functionality.Controls.Default.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Functionality.Controls.DevTooltips.Control;
using Appalachia.UI.ControlModel.Controls.Default;
using Appalachia.UI.Functionality.Buttons.Components;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Functionality.Controls.Default
{
    [Serializable]
    [FoldoutGroup("Components", false)]
    public abstract class AppaUITooltipControl<TControl, TConfig> : AppaUIControl<TControl, TConfig>,
                                                                    IAppaUITooltipControl<TControl, TConfig>
        where TControl : AppaUITooltipControl<TControl, TConfig>
        where TConfig : AppaUITooltipControlConfig<TControl, TConfig>, new()
    {
        #region Fields and Autoproperties

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_ELEMENTS + 01)]
        [SerializeField]
        [ShowIf(nameof(ShowsTooltip))]
        [ReadOnly]
        public DevTooltipControl tooltip;

        #endregion

        protected virtual bool RequiresTooltip => true;

        private bool ShowsTooltip => RequiresTooltip;

        public abstract string GetTooltipText();

        protected abstract AppaButton GetTooltipTarget();

        protected override void OnRefresh()
        {
            using (_PRF_Refresh.Auto())
            {
                base.OnRefresh();

                gameObject.GetOrAddComponent(ref tooltip);
            }
        }

        #region IAppaUITooltipControl<TControl,TConfig> Members

        public DevTooltipControl Tooltip => tooltip;

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_GetTooltipTarget =
            new ProfilerMarker(_PRF_PFX + nameof(GetTooltipTarget));

        #endregion
    }
}
