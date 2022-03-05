using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets;
using Appalachia.UI.Controls.Sets.Buttons.SelectableButton;
using Appalachia.UI.Controls.Sets2.Buttons.SelectableButton;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Core
{
    [Serializable]
    [CallStaticConstructorInEditor]
    public abstract class ActivityBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadta> :
        DeveloperInterfaceMetadata_V01.SingletonSubwidgetMetadata<TSubwidget, IActivityBarSubwidget, TSubwidgetMetadta,
            IActivityBarSubwidgetMetadata, ActivityBarWidget, ActivityBarWidgetMetadata, ActivityBarFeature,
            ActivityBarFeatureMetadata>,
        IActivityBarSubwidgetMetadata
        where TSubwidget : ActivityBarSubwidget<TSubwidget, TSubwidgetMetadta>
        where TSubwidgetMetadta : ActivityBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadta>
    {
        #region Fields and Autoproperties

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [HideInInspector]
        public SelectableButtonComponentSetData button;
            
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        [HideInInspector]
        public SelectableButtonComponentSetData button2;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private bool _enabled;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private ActivityBarSection _section;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private int _priority;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public Sprite icon;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public string tooltipText;

        #endregion

        public override void SubscribeResponsiveComponents(TSubwidget functionality)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(functionality);

                button.Changed.Event += OnChanged;
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
                initializer.Do(this, nameof(_priority),   () => _priority = 100);
                initializer.Do(this, nameof(tooltipText), () => tooltipText ??= "Where is my tooltip?");

                button = button2;
            }
        }

        #region IActivityBarSubwidgetMetadata Members

        public bool Enabled => _enabled;

        public ISelectableButtonComponentSetData Button => button;

        public int Priority => _priority;

        public ActivityBarSection Section => _section;

        #endregion
    }
}
