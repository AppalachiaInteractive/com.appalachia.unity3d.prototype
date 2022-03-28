using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Controls.Main;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts;
using Appalachia.UI.Functionality.Tooltips.Contracts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets
{
    public sealed partial class StatusBarWidgetMetadata :
        DeveloperInterfaceMetadata_V01.WidgetWithSingletonSubwidgetsMetadata<IStatusBarSubwidget,
            IStatusBarSubwidgetMetadata, StatusBarWidget, StatusBarWidgetMetadata, StatusBarFeature,
            StatusBarFeatureMetadata>,
        ITooltipOwnerConfig
    {
        #region Fields and Autoproperties

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.015f, 0.045f)]
        public float height;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public Color color;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [HideIf(nameof(HideAllFields))]
        public StatusBarControlConfig statusBar;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            initializer.Do(this, nameof(height), () => height = 0.03f);
            initializer.Do(this, nameof(color),  color == Color.clear, () => color = Colors.FromHexCode("CA9A40"));

            StatusBarControlConfig.Refresh(ref statusBar, this);
        }

        /// <inheritdoc />
        protected override void OnApply(StatusBarWidget widget)
        {
            using (_PRF_OnApply.Auto())
            {
                background.IsElected = true;

                background.Value.SolidColor(color);

                base.OnApply(widget);

                statusBar.Apply(widget.statusBar);

                widget.ValidateSubwidgets();

                widget.SortSubwidgetsByPriority();

                statusBar.fontStyle = fontStyle;

                for (var subwidgetIndex = 0; subwidgetIndex < widget.LeftStatusBarSubwidgets.Count; subwidgetIndex++)
                {
                    var subwidget = widget.LeftStatusBarSubwidgets[subwidgetIndex];
                    subwidget.Metadata.UpdateTooltipStyle(TooltipStyle);
                    statusBar.ApplyToSubwidget(subwidget);
                }

                for (var subwidgetIndex = 0; subwidgetIndex < widget.RightStatusBarSubwidgets.Count; subwidgetIndex++)
                {
                    var subwidget = widget.RightStatusBarSubwidgets[subwidgetIndex];

                    subwidget.Metadata.UpdateTooltipStyle(TooltipStyle);
                    statusBar.ApplyToSubwidget(subwidget);
                }
            }
        }

        protected override void SubscribeResponsiveComponents(StatusBarWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                statusBar.SubscribeToChanges(OnChanged);
                rectTransform.SubscribeToChanges(OnChanged);
            }
        }

        protected override void SuspendResponsiveComponents(StatusBarWidget target)
        {
            using (_PRF_SuspendResponsiveComponents.Auto())
            {
                base.SuspendResponsiveComponents(target);

                statusBar.SuspendChanges();
                rectTransform.SuspendChanges();
            }
        }

        protected override void UnsuspendResponsiveComponents(StatusBarWidget target)
        {
            using (_PRF_UnsuspendResponsiveComponents.Auto())
            {
                base.UnsuspendResponsiveComponents(target);

                statusBar.UnsuspendChanges();
                rectTransform.UnsuspendChanges();
            }
        }
    }
}
