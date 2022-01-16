using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.Common.Widgets;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.ActivityBar
{
    public sealed class DeveloperInterfaceActivityBarWidgetMetadata : AreaWidgetMetadata<
        DeveloperInterfaceActivityBarWidget, DeveloperInterfaceActivityBarWidgetMetadata,
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Dimensions)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        [PropertyRange(0.015f, 0.045f)]
        public float width;

        [BoxGroup(APPASTR.GroupNames.Style)]
        [OnValueChanged(nameof(InvokeSettingsChanged))]
        public ButtonStyleOverride iconStyle;

        #endregion

        public override void Apply(DeveloperInterfaceActivityBarWidget functionality)
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
                initializer.Do(this, nameof(width), () => width = 0.03f);

                iconStyle.SettingsChanged += _ => InvokeSettingsChanged();
            }
        }
    }
}
