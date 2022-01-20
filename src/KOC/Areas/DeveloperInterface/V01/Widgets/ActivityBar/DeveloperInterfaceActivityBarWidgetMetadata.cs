using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.UI.Controls.Sets.Button;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.ActivityBar
{
    public sealed class DeveloperInterfaceActivityBarWidgetMetadata : DeveloperInterfaceWidgetMetadata<
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
        public ButtonComponentSetStyle buttonStyle;

        #endregion

        public override void Apply(DeveloperInterfaceActivityBarWidget functionality)
        {
            using (_PRF_Apply.Auto())
            {
                base.Apply(functionality);

                for (var index = 0; index < functionality.Buttons.Count; index++)
                {
                    var button = functionality.Buttons[index];
                    var entry = functionality.Entries[index];

                    buttonStyle.ConfigureComponents(button);

                    button.ButtonIcon.sprite = entry.sprite;
                    button.TooltipData.Text = entry.tooltip;
                }
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(width), () => width = 0.03f);

                initializer.Do(
                    this,
                    nameof(ButtonComponentSetStyle),
                    buttonStyle == null,
                    () =>
                    {
                        buttonStyle = AppalachiaObject.LoadOrCreateNew<ButtonComponentSetStyle>(
                            nameof(DeveloperInterfaceActivityBarWidget) + "ButtonComponentSetStyle",
                            ownerType: typeof(ApplicationManager)
                        );
                    }
                );

                buttonStyle.SettingsChanged += _ => InvokeSettingsChanged();
            }
        }
    }
}
