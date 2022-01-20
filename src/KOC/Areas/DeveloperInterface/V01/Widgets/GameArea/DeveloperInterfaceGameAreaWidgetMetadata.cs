using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.GameArea
{
    public sealed class DeveloperInterfaceGameAreaWidgetMetadata : DeveloperInterfaceWidgetMetadata<
        DeveloperInterfaceGameAreaWidget, DeveloperInterfaceGameAreaWidgetMetadata,
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Dimensions)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public bool maintainAspectRatio;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(maintainAspectRatio), () => maintainAspectRatio = true);
            }
        }
    }
}
