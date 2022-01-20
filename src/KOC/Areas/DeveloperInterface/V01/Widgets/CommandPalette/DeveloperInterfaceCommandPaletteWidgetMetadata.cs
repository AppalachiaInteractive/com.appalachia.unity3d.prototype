using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.CommandPalette
{
    public sealed class DeveloperInterfaceCommandPaletteWidgetMetadata : DeveloperInterfaceWidgetMetadata<
        DeveloperInterfaceCommandPaletteWidget, DeveloperInterfaceCommandPaletteWidgetMetadata,
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Dimensions)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        [PropertyRange(0.3f, 0.5f)]
        public float width;

        [BoxGroup(APPASTR.GroupNames.Dimensions)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        [PropertyRange(0.02, 0.05f)]
        public float height;

        [BoxGroup(APPASTR.GroupNames.Dimensions)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        [PropertyRange(0.4f, 0.6f)]
        public float horizontalCenter;

        #endregion

        public override void Apply(DeveloperInterfaceCommandPaletteWidget functionality)
        {
            using (_PRF_Apply.Auto())
            {
                base.Apply(functionality);
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
