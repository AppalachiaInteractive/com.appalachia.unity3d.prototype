using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar.Widgets
{
    public sealed class MenuBarWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<MenuBarWidget,
        MenuBarWidgetMetadata, MenuBarFeature, MenuBarFeatureMetadata>
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
        protected override void UpdateFunctionalityInternal(MenuBarWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);
            }
        }
    }
}
