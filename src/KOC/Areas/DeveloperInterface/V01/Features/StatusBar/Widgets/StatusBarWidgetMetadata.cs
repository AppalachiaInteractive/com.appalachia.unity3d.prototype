using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts;
using Appalachia.UI.ControlModel.Components;
using Appalachia.UI.Functionality.Layout.Groups.Horizontal;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Widgets
{
    public sealed class StatusBarWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetWithSingletonSubwidgetsMetadata<
        IStatusBarSubwidget, IStatusBarSubwidgetMetadata, StatusBarWidget, StatusBarWidgetMetadata, StatusBarFeature,
        StatusBarFeatureMetadata>
    {
        #region Constants and Static Readonly

        public static readonly string LeftLayoutGroupSubwidgetsParentName =
            "Left " + StatusBarWidget.SubwidgetParentName;

        public static readonly string RightLayoutGroupSubwidgetsParentName =
            "Right " + StatusBarWidget.SubwidgetParentName;

        #endregion

        #region Fields and Autoproperties

        [BoxGroup(APPASTR.GroupNames.Size)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0.015f, 0.045f)]
        public float height;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [HideIf(nameof(HideAllFields))]
        public HorizontalLayoutGroupComponentGroupConfig leftLayoutGroup;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [HideIf(nameof(HideAllFields))]
        public HorizontalLayoutGroupComponentGroupConfig rightLayoutGroup;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [HideIf(nameof(HideAllFields))]
        public RectTransformConfig statusBarIconRectTransform;

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
            initializer.Do(this, nameof(iconSize),    iconSize == 0,    () => iconSize = 24);
            initializer.Do(this, nameof(iconSpacing), iconSpacing == 0, () => iconSpacing = 8);

            initializer.Do(
                this,
                nameof(leftLayoutGroup),
                leftLayoutGroup == null,
                () => leftLayoutGroup = new HorizontalLayoutGroupComponentGroupConfig(this)
            );

            initializer.Do(
                this,
                nameof(rightLayoutGroup),
                rightLayoutGroup == null,
                () => rightLayoutGroup = new HorizontalLayoutGroupComponentGroupConfig(this)
            );

            initializer.Do(
                this,
                nameof(statusBarIconRectTransform),
                statusBarIconRectTransform == null,
                () => statusBarIconRectTransform = new RectTransformConfig(this)
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

                HorizontalLayoutGroupComponentGroupConfig.RefreshAndApply(
                    ref rightLayoutGroup,
                    this,
                    ref widget.rightStatusBarLayoutGroup,
                    widget.SubwidgetParent,
                    RightLayoutGroupSubwidgetsParentName
                );

                HorizontalLayoutGroupComponentGroupConfig.RefreshAndApply(
                    ref leftLayoutGroup,
                    this,
                    ref widget.leftStatusBarLayoutGroup,
                    widget.SubwidgetParent,
                    LeftLayoutGroupSubwidgetsParentName
                );

                widget.ValidateSubwidgets();

                widget.SortSubwidgetsByPriority();

                statusBarIconRectTransform.SetSize(iconSize, iconSize);

                for (var subwidgetIndex = 0; subwidgetIndex < widget.LeftStatusBarSubwidgets.Count; subwidgetIndex++)
                {
                    var subwidget = widget.LeftStatusBarSubwidgets[subwidgetIndex];
                    var swMetadata = subwidget.Metadata;

                    /*var swLayoutGroup = swMetadata.Button.LayoutGroup;

                    swLayoutGroup.IsElected = true;
                    swLayoutGroup.BindValueEnabledState();

                    var swHLayoutGroup = swLayoutGroup.Value.HorizontalLayoutGroup;

                    swHLayoutGroup.spacing.OverrideValue(iconSpacing);
                    swHLayoutGroup.childControlWidth.OverrideValue(false);
                    swHLayoutGroup.childControlHeight.OverrideValue(false);
                    swHLayoutGroup.childForceExpandWidth.OverrideValue(false);
                    swHLayoutGroup.childForceExpandHeight.OverrideValue(true);
                    swHLayoutGroup.childScaleWidth.OverrideValue(false);
                    swHLayoutGroup.childScaleHeight.OverrideValue(false);
                    swHLayoutGroup.childAlignment.OverrideValue(TextAnchor.MiddleCenter);
                    */
                    
                    swMetadata.UpdateSubwidgetFont(fontStyle);
                    swMetadata.UpdateSubwidgetIconSize(statusBarIconRectTransform);
                    subwidget.ApplyMetadata();
                }

                for (var subwidgetIndex = 0; subwidgetIndex < widget.RightStatusBarSubwidgets.Count; subwidgetIndex++)
                {
                    var subwidget = widget.RightStatusBarSubwidgets[subwidgetIndex];

                    subwidget.Metadata.UpdateSubwidgetFont(fontStyle);
                    subwidget.Metadata.UpdateSubwidgetIconSize(statusBarIconRectTransform);
                    subwidget.ApplyMetadata();
                }

                widget.leftStatusBarLayoutGroup.RectTransform.SetSiblingIndex(0);
                widget.rightStatusBarLayoutGroup.RectTransform.SetSiblingIndex(1);
            }
        }
    }
}
