using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Widgets;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Core
{
    [CallStaticConstructorInEditor]
    [RequireComponent(typeof(RectTransform))]
    public abstract class SideBarSubwidget<TSubwidget, TSubwidgetMetadata> :
        DeveloperInterfaceManager_V01.SingletonSubwidget<TSubwidget, ISideBarSubwidget, TSubwidgetMetadata,
            ISideBarSubwidgetMetadata, SideBarWidget, SideBarWidgetMetadata, SideBarFeature, SideBarFeatureMetadata>,
        ISideBarSubwidget
        where TSubwidget : SideBarSubwidget<TSubwidget, TSubwidgetMetadata>
        where TSubwidgetMetadata : SideBarSubwidgetMetadata<TSubwidget, TSubwidgetMetadata>
    {
        public override bool ResetTransform => true;

        protected override void OnUpdateSubwidget()
        {
            using (_PRF_OnUpdateSubwidget.Auto())
            {
            }
        }
    }
}