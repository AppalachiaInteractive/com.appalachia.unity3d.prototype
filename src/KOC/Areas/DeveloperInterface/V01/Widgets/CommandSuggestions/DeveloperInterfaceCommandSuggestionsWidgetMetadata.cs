using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.Common.Widgets;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.CommandSuggestions
{
    public sealed class DeveloperInterfaceCommandSuggestionsWidgetMetadata : AreaWidgetMetadata<
        DeveloperInterfaceCommandSuggestionsWidget, DeveloperInterfaceCommandSuggestionsWidgetMetadata,
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Dimensions)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        [PropertyRange(0.005f, 0.03f)]
        public float commandSuggestionHeight;

        [BoxGroup(APPASTR.GroupNames.Content)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        [PropertyRange(1, 20)]
        public int maxSuggestions;

        [BoxGroup(APPASTR.GroupNames.Color)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public Color highlightedSuggestionColor;

        #endregion

        public override void Apply(DeveloperInterfaceCommandSuggestionsWidget functionality)
        {
            using (_PRF_Apply.Auto())
            {
                base.Apply(functionality);
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            initializer.Do(this, nameof(maxSuggestions),          () => maxSuggestions = 8);
            initializer.Do(this, nameof(commandSuggestionHeight), () => commandSuggestionHeight = 0.02f);
        }
    }
}
