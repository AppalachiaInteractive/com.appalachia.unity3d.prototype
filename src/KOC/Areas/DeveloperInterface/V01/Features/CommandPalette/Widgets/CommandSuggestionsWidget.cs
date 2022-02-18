using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Events;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.CommandPalette.Widgets.Model;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.CommandPalette.Widgets
{
    [CallStaticConstructorInEditor]
    public sealed class CommandSuggestionsWidget : DeveloperInterfaceManager_V01.Widget<
        CommandSuggestionsWidget, CommandSuggestionsWidgetMetadata, CommandPaletteFeature,
        CommandPaletteFeatureMetadata>
    {
        static CommandSuggestionsWidget()
        {
            When.Widget(_commandEntryWidget)
                .IsAvailableThen(commandEntryWidget => _commandEntryWidget = commandEntryWidget);
        }

        #region Static Fields and Autoproperties

        private static CommandEntryWidget _commandEntryWidget;

        #endregion

        #region Fields and Autoproperties

        private List<CommandSuggestion> _suggestions;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask DelayEnabling()
        {
            await base.DelayEnabling();

            await AppaTask.WaitUntil(() => _commandEntryWidget != null);
        }

        /// <inheritdoc />
        protected override void EnsureWidgetIsCorrectSize()
        {
            using (_PRF_EnsureWidgetIsCorrectSize.Auto())
            {
                base.EnsureWidgetIsCorrectSize();

                var commandPallete = _commandEntryWidget;
                var suggestionCount = Mathf.Min(_suggestions.Count, metadata.maxSuggestions);

                var anchorMin = RectTransform.anchorMin;
                var anchorMax = RectTransform.anchorMax;

                anchorMin.x = commandPallete.RectTransform.anchorMin.x;
                anchorMax.x = commandPallete.RectTransform.anchorMax.x;

                anchorMax.y = commandPallete.RectTransform.anchorMin.y;
                anchorMin.y = anchorMax.y - (suggestionCount * metadata.commandSuggestionHeight);

                UpdateAnchorMin(anchorMin);
                UpdateAnchorMax(anchorMax);
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                _suggestions = new List<CommandSuggestion>();
            }
        }

        /// <inheritdoc />
        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
                base.UnsubscribeFromAllFunctionalities();

                _commandEntryWidget.VisuallyChanged.Event -= OnDependencyChanged;
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                _commandEntryWidget.VisuallyChanged.Event += OnDependencyChanged;
                _commandEntryWidget.CommandPaletteInputModified.Event += OnCommandEntryInputModified;
                _commandEntryWidget.CommandPaletteInputSubmitted.Event += OnCommandEntryInputSubmitted;
            }
        }

        private void OnCommandEntryInputModified(ValueEvent<string>.Args args)
        {
            using (_PRF_OnCommandPaletteInputModified.Auto())
            {
                var input = args.value;
                _suggestions.Clear();

                for (var i = 0; i < input.Length; i++)
                {
                    var snippet = input.Substring(i, 1);
                    var newSuggestion = new CommandSuggestion(snippet);
                    _suggestions.Add(newSuggestion);
                }
            }
        }

        private void OnCommandEntryInputSubmitted(ValueEvent<string>.Args args)
        {
            using (_PRF_OnCommandPaletteInputSubmitted.Auto())
            {
                var input = args.value;
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_OnCommandPaletteInputModified =
            new ProfilerMarker(_PRF_PFX + nameof(OnCommandEntryInputModified));

        private static readonly ProfilerMarker _PRF_OnCommandPaletteInputSubmitted =
            new ProfilerMarker(_PRF_PFX + nameof(OnCommandEntryInputSubmitted));

        #endregion
    }
}
