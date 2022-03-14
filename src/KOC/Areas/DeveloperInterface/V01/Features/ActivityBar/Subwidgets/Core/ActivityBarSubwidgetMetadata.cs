using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets;
using Appalachia.UI.Core.Extensions;
using Appalachia.UI.Functionality.Buttons.Controls.Default;
using Appalachia.UI.Functionality.Buttons.Controls.Default.Contracts;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
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
        public AppaButtonControlConfig button;

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

                AppaButtonControlConfig.RefreshAndApply(
                    ref button,
                    ref subwidget.button,
                    subwidget.canvas.GameObject,
                    name,
                    this
                );

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
                    tempIcon = WidgetMetadata.defaultActivityBarIcon;
                }

                buttonIconMetadata.Value.Image.sprite.OverrideValue(tempIcon);

                var sizeDelta = subwidget.RectTransform.sizeDelta;

                sizeDelta.x = subwidget.button.ButtonIcon.RectTransform.sizeDelta.x;

                subwidget.RectTransform.sizeDelta = sizeDelta;
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(_enabled),    () => _enabled = true);
                initializer.Do(this, nameof(_section),    () => _section = ActivityBarSection.Top);
                initializer.Do(this, nameof(tooltipText), () => tooltipText ??= "Where is my tooltip?");
            }
        }

        #region IActivityBarSubwidgetMetadata Members

        public bool Enabled => _enabled;

        public IAppaButtonControlConfig Button => button;

        public ActivityBarSection Section => _section;

        #endregion
    }
}
