using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Controls.Subwidget;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets;
using Appalachia.UI.ControlModel.Components;
using Appalachia.UI.Core.Extensions;
using Appalachia.UI.Functionality.Buttons.Controls.Default.Contracts;
using Appalachia.UI.Functionality.Tooltips.Styling;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Core
{
    [Serializable]
    [CallStaticConstructorInEditor]
    public abstract class ActivityBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadata> :
        DeveloperInterfaceMetadata_V01.SingletonSubwidgetMetadata<TSubwidget, TSubwidgetMetadata, IActivityBarSubwidget,
            IActivityBarSubwidgetMetadata, ActivityBarWidget, ActivityBarWidgetMetadata, ActivityBarFeature,
            ActivityBarFeatureMetadata>,
        IActivityBarSubwidgetMetadata
        where TSubwidget : ActivityBarSubwidget<TSubwidget, TSubwidgetMetadata>
        where TSubwidgetMetadata : ActivityBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadata>
    {
        #region Fields and Autoproperties

        [OnValueChanged(nameof(OnChanged)), SerializeField]
        [HideIf("@!ShowAllFields")]
        public ActivityBarSubwidgetControlConfig button;

        [HideIf(nameof(HideAllFields))]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private bool _enabled;

        [HideIf(nameof(HideAllFields))]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private ActivityBarSection _section;

        [HideIf(nameof(HideAllFields))]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public Sprite icon;

        [HideIf(nameof(HideAllFields))]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public string tooltipText;

        #endregion

        public abstract ActivityBarSection DefaultSection { get; }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(_enabled),    () => _enabled = true);
                initializer.Do(this, nameof(_section),    () => _section = ActivityBarSection.Top);
                initializer.Do(this, nameof(tooltipText), () => tooltipText ??= "Where is my tooltip?");

                ActivityBarSubwidgetControlConfig.Refresh(ref button, this);
            }
        }

        protected override void OnApply(TSubwidget subwidget)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(subwidget);

                if (!subwidget.enabled || !Enabled)
                {
                    subwidget.button?.Disable();

                    return;
                }

                button.Apply(subwidget.button);

                subwidget.RectTransform.ResetRotationAndScale();

                /*
                button.LayoutGroup.IsElected = true;
                button.LayoutGroup.Value.HorizontalLayoutGroup.Enabled = false;*/
                button.ButtonIcon.IsElected = true;
                button.ButtonText.IsElected = false;
                button.ButtonShadow.IsElected = false;
                button.ButtonBackground.IsElected = false;

                var tempIcon = icon;

                var buttonIconMetadata = button.ButtonIcon;

                buttonIconMetadata.BindValueEnabledState();
                buttonIconMetadata.IsElected = true;

                if (tempIcon == null)
                {
                    tempIcon = WidgetMetadata.activityBar.defaultActivityBarIcon;
                }

                buttonIconMetadata.Value.Image.sprite.OverrideValue(tempIcon);
            }
        }

        protected override void SubscribeResponsiveComponents(TSubwidget functionality)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(functionality);

                button.SubscribeToChanges(OnChanged);
            }
        }

        protected override void UnsuspendResponsiveComponents(TSubwidget functionality)
        {
            using (_PRF_UnsuspendResponsiveComponents.Auto())
            {
                base.UnsuspendResponsiveComponents(functionality);

                button.UnsuspendChanges();
            }
        }

        protected override void SuspendResponsiveComponents(TSubwidget functionality)
        {
            using (_PRF_SuspendResponsiveComponents.Auto())
            {
                base.SuspendResponsiveComponents(functionality);

                button.SuspendChanges();
            }
        }

        #region IActivityBarSubwidgetMetadata Members

        public void UpdateTooltipStyle(TooltipStyleTypes tooltipStyle)
        {
            using (_PRF_UpdateTooltipStyle.Auto())
            {
                button.tooltip.tooltipStyle = tooltipStyle;
            }
        }

        public void UpdateSubwidgetIconSize(RectTransformConfig rectTransformData)
        {
            using (_PRF_UpdateSubwidgetIconSize.Auto())
            {
                var optionalGroupConfig = button.ButtonIcon;
                var groupConfig = optionalGroupConfig.Value;

                groupConfig.UpdateRectTransformConfig(rectTransformData);
            }
        }

        public bool Enabled => _enabled;

        public IAppaButtonControlConfig Button => button;

        public ActivityBarSection Section => _section;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_UpdateSubwidgetIconSize =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSubwidgetIconSize));

        private static readonly ProfilerMarker _PRF_UpdateTooltipStyle =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateTooltipStyle));

        #endregion
    }
}
