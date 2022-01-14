using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.Common.Widgets;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.SideBar
{
    public sealed class DeveloperInterfaceSideBarWidgetMetadata : AreaWidgetMetadata<
        DeveloperInterfaceSideBarWidget, DeveloperInterfaceSideBarWidgetMetadata,
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        #region Fields and Autoproperties

        [OnValueChanged(nameof(InvokeSettingsChanged))]
        [PropertyRange(0.10f, 0.40f)]
        public float width;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(width), () => width = 0.25f);
            }
        }
    }
}
