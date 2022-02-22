using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Widgets;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar
{
    [CallStaticConstructorInEditor]
    public class SideBarFeature : DeveloperInterfaceManager_V01.Feature<SideBarFeature,
        SideBarFeatureMetadata>
    {
        static SideBarFeature()
        {
            FunctionalitySet.RegisterWidget<SideBarWidget>(_dependencyTracker, i => _sideBarWidget = i);
        }

        #region Static Fields and Autoproperties

        private static SideBarWidget _sideBarWidget;

        #endregion
    }
}
