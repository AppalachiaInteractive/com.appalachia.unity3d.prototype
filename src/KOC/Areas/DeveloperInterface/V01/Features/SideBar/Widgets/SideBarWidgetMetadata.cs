using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Widgets
{
    public sealed class SideBarWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<SideBarWidget,
        SideBarWidgetMetadata, SideBarFeature, SideBarFeatureMetadata>
    {
        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Dimensions)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.10f, 0.40f)]
        public float width;

        #endregion

        protected override void UpdateFunctionality(SideBarWidget widget)
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
                initializer.Do(this, nameof(width), () => width = 0.25f);
            }
        }

        protected override void SubscribeResponsiveComponents(SideBarWidget target)
        {
            base.SubscribeResponsiveComponents(target);
        }
    }
}
