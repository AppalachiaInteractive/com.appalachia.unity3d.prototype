using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.CommandPalette.Widgets
{
    public sealed class CommandSuggestionsWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<
        CommandSuggestionsWidget, CommandSuggestionsWidgetMetadata, CommandPaletteFeature,
        CommandPaletteFeatureMetadata>
    {
        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Size)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.005f, 0.03f)]
        public float commandSuggestionHeight;

        [BoxGroup(APPASTR.GroupNames.Content)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(1, 20)]
        public int maxSuggestions;

        [BoxGroup(APPASTR.GroupNames.Color)]
        [OnValueChanged(nameof(OnChanged))]
        public Color highlightedSuggestionColor;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            initializer.Do(this, nameof(maxSuggestions),          () => maxSuggestions = 8);
            initializer.Do(this, nameof(commandSuggestionHeight), () => commandSuggestionHeight = 0.02f);
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(CommandSuggestionsWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);
            }
        }
    }
}
