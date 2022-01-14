using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.Common.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.CommandPalette;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.CommandSuggestions.Model;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.CommandSuggestions
{
    [CallStaticConstructorInEditor]
    public sealed class DeveloperInterfaceCommandSuggestionsWidget : AreaWidget<
        DeveloperInterfaceCommandSuggestionsWidget, DeveloperInterfaceCommandSuggestionsWidgetMetadata,
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        static DeveloperInterfaceCommandSuggestionsWidget()
        {
            RegisterDependency<DeveloperInterfaceCommandPaletteWidget>(
                i => _developerInterfaceCommandPaletteWidget = i
            );
        }

        #region Static Fields and Autoproperties

        private static DeveloperInterfaceCommandPaletteWidget _developerInterfaceCommandPaletteWidget;

        #endregion

        #region Fields and Autoproperties

        private List<CommandSuggestion> _suggestions;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                _suggestions = new List<CommandSuggestion>();
            }
        }

        protected override void OnApplyMetadataInternal()
        {
        }

        protected override void SubscribeToAllFunctionalties()
        {
            using (_PRF_SubscribeToAllFunctionalties.Auto())
            {
                _developerInterfaceCommandPaletteWidget.VisuallyChanged += ApplyMetadata;
                _developerInterfaceCommandPaletteWidget.CommandPaletteInputModified +=
                    OnCommandPaletteInputModified;
                _developerInterfaceCommandPaletteWidget.CommandPaletteInputSubmitted +=
                    OnCommandPaletteInputSubmitted;
            }
        }

        protected override void UnsubscribeFromAllFunctionalities()
        {
            using (_PRF_UnsubscribeFromAllFunctionalities.Auto())
            {
                _developerInterfaceCommandPaletteWidget.VisuallyChanged -= ApplyMetadata;
            }
        }

        protected override void UpdateSizeInternal()
        {
            using (_PRF_UpdateSizeInternal.Auto())
            {
                var commandPallete = _developerInterfaceCommandPaletteWidget;
                var suggestionCount = Mathf.Min(_suggestions.Count, metadata.maxSuggestions);

                var anchorMin = rectTransform.anchorMin;
                var anchorMax = rectTransform.anchorMax;

                anchorMin.x = commandPallete.rectTransform.anchorMin.x;
                anchorMax.x = commandPallete.rectTransform.anchorMax.x;

                anchorMax.y = commandPallete.rectTransform.anchorMin.y;
                anchorMin.y = anchorMax.y - (suggestionCount * metadata.commandSuggestionHeight);

                rectTransform.anchorMin = anchorMin;
                rectTransform.anchorMax = anchorMax;
            }
        }

        private void OnCommandPaletteInputModified(string input)
        {
            using (_PRF_OnCommandPaletteInputModified.Auto())
            {
                _suggestions.Clear();

                for (var i = 0; i < input.Length; i++)
                {
                    var snippet = input.Substring(i, 1);
                    var newSuggestion = new CommandSuggestion(snippet);
                    _suggestions.Add(newSuggestion);
                }
            }
        }

        private void OnCommandPaletteInputSubmitted(string input)
        {
            using (_PRF_OnCommandPaletteInputSubmitted.Auto())
            {
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_OnCommandPaletteInputModified =
            new ProfilerMarker(_PRF_PFX + nameof(OnCommandPaletteInputModified));

        private static readonly ProfilerMarker _PRF_OnCommandPaletteInputSubmitted =
            new ProfilerMarker(_PRF_PFX + nameof(OnCommandPaletteInputSubmitted));

        #endregion
    }
}
