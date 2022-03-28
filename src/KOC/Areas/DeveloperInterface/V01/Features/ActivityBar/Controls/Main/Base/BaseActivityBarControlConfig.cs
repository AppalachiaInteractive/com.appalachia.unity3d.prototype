using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Controls.Main.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Subwidgets.Contracts;
using Appalachia.UI.ControlModel.Components;
using Appalachia.UI.ControlModel.Components.Extensions;
using Appalachia.UI.ControlModel.Controls.Default;
using Appalachia.UI.Core.Extensions;
using Appalachia.UI.Functionality.Buttons.Groups.SelectionIndicator;
using Appalachia.UI.Functionality.Layout.Groups.Vertical;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar.Controls.Main.Base
{
    /// <summary>
    ///     Defines the metadata necessary for configuring a
    ///     <see cref="BaseActivityBarControl{TControl,TConfig}" />.
    /// </summary>
    /// <typeparam name="TControl">The control.</typeparam>
    /// <typeparam name="TConfig">Configuration for the control.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseActivityBarControlConfig<TControl, TConfig> : AppaUIControlConfig<TControl, TConfig>,
                                                                            IActivityBarControlConfig
        where TControl : BaseActivityBarControl<TControl, TConfig>, new()
        where TConfig : BaseActivityBarControlConfig<TControl, TConfig>, new()
    {
        protected BaseActivityBarControlConfig()
        {
        }

        protected BaseActivityBarControlConfig(Object owner) : base(owner)
        {
        }

        #region Fields and Autoproperties

        [HideIf("@!ShowAllFields && (HideTopActivityBar || HideAllFields)")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public VerticalLayoutGroupComponentGroupConfig topActivityBar;

        [HideIf("@!ShowAllFields && (HideBottomActivityBar || HideAllFields)")]
        [SerializeField, OnValueChanged(nameof(OnChanged))]
        public VerticalLayoutGroupComponentGroupConfig bottomActivityBar;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [HideInInspector]
        [HideIf("@!ShowAllFields")]
        public RectTransformConfig iconRectTransform;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(8, 100)]
        public int activityBarPaddingTop;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(0, 100)]
        public int activityBarSpacingTop;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(8, 100)]
        public int activityBarPaddingBottom;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(0, 100)]
        public int activityBarSpacingBottom;

        [FoldoutGroup(APPASTR.GroupNames.Content)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(8, 64)]
        public int iconSize;

        [FoldoutGroup(APPASTR.GroupNames.Content)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(0, 100)]
        public int iconPadding;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public Sprite defaultActivityBarIcon;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public SelectionIndicatorComponentGroupConfig selectionIndicator;

        #endregion

        protected virtual bool HideBottomActivityBar => false;

        protected virtual bool HideTopActivityBar => false;

        public void ApplyToSubwidget(IActivityBarSubwidget subwidget)
        {
            using (_PRF_ApplyToSubwidget.Auto())
            {
                var buttonSelectionIndicatorOption = subwidget.Metadata.Button.SelectionIndicator;
                buttonSelectionIndicatorOption.IsElected = true;

                var buttonSelectionIndicator = buttonSelectionIndicatorOption.value;

                selectionIndicator.CopyTo(buttonSelectionIndicator);

                subwidget.ApplyMetadata();

                var iconPadding2x = iconPadding * 2f;
                subwidget.RectTransform.AnchorTopCenter()
                         .PivotTopCenter()
                         .ResetRotationAndScale()
                         .SetWidth(iconSize + iconPadding2x)
                         .SetHeight(iconSize + iconPadding2x);

                subwidget.Metadata.UpdateSubwidgetIconSize(iconRectTransform);
                subwidget.Control.Tooltip.tooltipContent = subwidget.GetTooltipContent();
            }
        }

        protected override void BeforeApplying(TControl control)
        {
            using (_PRF_BeforeApplying.Auto())
            {
                base.BeforeApplying(control);

                iconRectTransform.BeginModifications().SetSize(iconSize, iconSize).ApplyModifications();

                if (bottomActivityBar != null)
                {
                    bottomActivityBar.VerticalLayoutGroup.padding.Overriding = true;
                    bottomActivityBar.VerticalLayoutGroup.padding.Value.bottom = activityBarPaddingBottom;
                    bottomActivityBar.VerticalLayoutGroup.spacing.Value = activityBarSpacingBottom;

                    bottomActivityBar.VerticalLayoutGroup.padding.Value.left = iconPadding;
                    bottomActivityBar.VerticalLayoutGroup.padding.Value.right = iconPadding;
                }

                if (topActivityBar != null)
                {
                    topActivityBar.VerticalLayoutGroup.padding.Overriding = true;
                    topActivityBar.VerticalLayoutGroup.padding.Value.top = activityBarPaddingTop;
                    topActivityBar.VerticalLayoutGroup.spacing.Value = activityBarSpacingTop;
                    topActivityBar.VerticalLayoutGroup.padding.Value.left = iconPadding;
                    topActivityBar.VerticalLayoutGroup.padding.Value.right = iconPadding;
                }
            }
        }

        /// <inheritdoc />
        protected override void OnApply(TControl control)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(control);

                topActivityBar.Apply(control.topActivityBar);
                bottomActivityBar.Apply(control.bottomActivityBar);
            }
        }

        protected override void OnInitializeFields(Initializer initializer)
        {
            using (_PRF_OnInitializeFields.Auto())
            {
                base.OnInitializeFields(initializer);

                VerticalLayoutGroupComponentGroupConfig.Refresh(ref topActivityBar,    Owner);
                VerticalLayoutGroupComponentGroupConfig.Refresh(ref bottomActivityBar, Owner);
                RectTransformConfig.Refresh(ref iconRectTransform, Owner);
                SelectionIndicatorComponentGroupConfig.Refresh(ref selectionIndicator, Owner);

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
                initializer.Do(this, nameof(iconSize),    iconSize == 0,    () => iconSize = 24);
                initializer.Do(this, nameof(iconPadding), iconPadding == 0, () => iconPadding = 2);
            }
        }

        protected override void OnRefresh(Object owner)
        {
            base.OnRefresh(owner);

            VerticalLayoutGroupComponentGroupConfig.Refresh(ref topActivityBar,    owner);
            VerticalLayoutGroupComponentGroupConfig.Refresh(ref bottomActivityBar, owner);
            RectTransformConfig.Refresh(ref iconRectTransform, Owner);
        }

        protected override void SubscribeResponsiveConfigs()
        {
            using (_PRF_SubscribeResponsiveConfigs.Auto())
            {
                base.SubscribeResponsiveConfigs();

                topActivityBar.SubscribeToChanges(OnChanged);
                bottomActivityBar.SubscribeToChanges(OnChanged);
                iconRectTransform.SubscribeToChanges(OnChanged);
            }
        }

        protected override void SuspendResponsiveConfigs()
        {
            using (_PRF_SuspendResponsiveConfigs.Auto())
            {
                base.SuspendResponsiveConfigs();

                topActivityBar.SuspendChanges();
                bottomActivityBar.SuspendChanges();
                iconRectTransform.SuspendChanges();
            }
        }
        
        protected override void UnsuspendResponsiveConfigs()
        {
            using (_PRF_UnsuspendResponsiveConfigs.Auto())
            {
                base.UnsuspendResponsiveConfigs();

                topActivityBar.UnsuspendChanges();
                bottomActivityBar.UnsuspendChanges();
                iconRectTransform.UnsuspendChanges();
            }
        }
        #region IActivityBarControlConfig Members

        public VerticalLayoutGroupComponentGroupConfig TopActivityBar
        {
            get => topActivityBar;
            protected set => topActivityBar = value;
        }

        public VerticalLayoutGroupComponentGroupConfig BottomActivityBar
        {
            get => bottomActivityBar;
            protected set => bottomActivityBar = value;
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
