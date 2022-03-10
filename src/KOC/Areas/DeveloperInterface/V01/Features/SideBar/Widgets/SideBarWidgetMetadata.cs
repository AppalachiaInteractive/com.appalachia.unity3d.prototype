using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Subwidgets.Contracts;
using Appalachia.UI.Controls.Sets.Layout.Foldout.Styling;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar.Widgets
{
    public sealed class SideBarWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetWithSingletonSubwidgetsMetadata<
        ISideBarSubwidget, ISideBarSubwidgetMetadata, SideBarWidget, SideBarWidgetMetadata, SideBarFeature,
        SideBarFeatureMetadata>
    {
        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Size)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.10f, 0.40f)]
        [SerializeField]
        public float width;

        [SerializeField]
        [OnValueChanged(nameof(OnChanged))]
        public FoldoutStyleOverride foldoutStyle;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(width), () => width = 0.25f);
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(SideBarWidget target)
        {
            base.SubscribeResponsiveComponents(target);
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(SideBarWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);

                widget.ValidateSubwidgets();
            }
        }
    }
}
