using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Sets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets;
using Appalachia.UI.Controls.Sets.Buttons.Button;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Components.Subsets;
using Appalachia.UI.Core.Extensions;
using Appalachia.UI.Core.Styling.Fonts;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
        public StatusBarSubwidgetComponentSetData button;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private bool _enabled;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private StatusBarSection _section;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public Sprite icon;

        #endregion

        public virtual bool RequiresIcon => true;

        public void UpdateSubwidgetFont(FontStyleOverride fontStyleOverride)
        {
            using (_PRF_UpdateSubwidgetFont.Auto())
            {
                var buttonData = button;
                var optionalSubsetData = buttonData.ButtonText;
                var subsetData = optionalSubsetData.Value;
                var componentData = subsetData.TextMeshProUGUI;

                componentData.fontStyle = fontStyleOverride;
            }
        }

        public void UpdateSubwidgetIconSize(RectTransformData rectTransformData)
        {
            using (_PRF_UpdateSubwidgetIconSize.Auto())
            {
                var buttonData = button;
                var optionalSubsetData = buttonData.ButtonIcon;
                var subsetData = optionalSubsetData.Value;

                subsetData.UpdateRectTransformData(rectTransformData);
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(_enabled), () => _enabled = true);
                initializer.Do(this, nameof(_section), () => _section = StatusBarSection.Left);
            }
        }

        protected override void SubscribeResponsiveComponents(TSubwidget functionality)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(functionality);

                button.Changed.Event += OnChanged;
            }
        }

        protected override void UpdateFunctionalityInternal(TSubwidget subwidget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(subwidget);

                if (!subwidget.enabled || !Enabled)
                {
                    subwidget.button?.Disable();

                    return;
                }

                if ((button != null) &&
                    (button.ButtonText != null) &&
                    button.ButtonText.Value is { TextMeshProUGUI: { } })
                {
                    button.ButtonText.Value.TextMeshProUGUI.fontStyle = WidgetMetadata.fontStyle;
                }

                StatusBarSubwidgetComponentSetData.RefreshAndApply(ref button, ref subwidget.button, subwidget.gameObject, name, this);

                subwidget.RectTransform.ResetRotationAndScale();

                var buttonTextMetadata = button.ButtonText;
                var buttonIconMetadata = button.ButtonIcon;
                var tempIcon = icon;

                buttonTextMetadata.BindValueEnabledState();
                buttonTextMetadata.IsElected = true;

                UpdateStatusBarIcon(buttonIconMetadata, tempIcon);

                var sizeDelta = subwidget.RectTransform.sizeDelta;

                sizeDelta.x = subwidget.button.ButtonIcon.RectTransform.sizeDelta.x +
                              subwidget.button.buttonText.TextMeshProUGUI.preferredWidth;

                subwidget.RectTransform.sizeDelta = sizeDelta;

                var statusBarText = subwidget.GetStatusBarText();

                var textComponent = subwidget.button.buttonText.TextMeshProUGUI;
                textComponent.text = statusBarText;

                var statusBarColor = subwidget.GetStatusBarColor();
                var spriteComponent = subwidget.button.ButtonIcon.Image;
                spriteComponent.color = statusBarColor;

                subwidget.LayoutStatusBarText();

                if (subwidget.devTooltipSubwidget != null)
                {
                    subwidget.OnDevTooltipUpdateRequested();
                }
            }
        }

        private void UpdateStatusBarIcon(ImageSubsetData.Optional buttonIconMetadata, Sprite tempIcon)
        {
            using (_PRF_UpdateStatusBarIcon.Auto())
            {
                buttonIconMetadata.BindValueEnabledState();

                if (RequiresIcon)
                {
                    buttonIconMetadata.IsElected = true;

                    if (tempIcon == null)
                    {
                        tempIcon = WidgetMetadata.defaultStatusBarIcon;
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

        public bool Enabled => _enabled;

        public IButtonComponentSetData Button => button;
        public StatusBarSection Section => _section;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_UpdateStatusBarIcon =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateStatusBarIcon));

        private static readonly ProfilerMarker _PRF_UpdateSubwidgetFont =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSubwidgetFont));

        private static readonly ProfilerMarker _PRF_UpdateSubwidgetIconSize =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSubwidgetIconSize));

        #endregion
    }
}
