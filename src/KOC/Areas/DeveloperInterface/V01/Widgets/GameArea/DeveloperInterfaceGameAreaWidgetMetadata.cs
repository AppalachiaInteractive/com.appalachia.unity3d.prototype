using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.Common.Widgets;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.GameArea
{
    public sealed class DeveloperInterfaceGameAreaWidgetMetadata : AreaWidgetMetadata<
        DeveloperInterfaceGameAreaWidget, DeveloperInterfaceGameAreaWidgetMetadata,
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        #region Fields and Autoproperties

        [OnValueChanged(nameof(InvokeSettingsChanged))]
        [ToggleLeft]
        public bool maintainAspectRatio;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            initializer.Do(this, nameof(maintainAspectRatio), () => maintainAspectRatio = true);
        }
    }
}
