using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Constants;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Components;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Models;
using Appalachia.UI.ControlModel.Components.Extensions;
using Appalachia.UI.Functionality.Images.Controls.Default;
using Appalachia.UI.Functionality.Images.Groups.Rounded;
using Appalachia.UI.Functionality.Layout.Groups.Vertical;
using Appalachia.UI.Styling.Fonts;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo.Widgets
{
    public sealed class DeveloperInfoOverlayWidgetMetadata : DeveloperInterfaceMetadata_V01.WidgetMetadata<
        DeveloperInfoOverlayWidget, DeveloperInfoOverlayWidgetMetadata, DeveloperInfoFeature,
        DeveloperInfoFeatureMetadata>
    {
        #region Fields and Autoproperties

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(10f, 100f)]
        public float insetX;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(10f, 100f)]
        public float insetY;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(300f, 500f)]
        public float width;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [PropertyRange(300f, 600f)]
        public float height;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [PropertyRange(0f, 1f)]
        [PropertyOrder(1000)]
        [SerializeField]
        public float alpha;

        [FoldoutGroup(APPASTR.GroupNames.Visual)]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public Color color;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public VerticalLayoutGroupComponentGroupConfig verticalLayoutGroup;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public ImageControlConfig headerImage;

        [OnValueChanged(nameof(OnChanged))]
        [HideIf(nameof(HideFontStyleField))]
        public FontStyleTypes labelFontStyle;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public List<DeveloperInfoType> types;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        public DeveloperInfoTextMeshControlConfig.List textMeshes;

        #endregion

        public override float GetCanvasGroupVisibleAlpha()
        {
            using (_PRF_GetCanvasGroupVisibleAlpha.Auto())
            {
                return alpha;
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(alpha), () => alpha = .5f);
                initializer.Do(this, nameof(color), color == Color.clear, () => color = Colors.FromHexCode("000000"));

                initializer.Do(this, nameof(width),  width == 0,  () => width = 400f);
                initializer.Do(this, nameof(height), height == 0, () => height = 600f);

                initializer.Do(
                    this,
                    nameof(types),
                    (types == null) || (types.Count == 0),
                    () =>
                    {
                        types = new List<DeveloperInfoType>
                        {
                            DeveloperInfoType.ActiveSceneNameAndCount,
                            DeveloperInfoType.ApplicationBuildGuid,
                            DeveloperInfoType.ApplicationVersion,
                            DeveloperInfoType.ApplicationUnityVersion,
                            DeveloperInfoType.ApplicationPlatform,
                            DeveloperInfoType.ApplicationSystemLanguage,
                            DeveloperInfoType.ScreenResolution,
                            DeveloperInfoType.WindowSize,
                            DeveloperInfoType.SystemMemory,
                            DeveloperInfoType.ProcessorType,
                            DeveloperInfoType.GraphicsMemorySize,
                            DeveloperInfoType.GraphicsDeviceName,
                            DeveloperInfoType.GraphicsDeviceVersion,
                            DeveloperInfoType.OperatingSystem
                        };
                    }
                );

                textMeshes ??= new();

                VerticalLayoutGroupComponentGroupConfig.Refresh(ref verticalLayoutGroup, this);
                ImageControlConfig.Refresh(ref headerImage, this);
            }
        }

        /// <inheritdoc />
        protected override void OnApply(DeveloperInfoOverlayWidget widget)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(widget);

                rectTransform.Value.BeginModifications()
                             .AnchorTopLeft()
                             .PivotTopLeft()
                             .SetPosition(insetX, 0f, insetY, 0f)
                             .ResetRotationAndScale()
                             .SetSize(width, height)
                             .ApplyModifications();

                verticalLayoutGroup.Apply(widget.verticalLayoutGroup);
                headerImage.Apply(widget.headerImage);

                roundedBackground.IsElected = true;
                background.IsElected = false;

                var firstBackground = roundedBackground.Value.ConfigList.FirstOrDefault();

                if (firstBackground == null)
                {
                    firstBackground = RoundedImageComponentGroupConfig.CreateWithOwner(this);
                    roundedBackground.Value.ConfigList.Add(firstBackground);
                }

                firstBackground.roundedImage.color.OverrideValue(color);

                for (var i = types.Count; i < widget.textMeshes.Count; i++)
                {
                    widget.textMeshes[i].DestroySafely();
                }

                while (textMeshes.Count < widget.textMeshes.Count)
                {
                    DeveloperInfoTextMeshControlConfig newTextMesh = null;
                    DeveloperInfoTextMeshControlConfig.Refresh(ref newTextMesh, this);

                    textMeshes.Add(newTextMesh);
                }

                for (var i = widget.textMeshes.Count; i < types.Count; i++)
                {
                    var type = types[i];

                    DeveloperInfoTextMeshControl textMesh = null;

                    DeveloperInfoTextMeshControl.Refresh(
                        ref textMesh,
                        widget.verticalLayoutGroup.gameObject,
                        type.ToString()
                    );

                    widget.textMeshes.Add(textMesh);
                }

                for (var i = 0; i < widget.textMeshes.Count; i++)
                {
                    var type = types[i];
                    var control = widget.textMeshes[i];
                    var config = textMeshes[i];

                    DeveloperInfoTextMeshControl.Refresh(
                        ref control,
                        widget.verticalLayoutGroup.gameObject,
                        type.ToString()
                    );

                    config.textMeshLabel.IsElected = true;
                    config.labelFont = labelFontStyle;
                    config.font = fontStyle;

                    var labelFont = StyleLookup.GetFont(labelFontStyle);
                    var valueFont = StyleLookup.GetFont(fontStyle);

                    labelFont.HorizontalAlignment = HorizontalAlignmentOptions.Right;

                    valueFont.EnableWordWrapping = true;
                    valueFont.OverflowMode = TextOverflowModes.Ellipsis;
                    valueFont.HorizontalAlignment = HorizontalAlignmentOptions.Left;

                    config.initialCalculation = type;

                    config.Apply(control);

                    var maxPreferredHeight = Mathf.Max(
                        control.textMeshValue.textMeshProUGUI.preferredHeight,
                        control.textMeshLabel.textMeshProUGUI.preferredHeight
                    );

                    control.layoutElement.minHeight = maxPreferredHeight;
                    control.layoutElement.preferredHeight = maxPreferredHeight;
                }

                canvas.Value.Canvas.CanvasGroup.Value.alpha.Overriding = false;
                widget.canvas.Canvas.CanvasGroup.alpha = alpha;
            }
        }

        /// <inheritdoc />
        protected override void SubscribeResponsiveComponents(DeveloperInfoOverlayWidget target)
        {
            using (_PRF_SubscribeResponsiveComponents.Auto())
            {
                base.SubscribeResponsiveComponents(target);

                headerImage.SubscribeToChanges(OnChanged);
                verticalLayoutGroup.SubscribeToChanges(OnChanged);
            }
        }
        /// <inheritdoc />
        protected override void SuspendResponsiveComponents(DeveloperInfoOverlayWidget target)
        {
            using (_PRF_SuspendResponsiveComponents.Auto())
            {
                base.SuspendResponsiveComponents(target);

                headerImage.SuspendChanges();
                verticalLayoutGroup.SuspendChanges();
            }
        }
        /// <inheritdoc />
        protected override void UnsuspendResponsiveComponents(DeveloperInfoOverlayWidget target)
        {
            using (_PRF_UnsuspendResponsiveComponents.Auto())
            {
                base.UnsuspendResponsiveComponents(target);

                headerImage.UnsuspendChanges();
                verticalLayoutGroup.UnsuspendChanges();
            }
        }
    }
}
