using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Styling;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Components.Subsets;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets
{
    public sealed class StatusBarWidgetMetadata : DeveloperInterfaceMetadata_V01.
        WidgetWithSingletonSubwidgetsMetadata<IStatusBarSubwidget, IStatusBarSubwidgetMetadata,
            StatusBarWidget, StatusBarWidgetMetadata, StatusBarFeature, StatusBarFeatureMetadata>
    {
        #region Constants and Static Readonly

        public static readonly string LeftLayoutGroupSubwidgetsParentName = "Left " + StatusBarWidget.SubwidgetParentName;

        public static readonly string RightLayoutGroupSubwidgetsParentName = "Right " + StatusBarWidget.SubwidgetParentName;

        #endregion

        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Size)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.015f, 0.045f)]
        public float height;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [ShowIf(nameof(showAll))]
        public HorizontalLayoutGroupSubsetData leftLayoutGroup;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [ShowIf(nameof(showAll))]
        public HorizontalLayoutGroupSubsetData rightLayoutGroup;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [ShowIf(nameof(showAll))]
        public RectTransformData statusBarIconRectTransform;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [ShowIf(nameof(showAll))]
        public DevTooltipStyleOverride devTooltipStyle;

        [BoxGroup(APPASTR.GroupNames.Size)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(8, 100)]
        public int statusBarPaddingLeft;

        [BoxGroup(APPASTR.GroupNames.Size)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(0, 100)]
        public int statusBarSpacingLeft;

        [BoxGroup(APPASTR.GroupNames.Size)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(8, 100)]
        public int statusBarPaddingRight;

        [BoxGroup(APPASTR.GroupNames.Size)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(0, 100)]
        public int statusBarSpacingRight;

        [BoxGroup(APPASTR.GroupNames.Content)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(8, 64)]
        public int iconSize;

        [BoxGroup(APPASTR.GroupNames.Content)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(0, 100)]
        public int iconSpacing;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public Sprite defaultStatusBarIcon;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            initializer.Do(this, nameof(height), () => height = 0.03f);

            initializer.Do(
                this,
                nameof(statusBarPaddingLeft),
                statusBarPaddingLeft == 0,
                () => statusBarPaddingLeft = 50
            );
            initializer.Do(
                this,
                nameof(statusBarPaddingRight),
                statusBarPaddingRight == 0,
                () => statusBarPaddingRight = 50
            );
            initializer.Do(
                this,
                nameof(statusBarSpacingLeft),
                statusBarSpacingLeft == 0,
                () => statusBarSpacingLeft = 10
            );
            initializer.Do(
                this,
                nameof(statusBarSpacingRight),
                statusBarSpacingRight == 0,
                () => statusBarSpacingRight = 10
            );
            initializer.Do(this, nameof(iconSize), iconSize == 0, () => iconSize = 24);
            initializer.Do(this, nameof(iconSpacing), iconSpacing == 0, () => iconSpacing = 8);

            initializer.Do(
                this,
                nameof(leftLayoutGroup),
                leftLayoutGroup == null,
                () => leftLayoutGroup = new HorizontalLayoutGroupSubsetData(this)
            );

            initializer.Do(
                this,
                nameof(rightLayoutGroup),
                rightLayoutGroup == null,
                () => rightLayoutGroup = new HorizontalLayoutGroupSubsetData(this)
            );

            initializer.Do(
                this,
                nameof(statusBarIconRectTransform),
                statusBarIconRectTransform == null,
                () => statusBarIconRectTransform = new RectTransformData(this)
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

        protected override void SubscribeResponsiveComponents(StatusBarWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                leftLayoutGroup.Changed.Event += OnChanged;
                rightLayoutGroup.Changed.Event += OnChanged;
                rectTransform.Changed.Event += OnChanged;
            }
        }

        /// <inheritdoc />
        protected override void UpdateFunctionalityInternal(StatusBarWidget widget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                base.UpdateFunctionalityInternal(widget);

                if (rightLayoutGroup != null)
                {
                    rightLayoutGroup.HorizontalLayoutGroup.childControlWidth.OverrideValue(false);
                    rightLayoutGroup.HorizontalLayoutGroup.childControlHeight.OverrideValue(true);
                    
                    rightLayoutGroup.HorizontalLayoutGroup.childForceExpandWidth.OverrideValue(false);
                    rightLayoutGroup.HorizontalLayoutGroup.childForceExpandHeight.OverrideValue(true);
                    
                    rightLayoutGroup.HorizontalLayoutGroup.childScaleWidth.OverrideValue(false);
                    rightLayoutGroup.HorizontalLayoutGroup.childScaleHeight.OverrideValue(false);

                    rightLayoutGroup.HorizontalLayoutGroup.padding.Overriding = true;
                    rightLayoutGroup.HorizontalLayoutGroup.padding.Value.right = statusBarPaddingRight;
                    rightLayoutGroup.HorizontalLayoutGroup.spacing.Value = statusBarSpacingRight;
                }

                if (leftLayoutGroup != null)
                {
                    leftLayoutGroup.HorizontalLayoutGroup.childControlWidth.OverrideValue(false);
                    leftLayoutGroup.HorizontalLayoutGroup.childControlHeight.OverrideValue(true);
                    
                    leftLayoutGroup.HorizontalLayoutGroup.childForceExpandWidth.OverrideValue(false);
                    leftLayoutGroup.HorizontalLayoutGroup.childForceExpandHeight.OverrideValue(true);
                    
                    leftLayoutGroup.HorizontalLayoutGroup.childScaleWidth.OverrideValue(false);
                    leftLayoutGroup.HorizontalLayoutGroup.childScaleHeight.OverrideValue(false);
                    
                    leftLayoutGroup.HorizontalLayoutGroup.padding.Overriding = true;
                    leftLayoutGroup.HorizontalLayoutGroup.padding.Value.left = statusBarPaddingLeft;
                    leftLayoutGroup.HorizontalLayoutGroup.spacing.Value = statusBarSpacingLeft;
                }
                
                HorizontalLayoutGroupSubsetData.RefreshAndUpdate(
                    ref rightLayoutGroup,
                    this,
                    ref widget.rightStatusBarLayoutGroup,
                    widget.SubwidgetParent,
                    RightLayoutGroupSubwidgetsParentName
                );
                
                HorizontalLayoutGroupSubsetData.RefreshAndUpdate(
                    ref leftLayoutGroup,
                    this,
                    ref widget.leftStatusBarLayoutGroup,
                    widget.SubwidgetParent,
                    LeftLayoutGroupSubwidgetsParentName
                );

                widget.ValidateSubwidgets();

                widget.SortSubwidgetsByPriority();

                statusBarIconRectTransform.SetSize(iconSize, iconSize);
                
                for (var subwidgetIndex = 0;
                     subwidgetIndex < widget.LeftStatusBarSubwidgets.Count;
                     subwidgetIndex++)
                {
                    var subwidget = widget.LeftStatusBarSubwidgets[subwidgetIndex];

                    var swLayoutGroup = subwidget.Metadata.Button.LayoutGroup;
                    
                    swLayoutGroup.IsElected = true;
                    swLayoutGroup.Value.Enabled = true;

                    var swHLayoutGroup = swLayoutGroup.Value.HorizontalLayoutGroup;
                    
                    swHLayoutGroup.spacing.OverrideValue(iconSpacing);
                    swHLayoutGroup.childControlWidth.OverrideValue(false);
                    swHLayoutGroup.childControlHeight.OverrideValue(false);
                    swHLayoutGroup.childForceExpandWidth.OverrideValue(false);
                    swHLayoutGroup.childForceExpandHeight.OverrideValue(true);
                    swHLayoutGroup.childScaleWidth.OverrideValue(false);
                    swHLayoutGroup.childScaleHeight.OverrideValue(false);
                    swHLayoutGroup.childAlignment.OverrideValue(TextAnchor.MiddleCenter);
                    
                    subwidget.UpdateSubwidget();

                    subwidget.UpdateSubwidgetFont(fontStyle);
                    subwidget.UpdateSubwidgetIconSize(statusBarIconRectTransform);
                }

                for (var subwidgetIndex = 0;
                     subwidgetIndex < widget.RightStatusBarSubwidgets.Count;
                     subwidgetIndex++)
                {
                    var subwidget = widget.RightStatusBarSubwidgets[subwidgetIndex];

                    subwidget.UpdateSubwidget();

                    subwidget.UpdateSubwidgetFont(fontStyle);
                    subwidget.UpdateSubwidgetIconSize(statusBarIconRectTransform);
                }

                widget.leftStatusBarLayoutGroup.RectTransform.SetSiblingIndex(0);
                widget.rightStatusBarLayoutGroup.RectTransform.SetSiblingIndex(1);
            }
        }
    }
}
