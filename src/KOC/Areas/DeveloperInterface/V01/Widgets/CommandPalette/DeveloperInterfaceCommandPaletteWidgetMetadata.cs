using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.Common.Widgets;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.CommandPalette
{
    public sealed class DeveloperInterfaceCommandPaletteWidgetMetadata : AreaWidgetMetadata<
        DeveloperInterfaceCommandPaletteWidget, DeveloperInterfaceCommandPaletteWidgetMetadata,
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        #region Fields and Autoproperties

        [OnValueChanged(nameof(InvokeSettingsChanged))]
        [PropertyRange(0.3f, 0.5f)]
        public float width;

        [OnValueChanged(nameof(InvokeSettingsChanged))]
        [PropertyRange(0.005f, 0.03f)]
        public float height;

        [OnValueChanged(nameof(InvokeSettingsChanged))]
        [PropertyRange(0.4f, 0.6f)]
        public float horizontalCenter;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            initializer.Do(this, nameof(width),            () => width = 0.4f);
            initializer.Do(this, nameof(height),           () => height = 0.02f);
            initializer.Do(this, nameof(horizontalCenter), () => horizontalCenter = 0.5f);
        }
    }
}
