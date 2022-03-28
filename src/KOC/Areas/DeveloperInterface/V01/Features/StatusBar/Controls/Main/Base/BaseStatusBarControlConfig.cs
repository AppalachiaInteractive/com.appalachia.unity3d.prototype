using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Controls.Main.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Subwidgets.Contracts;
using Appalachia.UI.ControlModel.Components;
using Appalachia.UI.ControlModel.Components.Extensions;
using Appalachia.UI.ControlModel.Controls.Default;
using Appalachia.UI.Functionality.Layout.Groups.Horizontal;
using Appalachia.UI.Functionality.Tooltips.Styling;
using Appalachia.UI.Styling.Fonts;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar.Controls.Main.Base
{
    /// <summary>
    ///     Defines the metadata necessary for configuring a
    ///     <see cref="BaseStatusBarControl{TControl,TConfig}" />.
    /// </summary>
    /// <typeparam name="TControl">The control.</typeparam>
    /// <typeparam name="TConfig">Configuration for the control.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseStatusBarControlConfig<TControl, TConfig> : AppaUIControlConfig<TControl, TConfig>,
                                                                          IStatusBarControlConfig
        where TControl : BaseStatusBarControl<TControl, TConfig>, new()
        where TConfig : BaseStatusBarControlConfig<TControl, TConfig>, new()
    {
        protected BaseStatusBarControlConfig()
        {
        }

        protected BaseStatusBarControlConfig(Object owner) : base(owner)
        {
        }

        #region Fields and Autoproperties

        [HideIf("@!ShowAllFields && (HideLeftStatusBar || HideAllFields)")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public HorizontalLayoutGroupComponentGroupConfig leftStatusBar;

        [HideIf("@!ShowAllFields && (HideRightStatusBar || HideAllFields)")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public HorizontalLayoutGroupComponentGroupConfig rightStatusBar;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [HideInInspector]
        [HideIf("@!ShowAllFields")]
        public RectTransformConfig iconRectTransform;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(8, 100)]
        public int statusBarPaddingLeft;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(0, 100)]
        public int statusBarSpacingLeft;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(8, 100)]
        public int statusBarPaddingRight;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(0, 100)]
        public int statusBarSpacingRight;

        [FoldoutGroup(APPASTR.GroupNames.Content)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(8, 64)]
        public int iconSize;

        [FoldoutGroup(APPASTR.GroupNames.Content)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(0, 100)]
        public int iconSpacing;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public Sprite defaultStatusBarIcon;

        [OnValueChanged(nameof(OnChanged))]
        public FontStyleTypes fontStyle;

        [FoldoutGroup(APPASTR.Common)]
        [OnValueChanged(nameof(OnChanged))]
        public TooltipStyleTypes tooltipStyle;

        #endregion

        protected virtual bool HideLeftStatusBar => false;

        protected virtual bool HideRightStatusBar => false;

        public void ApplyToSubwidget(IStatusBarSubwidget subwidget)
        {
            using (_PRF_ApplyToSubwidget.Auto())
            {
                var swMetadata = subwidget.Metadata;

                swMetadata.UpdateFontStyle(fontStyle);
                swMetadata.UpdateTooltipStyle(tooltipStyle);
                swMetadata.UpdateSubwidgetIconSize(iconRectTransform);
                subwidget.Control.Tooltip.tooltipContent = subwidget.GetTooltipContent();
                subwidget.ApplyMetadata();
            }
        }

        /// <inheritdoc />
        protected override void OnApply(TControl control)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(control);

                iconRectTransform.BeginModifications().SetSize(iconSize, iconSize).ApplyModifications();

                if (rightStatusBar != null)
                {
                    rightStatusBar.HorizontalLayoutGroup.childControlWidth.OverrideValue(false);
                    rightStatusBar.HorizontalLayoutGroup.childControlHeight.OverrideValue(true);

                    rightStatusBar.HorizontalLayoutGroup.childForceExpandWidth.OverrideValue(false);
                    rightStatusBar.HorizontalLayoutGroup.childForceExpandHeight.OverrideValue(true);

                    rightStatusBar.HorizontalLayoutGroup.childScaleWidth.OverrideValue(false);
                    rightStatusBar.HorizontalLayoutGroup.childScaleHeight.OverrideValue(false);

                    rightStatusBar.HorizontalLayoutGroup.padding.overriding = true;
                    rightStatusBar.HorizontalLayoutGroup.padding.Value.right = statusBarPaddingRight;
                    rightStatusBar.HorizontalLayoutGroup.spacing.value = statusBarSpacingRight;
                }

                if (leftStatusBar != null)
                {
                    leftStatusBar.HorizontalLayoutGroup.childControlWidth.OverrideValue(false);
                    leftStatusBar.HorizontalLayoutGroup.childControlHeight.OverrideValue(true);

                    leftStatusBar.HorizontalLayoutGroup.childForceExpandWidth.OverrideValue(false);
                    leftStatusBar.HorizontalLayoutGroup.childForceExpandHeight.OverrideValue(true);

                    leftStatusBar.HorizontalLayoutGroup.childScaleWidth.OverrideValue(false);
                    leftStatusBar.HorizontalLayoutGroup.childScaleHeight.OverrideValue(false);

                    leftStatusBar.HorizontalLayoutGroup.padding.Overriding = true;
                    leftStatusBar.HorizontalLayoutGroup.padding.Value.left = statusBarPaddingLeft;
                    leftStatusBar.HorizontalLayoutGroup.spacing.Value = statusBarSpacingLeft;
                }

                leftStatusBar.Apply(control.leftStatusBar);
                rightStatusBar.Apply(control.rightStatusBar);
            }
        }

        protected override void OnInitializeFields(Initializer initializer)
        {
            using (_PRF_OnInitializeFields.Auto())
            {
                base.OnInitializeFields(initializer);

                HorizontalLayoutGroupComponentGroupConfig.Refresh(ref leftStatusBar,  Owner);
                HorizontalLayoutGroupComponentGroupConfig.Refresh(ref rightStatusBar, Owner);
                RectTransformConfig.Refresh(ref iconRectTransform, Owner);

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
            }
        }

        protected override void OnRefresh(Object owner)
        {
            base.OnRefresh(owner);

            HorizontalLayoutGroupComponentGroupConfig.Refresh(ref leftStatusBar,  owner);
            HorizontalLayoutGroupComponentGroupConfig.Refresh(ref rightStatusBar, owner);
            RectTransformConfig.Refresh(ref iconRectTransform, Owner);
        }

        protected override void SubscribeResponsiveConfigs()
        {
            using (_PRF_SubscribeResponsiveConfigs.Auto())
            {
                base.SubscribeResponsiveConfigs();

                leftStatusBar.SubscribeToChanges(OnChanged);
                rightStatusBar.SubscribeToChanges(OnChanged);
                iconRectTransform.SubscribeToChanges(OnChanged);
            }
        }
        
        
        protected override void SuspendResponsiveConfigs()
        {
            using (_PRF_SuspendResponsiveConfigs.Auto())
            {
                base.SuspendResponsiveConfigs();

                leftStatusBar.SuspendChanges();
                rightStatusBar.SuspendChanges();
                iconRectTransform.SuspendChanges();
            }
        }
        
        
        protected override void UnsuspendResponsiveConfigs()
        {
            using (_PRF_UnsuspendResponsiveConfigs.Auto())
            {
                base.UnsuspendResponsiveConfigs();

                leftStatusBar.UnsuspendChanges();
                rightStatusBar.UnsuspendChanges();
                iconRectTransform.UnsuspendChanges();
            }
        }

        #region IStatusBarControlConfig Members

        public HorizontalLayoutGroupComponentGroupConfig LeftStatusBar
        {
            get => leftStatusBar;
            protected set => leftStatusBar = value;
        }

        public HorizontalLayoutGroupComponentGroupConfig RightStatusBar
        {
            get => rightStatusBar;
            protected set => rightStatusBar = value;
        }

        public RectTransformConfig IconRectTransform
        {
            get => iconRectTransform;
            protected set => iconRectTransform = value;
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_ApplyToSubwidget =
            new ProfilerMarker(_PRF_PFX + nameof(ApplyToSubwidget));

        #endregion
    }
}
