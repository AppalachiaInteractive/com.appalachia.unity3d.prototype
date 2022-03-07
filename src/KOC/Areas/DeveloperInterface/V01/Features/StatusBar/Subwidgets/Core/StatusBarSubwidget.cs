using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Components.Subsets;
using Appalachia.UI.Core.Extensions;
using Appalachia.UI.Core.Styling.Fonts;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core
{
    [CallStaticConstructorInEditor]
    [RequireComponent(typeof(RectTransform))]
    public abstract partial class StatusBarSubwidget<TSubwidget, TSubwidgetMetadata> :
        DeveloperInterfaceManager_V01.SingletonSubwidget<TSubwidget, IStatusBarSubwidget, TSubwidgetMetadata,
            IStatusBarSubwidgetMetadata, StatusBarWidget, StatusBarWidgetMetadata, StatusBarFeature,
            StatusBarFeatureMetadata>,
        IStatusBarSubwidget
        where TSubwidget : StatusBarSubwidget<TSubwidget, TSubwidgetMetadata>
        where TSubwidgetMetadata : StatusBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadata>
    {
        #region Fields and Autoproperties

        public Sets2.StatusBarSubwidgetComponentSet button;

        [SerializeField] private DevTooltipSubwidget _devTooltipSubwidget;

        #endregion

        protected abstract bool RequiresIcon { get; }

        protected abstract string GetStatusBarText();

        protected virtual Color GetStatusBarColor()
        {
            return Color.white;
        }

        protected override void OnUpdateSubwidget()
        {
            using (_PRF_OnUpdateSubwidget.Auto())
            {
                if (!enabled || !_metadata.Enabled)
                {
                    button?.Disable();

                    return;
                }

                if ((_metadata != null) &&
                    (_metadata.button != null) &&
                    (_metadata.button.ButtonText != null) &&
                    _metadata.button.ButtonText.Value is { TextMeshProUGUI: { } })
                {
                    _metadata.button.ButtonText.Value.TextMeshProUGUI.fontStyle = Widget.Metadata.fontStyle;
                }

                Sets2.StatusBarSubwidgetComponentSetData.RefreshAndUpdate(
                    ref _metadata.button,
                    ref button,
                    gameObject,
                    name
                );

                RectTransform.ResetRotationAndScale();

                var buttonTextMetadata = _metadata.button.ButtonText;
                var buttonIconMetadata = _metadata.button.ButtonIcon;
                var icon = _metadata.icon;

                buttonTextMetadata.IsElected = true;
                buttonTextMetadata.Value.Enabled = true;

                UpdateStatusBarIcon(buttonIconMetadata, icon);

                var sizeDelta = RectTransform.sizeDelta;

                sizeDelta.x = button.ButtonIcon.RectTransform.sizeDelta.x +
                              button.buttonText.TextMeshProUGUI.preferredWidth;

                RectTransform.sizeDelta = sizeDelta;

                var statusBarText = GetStatusBarText();

                var textComponent = button.buttonText.TextMeshProUGUI;
                textComponent.text = statusBarText;

                var statusBarColor = GetStatusBarColor();
                var spriteComponent = button.ButtonIcon.Image;
                spriteComponent.color = statusBarColor;
                
                UpdateStatusBarTextSize();

                if (_devTooltipSubwidget != null)
                {
                    OnDevTooltipUpdateRequested();
                }

                DevTooltipSubwidget.RefreshAndUpdateSubwidget(ref _devTooltipSubwidget, name);

                _devTooltipSubwidget.SubscribeToUpdateRequests(OnDevTooltipUpdateRequested);
                _devTooltipSubwidget.UpdateSubwidget();
            }
        }

        protected void UpdateStatusBarTextSize()
        {
            using (_PRF_UpdateStatusBarTextSize.Auto())
            {
                var textSubset = button.buttonText;
                var text = textSubset.TextMeshProUGUI;

                if (text != null)
                {
                    text.autoSizeTextContainer = true;

                    if (text.font != null)
                    {
                        var sizeDelta = text.rectTransform.sizeDelta;
                        sizeDelta.x = text.preferredWidth;
                        sizeDelta.y = text.preferredHeight;

                        text.rectTransform.sizeDelta = sizeDelta;
                    }

                    LayoutRebuilder.MarkLayoutForRebuild(text.rectTransform);
                }
            }
        }

        private void UpdateStatusBarIcon(AppalachiaBase<ImageSubsetData>.Optional buttonIconMetadata, Sprite icon)
        {
            using (_PRF_UpdateStatusBarIcon.Auto())
            {
                if (RequiresIcon)
                {
                    buttonIconMetadata.IsElected = true;
                    buttonIconMetadata.Value.Enabled = true;

                    if (icon == null)
                    {
                        icon = Widget.Metadata.defaultStatusBarIcon;
                    }

                    buttonIconMetadata.Value.Image.sprite.OverrideValue(icon);
                }
                else
                {
                    buttonIconMetadata.IsElected = false;
                    buttonIconMetadata.Value.Enabled = false;
                    buttonIconMetadata.Value.Image.sprite.OverrideValue(null);
                }
            }
        }

        #region IStatusBarSubwidget Members

        public abstract string GetDevTooltipText();

        public void OnDevTooltipUpdateRequested()
        {
            using (_PRF_UpdateDevTooltipSubwidget.Auto())
            {
                var requiredTooltipText = GetDevTooltipText();
                _devTooltipSubwidget.SetCurrentTooltip(requiredTooltipText);
                _devTooltipSubwidget.SetCurrentTarget(button.AppaButton);
                _devTooltipSubwidget.SetCurrentStyle(Widget.Metadata.devTooltipStyle);
            }
        }

        public void UpdateSubwidgetFont(FontStyleOverride fontStyleOverride)
        {
            using (_PRF_UpdateSubwidgetFont.Auto())
            {
                var buttonData = _metadata.button;
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
                var buttonData = _metadata.button;
                var optionalSubsetData = buttonData.ButtonIcon;
                var subsetData = optionalSubsetData.Value;

                subsetData.UpdateRectTransformData(rectTransformData);
            }
        }

        #endregion

        #region Profiling

        protected static readonly ProfilerMarker _PRF_GetDevTooltipText =
            new ProfilerMarker(_PRF_PFX + nameof(GetDevTooltipText));

        protected static readonly ProfilerMarker _PRF_GetStatusBarText =
            new ProfilerMarker(_PRF_PFX + nameof(GetStatusBarText));

        private static readonly ProfilerMarker _PRF_UpdateDevTooltipSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(OnDevTooltipUpdateRequested));

        private static readonly ProfilerMarker _PRF_UpdateStatusBarIcon =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateStatusBarIcon));

        private static readonly ProfilerMarker _PRF_UpdateStatusBarTextSize =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateStatusBarTextSize));

        private static readonly ProfilerMarker _PRF_UpdateSubwidgetFont =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSubwidgetFont));

        private static readonly ProfilerMarker _PRF_UpdateSubwidgetIconSize =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSubwidgetIconSize));

        #endregion
    }
}
