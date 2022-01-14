using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.Common.Widgets;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.Panel
{
    public sealed class DeveloperInterfacePanelWidgetMetadata : AreaWidgetMetadata<
        DeveloperInterfacePanelWidget, DeveloperInterfacePanelWidgetMetadata, DeveloperInterfaceManager_V01,
        DeveloperInterfaceMetadata_V01>
    {
        #region Fields and Autoproperties

        [OnValueChanged(nameof(InvokeSettingsChanged))]
        [PropertyRange(0.25f, 0.35f)]
        public float height;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(height), () => height = 0.3f);
            }
        }
    }
}
