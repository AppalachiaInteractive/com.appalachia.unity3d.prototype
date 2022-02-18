using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets
{
    public sealed class StatusBarWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<
        StatusBarWidget, StatusBarWidgetMetadata, StatusBarFeature, StatusBarFeatureMetadata>
    {
        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Dimensions)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.015f, 0.045f)]
        public float height;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            initializer.Do(this, nameof(height), () => height = 0.03f);
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(StatusBarWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);
            }
        }
    }
}
