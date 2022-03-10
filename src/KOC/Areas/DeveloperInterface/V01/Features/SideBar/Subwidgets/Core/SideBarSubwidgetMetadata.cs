using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Widgets;
using Appalachia.Utility.Async;

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
        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
            }
        }

        protected override void SubscribeResponsiveComponents(TSubwidget functionality)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(functionality);
            }
        }
    }
}
