using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.Panel
{
    public sealed class DeveloperInterfacePanelWidgetMetadata : DeveloperInterfaceWidgetMetadata<
        DeveloperInterfacePanelWidget, DeveloperInterfacePanelWidgetMetadata, DeveloperInterfaceManager_V01,
        DeveloperInterfaceMetadata_V01>
    {
        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Dimensions)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        [PropertyRange(0.25f, 0.35f)]
        public float height;

        #endregion

        public override void Apply(DeveloperInterfacePanelWidget functionality)
        {
            using (_PRF_Apply.Auto())
            {
                base.Apply(functionality);
            }
        }

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
