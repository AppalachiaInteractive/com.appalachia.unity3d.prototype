using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets;
using Appalachia.UI.Controls.Sets.Buttons.SelectableButton;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Core
{
    [CallStaticConstructorInEditor]
    [RequireComponent(typeof(RectTransform))]
    public abstract partial class ActivityBarSubwidget<TSubwidget, TSubwidgetMetadata> :
        DeveloperInterfaceManager_V01.SingletonSubwidget<TSubwidget, IActivityBarSubwidget, TSubwidgetMetadata,
            IActivityBarSubwidgetMetadata, ActivityBarWidget, ActivityBarWidgetMetadata, ActivityBarFeature,
            ActivityBarFeatureMetadata>,
        IActivityBarSubwidget
        where TSubwidget : ActivityBarSubwidget<TSubwidget, TSubwidgetMetadata>
        where TSubwidgetMetadata : ActivityBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadata>
    {
        #region Fields and Autoproperties

        public SelectableButtonComponentSet button;

        [SerializeField] private DevTooltipSubwidget _devTooltipSubwidget;

        #endregion

        public bool IsActive => Equals(Widget.ActiveSubwidget, this);

        protected override void OnUpdateSubwidget()
        {
            using (_PRF_OnUpdateSubwidget.Auto())
            {
                if (!enabled || !Metadata.Enabled)
                {
                    button?.Disable();

                    return;
                }

                SelectableButtonComponentSetData.RefreshAndApply(
                    ref Metadata.button,
                    ref button,
                    gameObject,
                    name
                );

                RectTransform.ResetRotationAndScale();

                Metadata.button.ButtonText.IsElected = false;
                Metadata.button.ButtonShadow.IsElected = false;
                Metadata.button.ButtonBackground.IsElected = false;

                var icon = Metadata.icon;
                var buttonIconMetadata = Metadata.button.ButtonIcon;

                buttonIconMetadata.BindValueEnabledState();
                buttonIconMetadata.IsElected = true;
                if (icon == null)
                {
                    icon = Widget.Metadata.defaultActivityBarIcon;
                }

                buttonIconMetadata.Value.Image.sprite.OverrideValue(icon);

                var sizeDelta = RectTransform.sizeDelta;

                sizeDelta.x = button.ButtonIcon.RectTransform.sizeDelta.x;

                RectTransform.sizeDelta = sizeDelta;

                if (_devTooltipSubwidget != null)
                {
                    OnDevTooltipUpdateRequested();
                }

                DevTooltipSubwidget.RefreshAndApplySubwidget(ref _devTooltipSubwidget, name);

                _devTooltipSubwidget.SubscribeToUpdateRequests(OnDevTooltipUpdateRequested);
                _devTooltipSubwidget.UpdateSubwidget();
            }
        }

        [ButtonGroup(nameof(Activate))]
        private void Activate()
        {
            using (_PRF_Activate.Auto())
            {
                Widget.SetActiveSubwidget(this);
                button.Selected = true;
            }
            
        }
        
        void IActivable.Activate()
        {
            Activate();
        }

        [ButtonGroup(nameof(Activate))]
        protected void Toggle()
        {
            using (_PRF_Toggle.Auto())
            {
                if (IsActive)
                {
                    Widget.DeactivateActiveSubwidget();
                }
                else
                {
                    Activate();
                }
            }
            
        }
        
        void IActivable.Toggle()
        {
            Toggle();
        }

        #region IActivityBarSubwidget Members

        [ButtonGroup(nameof(Activate))]
        public void Deactivate()
        {
            using (_PRF_Deactivate.Auto())
            {
                button.Selected = false;
            }
        }

        protected abstract void OnDeactivate();
        protected abstract void OnActivate();

        public virtual string GetDevTooltipText()
        {
            using (_PRF_GetDevTooltipText.Auto())
            {
                return Metadata.tooltipText;
            }
        }

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

        public void UpdateSubwidgetIconSize(RectTransformData rectTransformData)
        {
            using (_PRF_UpdateSubwidgetIconSize.Auto())
            {
                var buttonData = Metadata.button;
                var optionalSubsetData = buttonData.ButtonIcon;
                var subsetData = optionalSubsetData.Value;

                subsetData.UpdateRectTransformData(rectTransformData);
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_Activate = new ProfilerMarker(_PRF_PFX + nameof(Activate));

        private static readonly ProfilerMarker _PRF_Deactivate = new ProfilerMarker(_PRF_PFX + nameof(Deactivate));

        private static readonly ProfilerMarker _PRF_GetDevTooltipText =
            new ProfilerMarker(_PRF_PFX + nameof(GetDevTooltipText));
        
        private static readonly ProfilerMarker _PRF_Toggle = new ProfilerMarker(_PRF_PFX + nameof(Toggle));

        private static readonly ProfilerMarker _PRF_UpdateDevTooltipSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(OnDevTooltipUpdateRequested));

        private static readonly ProfilerMarker _PRF_UpdateSubwidgetIconSize =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateSubwidgetIconSize));

        #endregion
    }
}
