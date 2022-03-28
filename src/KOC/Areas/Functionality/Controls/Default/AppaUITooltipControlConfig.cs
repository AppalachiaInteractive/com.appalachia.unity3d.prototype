using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Functionality.Controls.Default.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Functionality.Controls.DevTooltips.Control;
using Appalachia.UI.ControlModel.Controls.Default;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Functionality.Controls.Default
{
    [CallStaticConstructorInEditor]
    [Serializable]
    public abstract class AppaUITooltipControlConfig<TControl, TConfig> : AppaUIControlConfig<TControl, TConfig>,
                                                                          IAppaUITooltipControlConfig<TControl, TConfig>
        where TControl : AppaUITooltipControl<TControl, TConfig>
        where TConfig : AppaUITooltipControlConfig<TControl, TConfig>, IAppaUITooltipControlConfig<TControl, TConfig>,
        new()
    {
        protected AppaUITooltipControlConfig()
        {
        }

        protected AppaUITooltipControlConfig(Object owner) : base(owner)
        {
        }

        #region Fields and Autoproperties

        [HideIf("@!ShowAllFields && (HideTooltip || HideAllFields)")]
        [PropertyOrder(ORDER_RECT)]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public DevTooltipControlConfig tooltip;

        #endregion

        protected virtual bool HideTooltip => !RequiresTooltip;
        protected virtual bool RequiresTooltip => true;

        private bool ShowsTooltip => RequiresTooltip;

        protected override void OnApply(TControl control)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(control);
                tooltip.Apply(control.tooltip);
            }
        }

        protected override void OnInitializeFields(Initializer initializer)
        {
            using (_PRF_OnInitializeFields.Auto())
            {
                base.OnInitializeFields(initializer);
                DevTooltipControlConfig.Refresh(ref tooltip, Owner);
            }
        }

        protected override void OnRefresh(Object owner)
        {
            using (_PRF_Refresh.Auto())
            {
                base.OnRefresh(owner);

                DevTooltipControlConfig.Refresh(ref tooltip, Owner);
            }
        }

        protected override void SubscribeResponsiveConfigs()
        {
            using (_PRF_SubscribeResponsiveConfigs.Auto())
            {
                base.SubscribeResponsiveConfigs();

                tooltip.SubscribeToChanges(OnChanged);
            }
        }

        #region IAppaUITooltipControlConfig<TControl,TConfig> Members

        public DevTooltipControlConfig Tooltip => tooltip;

        #endregion
    }
}
