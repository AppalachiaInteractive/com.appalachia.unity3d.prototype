using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Controls.Subwidget;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets;
using Appalachia.UI.ControlModel.Components;
using Appalachia.UI.ControlModel.Components.Extensions;
using Appalachia.UI.Core.Extensions;
using Appalachia.UI.Functionality.Buttons.Controls.Default.Contracts;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.UI.Functionality.Tooltips.Styling;
using Appalachia.UI.Styling.Fonts;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core
{
    [Serializable]
    [CallStaticConstructorInEditor]
    public abstract class StatusBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadta> :
        DeveloperInterfaceMetadata_V01.SingletonSubwidgetMetadata<TSubwidget, TSubwidgetMetadta, IStatusBarSubwidget,
            IStatusBarSubwidgetMetadata, StatusBarWidget, StatusBarWidgetMetadata, StatusBarFeature,
            StatusBarFeatureMetadata>,
        IStatusBarSubwidgetMetadata
        where TSubwidget : StatusBarSubwidget<TSubwidget, TSubwidgetMetadta>
        where TSubwidgetMetadta : StatusBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadta>
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("_button")]
        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideAllFields))]
        [SerializeField]
        public StatusBarSubwidgetControlConfig button;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private bool _enabled;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private StatusBarSection _section;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public Sprite icon;

        #endregion

        public abstract StatusBarSection DefaultSection { get; }

        public virtual bool RequiresIcon => true;

        protected override void BeforeApplying(TSubwidget subwidget)
        {
            using (_PRF_BeforeApplying.Auto())
            {
                base.BeforeApplying(subwidget);

                button.buttonText.IsElected = true;
                button.buttonText.Value.RectTransform.BeginModifications().AnchorRight().PivotMiddleRight().ApplyModifications();

                button.buttonText.value.textMeshProUGUI.fontStyle = fontStyle;

                var fontStyleOverride = StyleLookup.GetFont(button.buttonText.value.textMeshProUGUI.fontStyle);
                fontStyleOverride.HorizontalAlignment = HorizontalAlignmentOptions.Right;

                button.buttonIcon.IsElected = true;
                button.buttonIcon.Value.RectTransform.BeginModifications().AnchorLeft().PivotMiddleLeft().ApplyModifications();

                if ((button.ButtonText != null) && button.ButtonText.Value is { TextMeshProUGUI: { } })
                {
                    button.ButtonText.Value.TextMeshProUGUI.fontStyle = WidgetMetadata.fontStyle;
                }

                var buttonTextMetadata = button.ButtonText;

                buttonTextMetadata.BindValueEnabledState();
                buttonTextMetadata.IsElected = true;

                var buttonIconMetadata = button.ButtonIcon;
                var tempIcon = icon;

                UpdateStatusBarIcon(buttonIconMetadata, tempIcon);
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(_enabled),       () => _enabled = true);
                initializer.Do(this, nameof(DefaultSection), () => _section = DefaultSection);

                StatusBarSubwidgetControlConfig.Refresh(ref button, this);
            }
        }

        protected override void OnApply(TSubwidget subwidget)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(subwidget);

                if (!subwidget.enabled || !Enabled)
                {
                    subwidget.button.Disable();

                    return;
                }

                subwidget.RectTransform.ResetRotationAndScale();

                button.Apply(subwidget.button);
                
                var sizeDelta = subwidget.RectTransform.sizeDelta;

                var iconSize = subwidget.button.ButtonIcon.RectTransform.sizeDelta.x;
                var padding = 8f;

                sizeDelta.x = iconSize + padding + subwidget.button.buttonText.TextMeshProUGUI.preferredWidth;

                subwidget.RectTransform.sizeDelta = sizeDelta;

                var statusBarText = subwidget.GetStatusBarText();

                var textComponent = subwidget.button.buttonText.TextMeshProUGUI;
                textComponent.text = statusBarText;

                var statusBarColor = subwidget.GetStatusBarColor();
                var spriteComponent = subwidget.button.ButtonIcon.Image;
                spriteComponent.color = statusBarColor;

                subwidget.LayoutStatusBarText();
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

        private void UpdateStatusBarIcon(ImageComponentGroupConfig.Optional buttonIconMetadata, Sprite tempIcon)
        {
            using (_PRF_UpdateStatusBarIcon.Auto())
            {
                buttonIconMetadata.BindValueEnabledState();

                if (RequiresIcon)
                {
                    buttonIconMetadata.IsElected = true;

                    if (tempIcon == null)
                    {
                        tempIcon = WidgetMetadata.statusBar.defaultStatusBarIcon;
                    }

                    buttonIconMetadata.Value.Image.sprite.OverrideValue(tempIcon);
                }
                else
                {
                    buttonIconMetadata.IsElected = false;
                    buttonIconMetadata.Value.Image.sprite.OverrideValue(null);
                }
            }
        }

        #region IStatusBarSubwidgetMetadata Members

        public void UpdateFontStyle(FontStyleTypes style)
        {
            using (_PRF_UpdateSubwidgetFont.Auto())
            {
                button.buttonText.value.textMeshProUGUI.fontStyle = style;
            }
        }

        public void UpdateTooltipStyle(TooltipStyleTypes style)
        {
            using (_PRF_UpdateTooltipStyle.Auto())
            {
                button.tooltip.tooltipStyle = style;
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
        public StatusBarSection Section => _section;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_UpdateStatusBarIcon =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateStatusBarIcon));

        private static readonly ProfilerMarker _PRF_UpdateSubwidgetFont =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateFontStyle));

        private static readonly ProfilerMarker _PRF_UpdateSubwidgetIconSize =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSubwidgetIconSize));

        private static readonly ProfilerMarker _PRF_UpdateTooltipStyle =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateTooltipStyle));

        #endregion
    }
}
