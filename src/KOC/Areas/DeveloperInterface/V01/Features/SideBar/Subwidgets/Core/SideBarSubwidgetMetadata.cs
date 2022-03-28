using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Widgets;
using Appalachia.UI.ControlModel.Components.Extensions;
using Appalachia.UI.Functionality.Tooltips.Styling;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Core
{
    [Serializable]
    [CallStaticConstructorInEditor]
    public abstract class SideBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadta> :
        DeveloperInterfaceMetadata_V01.SingletonSubwidgetMetadata<TSubwidget, TSubwidgetMetadta, ISideBarSubwidget,
            ISideBarSubwidgetMetadata, SideBarWidget, SideBarWidgetMetadata, SideBarFeature, SideBarFeatureMetadata>,
        ISideBarSubwidgetMetadata
        where TSubwidget : SideBarSubwidget<TSubwidget, TSubwidgetMetadta>
        where TSubwidgetMetadta : SideBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadta>
    {
        #region Fields and Autoproperties

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        private TooltipStyleTypes _tooltipStyle;

        #endregion

        public TooltipStyleTypes TooltipStyle => _tooltipStyle;

        public void UpdateTooltipStyle(TooltipStyleTypes tooltipStyle)
        {
            using (_PRF_UpdateTooltipStyle.Auto())
            {
                _tooltipStyle = tooltipStyle;
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                rectTransform.value.BeginModifications().FullScreen().ApplyModifications();
            }
        }

        protected override void SubscribeResponsiveComponents(TSubwidget functionality)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(functionality);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_UpdateTooltipStyle =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateTooltipStyle));

        #endregion
    }
}
