using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Styling;
using Appalachia.UI.Controls.Common;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Components.Subsets;
using Appalachia.UI.Core.Extensions;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Widgets
{
    public sealed class ActivityBarWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetWithSingletonSubwidgetsMetadata
    <IActivityBarSubwidget, IActivityBarSubwidgetMetadata, ActivityBarWidget, ActivityBarWidgetMetadata,
        ActivityBarFeature, ActivityBarFeatureMetadata>
    {
        #region Constants and Static Readonly

        public static readonly string BottomLayoutGroupSubwidgetsParentName =
            "Bottom " + ActivityBarWidget.SubwidgetParentName;

        public static readonly string TopLayoutGroupSubwidgetsParentName =
            "Top " + ActivityBarWidget.SubwidgetParentName;

        #endregion

        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Size)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.03f, 0.07f)]
        public float width;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [HideInInspector]
        public VerticalLayoutGroupSubsetData topLayoutGroup;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [HideInInspector]
        public VerticalLayoutGroupSubsetData bottomLayoutGroup;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [HideInInspector]
        public RectTransformData activityBarIconRectTransform;

        [BoxGroup(APPASTR.GroupNames.Size)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(8, 100)]
        public int activityBarPaddingTop;

        [BoxGroup(APPASTR.GroupNames.Size)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(0, 100)]
        public int activityBarSpacingTop;

        [BoxGroup(APPASTR.GroupNames.Size)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(8, 100)]
        public int activityBarPaddingBottom;

        [BoxGroup(APPASTR.GroupNames.Size)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(0, 100)]
        public int activityBarSpacingBottom;

        [BoxGroup(APPASTR.GroupNames.Content)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(8, 64)]
        public int iconSize;

        [BoxGroup(APPASTR.GroupNames.Content)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(8, 64)]
        public int iconPadding;

        [BoxGroup(APPASTR.GroupNames.Content)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public float selectionIndicatorSize;

        [BoxGroup(APPASTR.GroupNames.Content)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public Color selectedIndicatorColor;

        [BoxGroup(APPASTR.GroupNames.Content)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public Color unselectedIndicatorColor;

        [BoxGroup(APPASTR.GroupNames.Content)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public Color hoveringIndicatorColor;

        [BoxGroup(APPASTR.GroupNames.Content)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public AppearanceDirection selectionIndicatorDirection;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public Sprite defaultActivityBarIcon;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            initializer.Do(this, nameof(width), () => width = 0.03f);

            initializer.Do(
                this,
                nameof(activityBarPaddingTop),
                activityBarPaddingTop == 0,
                () => activityBarPaddingTop = 50
            );
            initializer.Do(
                this,
                nameof(activityBarPaddingBottom),
                activityBarPaddingBottom == 0,
                () => activityBarPaddingBottom = 50
            );
            initializer.Do(
                this,
                nameof(activityBarSpacingTop),
                activityBarSpacingTop == 0,
                () => activityBarSpacingTop = 10
            );
            initializer.Do(
                this,
                nameof(activityBarSpacingBottom),
                activityBarSpacingBottom == 0,
                () => activityBarSpacingBottom = 10
            );

            initializer.Do(this, nameof(iconPadding), iconPadding == 0, () => iconPadding = 10);
            initializer.Do(this, nameof(iconSize),    iconSize == 0,    () => iconSize = 24);

            initializer.Do(
                this,
                nameof(topLayoutGroup),
                topLayoutGroup == null,
                () => topLayoutGroup = new VerticalLayoutGroupSubsetData(this)
            );

            initializer.Do(
                this,
                nameof(bottomLayoutGroup),
                bottomLayoutGroup == null,
                () => bottomLayoutGroup = new VerticalLayoutGroupSubsetData(this)
            );

            initializer.Do(
                this,
                nameof(activityBarIconRectTransform),
                activityBarIconRectTransform == null,
                () => activityBarIconRectTransform = new RectTransformData(this)
            );

            initializer.Do(
                this,
                nameof(devTooltipStyle),
                devTooltipStyle == null,
                () =>
                {
                    devTooltipStyle = LoadOrCreateNew<DevTooltipStyleOverride>(
                        GetAssetName<DevTooltipStyleOverride>(),
                        ownerType: typeof(ApplicationManager)
                    );
                }
            );
        }

        protected override void SubscribeResponsiveComponents(ActivityBarWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                topLayoutGroup.Changed.Event += OnChanged;
                bottomLayoutGroup.Changed.Event += OnChanged;
                rectTransform.Changed.Event += OnChanged;
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(ActivityBarWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);

                if (bottomLayoutGroup != null)
                {
                    bottomLayoutGroup.VerticalLayoutGroup.childControlWidth.OverrideValue(true);
                    bottomLayoutGroup.VerticalLayoutGroup.childControlHeight.OverrideValue(false);

                    bottomLayoutGroup.VerticalLayoutGroup.childForceExpandWidth.OverrideValue(true);
                    bottomLayoutGroup.VerticalLayoutGroup.childForceExpandHeight.OverrideValue(false);

                    bottomLayoutGroup.VerticalLayoutGroup.childScaleWidth.OverrideValue(false);
                    bottomLayoutGroup.VerticalLayoutGroup.childScaleHeight.OverrideValue(false);

                    bottomLayoutGroup.VerticalLayoutGroup.padding.Overriding = true;
                    bottomLayoutGroup.VerticalLayoutGroup.padding.Value.bottom = activityBarPaddingBottom;
                    bottomLayoutGroup.VerticalLayoutGroup.spacing.Value = activityBarSpacingBottom;
                }

                if (topLayoutGroup != null)
                {
                    topLayoutGroup.VerticalLayoutGroup.childControlWidth.OverrideValue(true);
                    topLayoutGroup.VerticalLayoutGroup.childControlHeight.OverrideValue(false);

                    topLayoutGroup.VerticalLayoutGroup.childForceExpandWidth.OverrideValue(true);
                    topLayoutGroup.VerticalLayoutGroup.childForceExpandHeight.OverrideValue(false);

                    topLayoutGroup.VerticalLayoutGroup.childScaleWidth.OverrideValue(false);
                    topLayoutGroup.VerticalLayoutGroup.childScaleHeight.OverrideValue(false);

                    topLayoutGroup.VerticalLayoutGroup.padding.Overriding = true;
                    topLayoutGroup.VerticalLayoutGroup.padding.Value.top = activityBarPaddingTop;
                    topLayoutGroup.VerticalLayoutGroup.spacing.Value = activityBarSpacingTop;
                }

                VerticalLayoutGroupSubsetData.RefreshAndApply(
                    ref bottomLayoutGroup,
                    this,
                    ref widget.bottomActivityBarLayoutGroup,
                    widget.SubwidgetParent,
                    BottomLayoutGroupSubwidgetsParentName
                );

                VerticalLayoutGroupSubsetData.RefreshAndApply(
                    ref topLayoutGroup,
                    this,
                    ref widget.topActivityBarLayoutGroup,
                    widget.SubwidgetParent,
                    TopLayoutGroupSubwidgetsParentName
                );

                widget.ValidateSubwidgets();

                widget.SortSubwidgetsByPriority();

                activityBarIconRectTransform.AnchorCenter().PivotCenter().SetSize(iconSize, iconSize);

                for (var subwidgetIndex = 0; subwidgetIndex < widget.TopActivityBarSubwidgets.Count; subwidgetIndex++)
                {
                    var subwidget = widget.TopActivityBarSubwidgets[subwidgetIndex];

                    subwidget.Metadata.Button.SelectedIndicatorColor = selectedIndicatorColor;
                    subwidget.Metadata.Button.HoveringIndicatorColor = hoveringIndicatorColor;
                    subwidget.Metadata.Button.UnselectedIndicatorColor = unselectedIndicatorColor;
                    subwidget.Metadata.Button.SelectionIndicatorDirection = selectionIndicatorDirection;
                    subwidget.Metadata.Button.SelectionIndicatorSize = selectionIndicatorSize;
                    
                    /*var swLayoutGroup = subwidget.Metadata.Button.LayoutGroup;

                    swLayoutGroup.IsElected = false;
                    swLayoutGroup.BindValueEnabledState();*/

                    subwidget.ApplyMetadata();

                    subwidget.RectTransform.SetHeight(iconSize + (iconPadding * 2f));

                    subwidget.UpdateSubwidgetIconSize(activityBarIconRectTransform);
                }

                for (var subwidgetIndex = 0;
                     subwidgetIndex < widget.BottomActivityBarSubwidgets.Count;
                     subwidgetIndex++)
                {
                    var subwidget = widget.BottomActivityBarSubwidgets[subwidgetIndex];

                    subwidget.ApplyMetadata();

                    subwidget.UpdateSubwidgetIconSize(activityBarIconRectTransform);
                }

                widget.topActivityBarLayoutGroup.RectTransform.SetSiblingIndex(0);
                widget.bottomActivityBarLayoutGroup.RectTransform.SetSiblingIndex(1);
            }
        }
    }
}
