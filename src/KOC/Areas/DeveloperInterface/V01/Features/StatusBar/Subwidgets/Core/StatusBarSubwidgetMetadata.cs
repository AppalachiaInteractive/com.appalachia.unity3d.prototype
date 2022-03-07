using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets;
using Appalachia.UI.Controls.Sets2.Buttons.Button;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Core
{
    [Serializable]
    [CallStaticConstructorInEditor]
    public abstract class
        StatusBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadta> :
            DeveloperInterfaceMetadata_V01.SingletonSubwidgetMetadata<TSubwidget, IStatusBarSubwidget,
                TSubwidgetMetadta, IStatusBarSubwidgetMetadata, StatusBarWidget, StatusBarWidgetMetadata,
                StatusBarFeature, StatusBarFeatureMetadata>,
            IStatusBarSubwidgetMetadata
        where TSubwidget : StatusBarSubwidget<TSubwidget, TSubwidgetMetadta>
        where TSubwidgetMetadta : StatusBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadta>
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("_button")]
        [OnValueChanged(nameof(OnChanged))]
        [ShowIf(nameof(showAll))]
        [SerializeField] public Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Sets2.StatusBarSubwidgetComponentSetData button;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private bool _enabled;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private StatusBarSection _section;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private int _priority;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public Sprite icon;

        #endregion

        public override void SubscribeResponsiveComponents(TSubwidget functionality)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                button.Changed.Event += OnChanged;
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(_enabled),  () => _enabled = true);
                initializer.Do(this, nameof(_section),  () => _section = StatusBarSection.Left);
                initializer.Do(this, nameof(_priority), () => _priority = 100);
            }
        }

        #region IStatusBarSubwidgetMetadata Members

        public bool Enabled => _enabled;

        public IButtonComponentSetData Button => button;

        public int Priority => _priority;

        public StatusBarSection Section => _section;

        #endregion
    }
}
