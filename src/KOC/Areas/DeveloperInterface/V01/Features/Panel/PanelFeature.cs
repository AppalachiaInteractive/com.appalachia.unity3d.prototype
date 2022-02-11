using System;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Model;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Panel.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar;
using Appalachia.Utility.Async;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Panel
{
    [CallStaticConstructorInEditor]
    public class PanelFeature : DeveloperInterfaceManager_V01.Feature<PanelFeature, PanelFeatureMetadata>,
                                IActivityBarEntryProvider
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

        protected override async AppaTask BeforeDisable()
        {
            using (_PRF_BeforeDisable.Auto())
            {
                await HideFeature();
            }
        }

        protected override async AppaTask BeforeEnable()
        {
            using (_PRF_BeforeEnable.Auto())
            {
                await AppaTask.CompletedTask;
            }
        }

        protected override async AppaTask BeforeFirstEnable()
        {
            using (_PRF_BeforeFirstEnable.Auto())
            {
                await AppaTask.CompletedTask;
            }
        }

        protected override async AppaTask OnHide()
        {
            using (_PRF_OnHide.Auto())
            {
                _panelWidget.Hide();
                await AppaTask.CompletedTask;
            }
        }

        protected override async AppaTask OnShow()
        {
            using (_PRF_OnShow.Auto())
            {
                _panelWidget.Show();
                await AppaTask.CompletedTask;
            }
        }

        #region IActivityBarEntryProvider Members

        public ActivityBarEntry[] GetActivityBarEntries()
        {
            using (_PRF_GetActivityBarEntries.Auto())
            {
                return Array.Empty<ActivityBarEntry>();
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetActivityBarEntries =
            new ProfilerMarker(_PRF_PFX + nameof(GetActivityBarEntries));

        #endregion
    }
}
