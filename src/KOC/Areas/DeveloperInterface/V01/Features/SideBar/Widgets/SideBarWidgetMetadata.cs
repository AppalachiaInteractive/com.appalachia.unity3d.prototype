using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Contracts;
using Appalachia.UI.Functionality.Foldout.Styling;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Widgets
{
    public sealed partial class SideBarWidgetMetadata : DeveloperInterfaceMetadata_V01.
        WidgetWithSingletonSubwidgetsMetadata<ISideBarSubwidget, ISideBarSubwidgetMetadata, SideBarWidget,
            SideBarWidgetMetadata, SideBarFeature, SideBarFeatureMetadata>
    {
        #region Fields and Autoproperties

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.10f, 0.40f)]
        [SerializeField]
        public float width;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public Color color;

        [SerializeField]
        [OnValueChanged(nameof(OnChanged))]
        public FoldoutStyleTypes foldoutStyle;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                canvas.Value.HideAllFields = false;
                background.Value.HideAllFields = false;
                roundedBackground.Value.HideAllFields = false;
                rectTransform.Value.HideAllFields = false;

                initializer.Do(this, nameof(width), width == 0f,          () => width = 0.25f);
                initializer.Do(this, nameof(color), color == Color.clear, () => color = Colors.FromHexCode("252526"));
            }
        }

        /// <inheritdoc />
        protected override void OnApply(SideBarWidget widget)
        {
            using (_PRF_OnApply.Auto())
            {
                background.IsElected = true;
                background.Value.SolidColor(color);

                base.OnApply(widget);

                widget.ValidateSubwidgets();
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(SideBarWidget target)
        {
            base.SubscribeResponsiveComponents(target);
        }
    }
}
