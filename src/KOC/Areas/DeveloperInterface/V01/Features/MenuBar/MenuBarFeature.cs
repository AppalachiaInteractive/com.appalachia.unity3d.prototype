using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar.Widgets;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar
{
    [CallStaticConstructorInEditor]
    public class MenuBarFeature : DeveloperInterfaceManager_V01.Feature<MenuBarFeature,
        MenuBarFeatureMetadata>
    {
        static MenuBarFeature()
        {
            FunctionalitySet.RegisterWidget<MenuBarWidget>(_dependencyTracker, i => _menuBarWidget = i);
        }

        #region Static Fields and Autoproperties

        private static MenuBarWidget _menuBarWidget;

        #endregion
    }
}
