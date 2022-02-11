using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Panel.Widgets
{
    public sealed class PanelWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<PanelWidget,
        PanelWidgetMetadata, PanelFeature, PanelFeatureMetadata>
    {
        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Dimensions)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.25f, 0.35f)]
        public float height;

        #endregion

        protected override void UpdateFunctionality(PanelWidget widget)
        {
            using (_PRF_Apply.Auto())
            {
                base.UpdateFunctionality(widget);
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
