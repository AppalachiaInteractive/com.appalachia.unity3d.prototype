using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.CommandPalette.Widgets
{
    public sealed class CommandEntryWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<
        CommandEntryWidget, CommandEntryWidgetMetadata, CommandPaletteFeature, CommandPaletteFeatureMetadata>
    {
        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Dimensions)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.3f, 0.5f)]
        public float width;

        [BoxGroup(APPASTR.GroupNames.Dimensions)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.02, 0.05f)]
        public float height;

        [BoxGroup(APPASTR.GroupNames.Dimensions)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.4f, 0.6f)]
        public float horizontalCenter;

        #endregion

        protected override void UpdateFunctionality(CommandEntryWidget widget)
        {
            using (_PRF_Apply.Auto())
            {
                base.UpdateFunctionality(widget);
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            initializer.Do(this, nameof(width),            () => width = 0.4f);
            initializer.Do(this, nameof(height),           () => height = 0.02f);
            initializer.Do(this, nameof(horizontalCenter), () => horizontalCenter = 0.5f);
        }
    }
}
