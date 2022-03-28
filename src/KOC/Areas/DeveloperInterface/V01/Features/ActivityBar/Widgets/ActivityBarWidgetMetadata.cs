using System.Linq;
using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Controls.Main;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Contracts;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.UI.Functionality.Tooltips.Contracts;
using Appalachia.UI.Functionality.Tooltips.Styling;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets
{
    public sealed partial class ActivityBarWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetWithSingletonSubwidgetsMetadata
                                                    <IActivityBarSubwidget, IActivityBarSubwidgetMetadata,
                                                        ActivityBarWidget, ActivityBarWidgetMetadata, ActivityBarFeature
                                                        , ActivityBarFeatureMetadata>
    {
        #region Fields and Autoproperties

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.03f, 0.07f)]
        public float width;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public Color color;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public ActivityBarControlConfig activityBar;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            initializer.Do(this, nameof(width), () => width = 0.03f);
            initializer.Do(this, nameof(color), color == Color.clear, () => color = Colors.FromHexCode("333333"));

            ActivityBarControlConfig.Refresh(ref activityBar, this);
        }

        /// <inheritdoc />
        protected override void OnApply(ActivityBarWidget widget)
        {
            using (_PRF_OnApply.Auto())
            {
                background.IsElected = true;

                var firstBackground = background.Value.ConfigList.FirstOrDefault();

                if (firstBackground == null)
                {
                    firstBackground = ImageComponentGroupConfig.CreateWithOwner(this);
                    background.Value.ConfigList.Add(firstBackground);
                }

                firstBackground.image.color.OverrideValue(color);

                base.OnApply(widget);

                activityBar.Apply(widget.activityBar, this);

                widget.ValidateSubwidgets();

                widget.SortSubwidgetsByPriority();

                for (var subwidgetIndex = 0; subwidgetIndex < widget.TopActivityBarSubwidgets.Count; subwidgetIndex++)
                {
                    var subwidget = widget.TopActivityBarSubwidgets[subwidgetIndex];
                    
                    subwidget.Metadata.UpdateTooltipStyle(TooltipStyle);
                    activityBar.ApplyToSubwidget(subwidget);
                }

                for (var subwidgetIndex = 0;
                     subwidgetIndex < widget.BottomActivityBarSubwidgets.Count;
                     subwidgetIndex++)
                {
                    var subwidget = widget.BottomActivityBarSubwidgets[subwidgetIndex];

                    subwidget.Metadata.UpdateTooltipStyle(TooltipStyle);
                    activityBar.ApplyToSubwidget(subwidget);
                }
            }
        }

        protected override void SubscribeResponsiveComponents(ActivityBarWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                activityBar.SubscribeToChanges(OnChanged);
                rectTransform.SubscribeToChanges(OnChanged);
            }
        }

        protected override void UnsuspendResponsiveComponents(ActivityBarWidget target)
        {
            using (_PRF_UnsuspendResponsiveComponents.Auto())
            {
                base.UnsuspendResponsiveComponents(target);

                activityBar.UnsuspendChanges();
                rectTransform.UnsuspendChanges();
            }
        }

        protected override void SuspendResponsiveComponents(ActivityBarWidget target)
        {
            using (_PRF_SuspendResponsiveComponents.Auto())
            {
                base.SuspendResponsiveComponents(target);

                activityBar.SuspendChanges();
                rectTransform.SuspendChanges();
            }
        }
    }
}
