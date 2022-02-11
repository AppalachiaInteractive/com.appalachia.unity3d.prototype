using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Model;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar.Model;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar.Widgets;
using Appalachia.Utility.Async;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar
{
    [CallStaticConstructorInEditor]
    public class MenuBarFeature : DeveloperInterfaceManager_V01.Feature<MenuBarFeature,
                                      MenuBarFeatureMetadata>,
                                  IActivityBarEntryProvider
    {
        static MenuBarFeature()
        {
            FunctionalitySet.RegisterWidget<MenuBarWidget>(_dependencyTracker, i => _menuBarWidget = i);
            FunctionalitySet.RegisterFeature<ActivityBarFeature>(
                _dependencyTracker,
                i => _activityBarFeature = i
            );

            When.Any<IMenuBarEntryProvider>().IsAvailableThen(RegisterMenuProvider);
        }

        #region Static Fields and Autoproperties

        private static ActivityBarFeature _activityBarFeature;

        private static MenuBarWidget _menuBarWidget;

        #endregion

        #region Fields and Autoproperties

        [NonSerialized] private static List<MenuBarEntry> _leftEntries;
        [NonSerialized] private static List<MenuBarEntry> _rightEntries;

        #endregion

        public IReadOnlyList<MenuBarEntry> LeftEntries
        {
            get
            {
                _leftEntries ??= new List<MenuBarEntry>();
                return _leftEntries;
            }
        }

        public IReadOnlyList<MenuBarEntry> RightEntries
        {
            get
            {
                _rightEntries ??= new List<MenuBarEntry>();
                return _rightEntries;
            }
        }

        private static void RegisterMenu(MenuBarEntry menu)
        {
            using (_PRF_RegisterMenu.Auto())
            {
                if (menu.clampToLeft)
                {
                    _leftEntries.Add(menu);
                }
                else
                {
                    _rightEntries.Add(menu);
                    _rightEntries.Sort((entry1, entry2) => entry1.priority.CompareTo(entry2));
                }
            }
        }

        private static void RegisterMenuProvider(IMenuBarEntryProvider provider)
        {
            using (_PRF_RegisterMenuProvider.Auto())
            {
                var menuEntries = provider.GetMenuBarEntries();

                if (menuEntries == null)
                {
                    return;
                }

                foreach (var menu in menuEntries)
                {
                    RegisterMenu(menu);
                }
            }
        }

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
                await ShowFeature();
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
                _menuBarWidget.Hide();

                await AppaTask.CompletedTask;
            }
        }

        protected override async AppaTask OnShow()
        {
            using (_PRF_OnShow.Auto())
            {
                _menuBarWidget.Show();

                await AppaTask.CompletedTask;
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterMenu =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterMenu));

        private static readonly ProfilerMarker _PRF_RegisterMenuProvider =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterMenuProvider));

        #endregion

        private static readonly ProfilerMarker _PRF_GetActivityBarEntries =
            new ProfilerMarker(_PRF_PFX + nameof(GetActivityBarEntries));

        public ActivityBarEntry[] GetActivityBarEntries()
        {
            using (_PRF_GetActivityBarEntries.Auto())
            {
                return Array.Empty<ActivityBarEntry>();
            }
        }
    }
}
