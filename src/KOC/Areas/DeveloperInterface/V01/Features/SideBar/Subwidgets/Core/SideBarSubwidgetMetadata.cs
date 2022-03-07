using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Widgets;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Core
{
    [Serializable]
    [CallStaticConstructorInEditor]
    public abstract class SideBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadta> :
        DeveloperInterfaceMetadata_V01.SingletonSubwidgetMetadata<TSubwidget, ISideBarSubwidget, TSubwidgetMetadta,
            ISideBarSubwidgetMetadata, SideBarWidget, SideBarWidgetMetadata, SideBarFeature, SideBarFeatureMetadata>,
        ISideBarSubwidgetMetadata
        where TSubwidget : SideBarSubwidget<TSubwidget, TSubwidgetMetadta>
        where TSubwidgetMetadta : SideBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadta>
    {
        #region Fields and Autoproperties

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private bool _enabled;

        #endregion

        protected override void SubscribeResponsiveComponents(TSubwidget functionality)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(functionality);
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
            }
        }

        #region ISideBarSubwidgetMetadata Members

        public bool Enabled => _enabled;

        #endregion
    }
}
