using Appalachia.Core.Attributes;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.CommandPalette.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.CommandPalette
{
    [CallStaticConstructorInEditor]
    public class CommandPaletteFeature : DeveloperInterfaceManager_V01.Feature<CommandPaletteFeature,
        CommandPaletteFeatureMetadata>
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

            FunctionalitySet.RequireFeature<ActivityBarFeature>(
                _dependencyTracker,
                i => _activityBarFeature = i
            );
            FunctionalitySet.RequireFeature<StatusBarFeature>(
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
    }
}
