using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets
{
    public sealed class ActivityBarWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<
        ActivityBarWidget, ActivityBarWidgetMetadata, ActivityBarFeature, ActivityBarFeatureMetadata>
    {
        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Size)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.03f, 0.07f)]
        public float width;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(width), () => width = 0.03f);
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(ActivityBarWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(ActivityBarWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);
            }
        }
    }
}
