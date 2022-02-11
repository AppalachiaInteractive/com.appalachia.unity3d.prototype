using System;
using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.CommandPalette.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Model;
using Appalachia.Utility.Async;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.CommandPalette
{
    [CallStaticConstructorInEditor]
    public class CommandPaletteFeature : DeveloperInterfaceManager_V01.Feature<CommandPaletteFeature,
                                             CommandPaletteFeatureMetadata>,
                                         IStatusBarEntryProvider
    {
        static CommandPaletteFeature()
        {
            FunctionalitySet.RegisterWidget<CommandEntryWidget>(
                _dependencyTracker,
                i => _commandEntryWidget = i
            );

            FunctionalitySet.RegisterWidget<CommandSuggestionsWidget>(
                _dependencyTracker,
                i => _commandSuggestionsWidget = i
            );

            FunctionalitySet.RegisterFeature<ActivityBarFeature>(
                _dependencyTracker,
                i => _activityBarFeature = i
            );
            FunctionalitySet.RegisterFeature<StatusBarFeature>(
                _dependencyTracker,
                i => _statusBarFeature = i
            );
        }

        #region Static Fields and Autoproperties

        private static ActivityBarFeature _activityBarFeature;

        private static CommandEntryWidget _commandEntryWidget;
        private static CommandSuggestionsWidget _commandSuggestionsWidget;
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
                _commandEntryWidget.Hide();
                _commandSuggestionsWidget.Hide();
                await AppaTask.CompletedTask;
            }
        }

        protected override async AppaTask OnShow()
        {
            using (_PRF_OnShow.Auto())
            {
                _commandEntryWidget.Show();
                await AppaTask.CompletedTask;
            }
        }

        #region IStatusBarEntryProvider Members

        public StatusBarEntry[] GetStatusBarEntries()
        {
            using (_PRF_GetStatusBarEntries.Auto())
            {
                return Array.Empty<StatusBarEntry>();
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_GetStatusBarEntries =
            new ProfilerMarker(_PRF_PFX + nameof(GetStatusBarEntries));

        #endregion
    }
}
