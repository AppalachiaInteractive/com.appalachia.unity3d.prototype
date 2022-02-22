using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Panel.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Panel
{
    [CallStaticConstructorInEditor]
    public class PanelFeature : DeveloperInterfaceManager_V01.Feature<PanelFeature, PanelFeatureMetadata>
    {
        static PanelFeature()
        {
            FunctionalitySet.RegisterWidget<PanelWidget>(_dependencyTracker, i => _panelWidget = i);
            FunctionalitySet.RegisterFeature<ActivityBarFeature>(
                _dependencyTracker,
                i => _activityBarFeature = i
            );
            FunctionalitySet.RegisterFeature<MenuBarFeature>(_dependencyTracker, i => _menuBarFeature = i);
            FunctionalitySet.RegisterFeature<StatusBarFeature>(
                _dependencyTracker,
                i => _statusBarFeature = i
            );
        }

        #region Static Fields and Autoproperties

        private static ActivityBarFeature _activityBarFeature;
        private static MenuBarFeature _menuBarFeature;
        private static PanelWidget _panelWidget;
        private static StatusBarFeature _statusBarFeature;

        #endregion
    }
}
