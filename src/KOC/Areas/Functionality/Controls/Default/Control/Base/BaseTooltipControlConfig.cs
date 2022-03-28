using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Control.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Styling;
using Appalachia.UI.ControlModel.ComponentGroups.Default;
using Appalachia.UI.ControlModel.ComponentGroups.Fadeable;
using Appalachia.UI.ControlModel.Controls.Default;
using Appalachia.UI.Core.Extensions;
using Appalachia.UI.Core.Layout;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.UI.Functionality.Images.Groups.Outline;
using Appalachia.UI.Functionality.Text.Groups;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Control.Base
{
    /// <summary>
    ///     Defines the metadata necessary for configuring a
    ///     <see cref="BaseTooltipControl{TControl,TConfig}" />.
    /// </summary>
    /// <typeparam name="TControl">The control.</typeparam>
    /// <typeparam name="TConfig">Configuration for the control.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseTooltipControlConfig<TControl, TConfig> : AppaUIControlConfig<TControl, TConfig>,
                                                                        ITooltipControlConfig
        where TControl : BaseTooltipControl<TControl, TConfig>, new()
        where TConfig : BaseTooltipControlConfig<TControl, TConfig>, new()

    {
        #region Constants and Static Readonly

        protected const int ORDER_TOOLTIP = ORDER_RECT + 50;

        #endregion

        #region Fields and Autoproperties

        [FormerlySerializedAs("styleOverride")]
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [HideIf(nameof(HideAllFields))]
        public TooltipStyleOverride style;

        [FormerlySerializedAs("_background")]
        [PropertyOrder(ORDER_TOOLTIP + 10)]
        [SerializeField]
        public OutlinedImageComponentGroupConfig background;

        [PropertyOrder(ORDER_TOOLTIP + 19)]
        [SerializeField]
        public BasicUIComponentGroupConfig triangle;

        [FormerlySerializedAs("_triangleBackground")]
        [PropertyOrder(ORDER_TOOLTIP + 20)]
        [SerializeField]
        public ImageComponentGroupConfig triangleBackground;

        [FormerlySerializedAs("_triangleForeground")]
        [PropertyOrder(ORDER_TOOLTIP + 30)]
        [SerializeField]
        public ImageComponentGroupConfig triangleForeground;

        [FormerlySerializedAs("_text")]
        [PropertyOrder(ORDER_TOOLTIP + 40)]
        [SerializeField]
        public TextMeshProUGUIComponentGroupConfig text;

        #endregion

        protected override void BeforeApplying(TControl control)
        {
            using (_PRF_BeforeApplying.Auto())
            {
                base.BeforeApplying(control);

                var textComponent = control.text.TextMeshProUGUI;

                var text = control.tooltipContent;

                var requiredTextSize = text.IsNullOrEmpty()
                    ? new Vector2(0, 0)
                    : textComponent.GetPreferredValues(text);

                if (style == null)
                {
                    return;
                }

                FormatRectTransform(control, requiredTextSize);

                FormatTooltipBackground();

                FormatTooltipTriangle();

                FormatText();

                textComponent.text = text;
            }
        }

        /// <inheritdoc />
        protected override void OnApply(TControl control)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(control);

                background.Apply(control.background);
                triangle.Apply(control.triangle);
                triangleBackground.Apply(control.triangleBackground);
                triangleForeground.Apply(control.triangleForeground);
                text.Apply(control.text);

                RectTransform.DisableAllFields = false;
                background.RectTransform.DisableAllFields = false;
                triangleBackground.RectTransform.DisableAllFields = false;
                triangleForeground.RectTransform.DisableAllFields = false;
                text.RectTransform.DisableAllFields = false;

                RectTransform.HideAllFields = true;
                background.RectTransform.HideAllFields = true;
                triangleBackground.RectTransform.HideAllFields = true;
                triangleForeground.RectTransform.HideAllFields = true;
                text.RectTransform.HideAllFields = true;
            }
        }

        protected override void OnInitializeFields(Initializer initializer)
        {
            base.OnInitializeFields(initializer);
            OutlinedImageComponentGroupConfig.Refresh(ref background, Owner);
            BasicUIComponentGroupConfig.Refresh(ref triangle, Owner);
            ImageComponentGroupConfig.Refresh(ref triangleBackground, Owner);
            ImageComponentGroupConfig.Refresh(ref triangleForeground, Owner);
            TextMeshProUGUIComponentGroupConfig.Refresh(ref text, Owner);
        }

        protected override void OnRefresh(Object owner)
        {
            base.OnRefresh(owner);

            OutlinedImageComponentGroupConfig.Refresh(ref background, Owner);
            BasicUIComponentGroupConfig.Refresh(ref triangle, Owner);
            ImageComponentGroupConfig.Refresh(ref triangleBackground, Owner);
            ImageComponentGroupConfig.Refresh(ref triangleForeground, Owner);
            TextMeshProUGUIComponentGroupConfig.Refresh(ref text, Owner);
        }

        protected override void SubscribeResponsiveConfigs()
        {
            using (_PRF_SubscribeResponsiveConfigs.Auto())
            {
                base.SubscribeResponsiveConfigs();

                background.SubscribeToChanges(OnChanged);
                triangle.SubscribeToChanges(OnChanged);
                triangleBackground.SubscribeToChanges(OnChanged);
                triangleForeground.SubscribeToChanges(OnChanged);
                text.SubscribeToChanges(OnChanged);
            }
        }

        private void FormatRectTransform(TControl control, Vector2 requiredTextSize)
        {
            using (_PRF_FormatRectTransform.Auto())
            {
                var requiredPaddingSize = style.TextPadding * 2f * Vector2.one;

                var requiredSize = requiredPaddingSize + requiredTextSize;

                var target = control.Target;

                var targetRect = target.rectTransform.GetScreenSpaceRect();

                control.calculatedTargetCenter = targetRect.center;

                rectTransform.AnchorBottomLeft().ResetRotationAndScale();

                switch (style.Direction)
                {
                    case AppearanceDirection.Above:
                        control.calculatedTooltipReferencePoint = targetRect.GetTopCenter();
                        control.calculatedTooltipOffset = new Vector2(0f, style.DistanceFromTarget);
                        break;
                    case AppearanceDirection.Left:
                        control.calculatedTooltipReferencePoint = targetRect.GetMiddleLeft();
                        control.calculatedTooltipOffset = new Vector2(-style.DistanceFromTarget, 0f);
                        break;
                    case AppearanceDirection.Right:
                        control.calculatedTooltipReferencePoint = targetRect.GetMiddleRight();
                        control.calculatedTooltipOffset = new Vector2(style.DistanceFromTarget, 0f);
                        break;
                    case AppearanceDirection.Below:
                        control.calculatedTooltipReferencePoint = targetRect.GetBottomCenter();
                        control.calculatedTooltipOffset = new Vector2(0f, -style.DistanceFromTarget);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                control.calculatedTooltipFinalPosition =
                    control.calculatedTooltipReferencePoint + control.calculatedTooltipOffset;

                rectTransform.SetPositionXYAndSize(control.calculatedTooltipFinalPosition, requiredSize);

                switch (style.Direction)
                {
                    case AppearanceDirection.Above:
                        rectTransform.PivotBottomCenter();
                        break;
                    case AppearanceDirection.Left:
                        rectTransform.PivotMiddleRight();
                        break;
                    case AppearanceDirection.Right:
                        rectTransform.PivotMiddleLeft();
                        break;
                    case AppearanceDirection.Below:
                        rectTransform.PivotTopCenter();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(style.Direction), style.Direction, null);
                }
            }
        }

        private void FormatText()
        {
            using (_PRF_FormatText.Auto())
            {
                text.RectTransform.ResetAnchorsRotationAndScale().ResetSize().Inset(style.TextPadding);

                text.TextMeshProUGUI.fontStyle.OverflowMode = TextOverflowModes.Truncate;
                text.TextMeshProUGUI.fontStyle.HorizontalAlignment = HorizontalAlignmentOptions.Left;
                text.TextMeshProUGUI.fontStyle.VerticalAlignment = VerticalAlignmentOptions.Top;
            }
        }

        private void FormatTooltipBackground()
        {
            using (_PRF_FormatTooltipBackground.Auto())
            {
                background.RectTransform.FullScreen();

                background.Outline.effectColor.OverrideValue(style.OutlineColor);
                var v2Thickness = style.OutlineThickness * Vector2.one;
                background.Outline.effectDistance.OverrideValue(v2Thickness);
                background.Image.color.OverrideValue(style.BackgroundColor);
                background.Image.sprite.OverrideValue(null);
            }
        }

        private void FormatTooltipTriangle()
        {
            using (_PRF_FormatTooltipTriangle.Auto())
            {
                triangleForeground.ShouldEnableFunction = () => style.ShowTriangle;
                triangleBackground.ShouldEnableFunction = () => style.ShowTriangle;

                if (!style.ShowTriangle)
                {
                    return;
                }

                var size = Vector2.one * style.TriangleSize;

                var offset = style.Direction switch
                {
                    AppearanceDirection.Above => new Vector2(0f,                         .5f * style.TriangleSize),
                    AppearanceDirection.Left  => new Vector2(.5f * -style.TriangleSize, 0f),
                    AppearanceDirection.Right => new Vector2(.5f * style.TriangleSize,  0f),
                    AppearanceDirection.Below => new Vector2(0f,                         .5f * -style.TriangleSize),
                    _                         => throw new ArgumentOutOfRangeException()
                };

                triangle.rectTransform.ResetRotationAndScale();
                switch (style.Direction)
                {
                    case AppearanceDirection.Above:
                        triangle.rectTransform.AnchorBottomCenter().PivotTopCenter();
                        break;
                    case AppearanceDirection.Left:
                        triangle.rectTransform.AnchorMiddleRight().PivotMiddleLeft();
                        break;
                    case AppearanceDirection.Right:
                        triangle.rectTransform.AnchorMiddleLeft().PivotMiddleRight();
                        break;
                    case AppearanceDirection.Below:
                        triangle.rectTransform.AnchorTopCenter().PivotBottomCenter();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(style.Direction), style.Direction, null);
                }

                triangle.rectTransform.SetPositionXYAndSize(offset, size);

                triangleBackground.RectTransform.ResetAnchors()
                                   .ResetPivot()
                                   .ResetPositionAndScale()
                                   .SetRotation(0f, 0f, GetTriangleRotation(style.Direction))
                                   .ResetSize();

                triangleBackground.Image.color.OverrideValue(style.OutlineColor);
                triangleBackground.Image.sprite.OverrideValue(style.TriangleSprite);

                var triangleOutlineOffset = -3f * style.OutlineThickness;

                triangleForeground.RectTransform.ResetAnchors()
                                   .ResetPivot()
                                   .ResetScale()
                                   .SetRotation(0f, 0f, GetTriangleRotation(style.Direction))
                                   .SetPosition(
                                        style.Direction == AppearanceDirection.Left ? triangleOutlineOffset : 0f,
                                        style.Direction == AppearanceDirection.Right ? triangleOutlineOffset : 0f,
                                        style.Direction == AppearanceDirection.Above ? triangleOutlineOffset : 0f,
                                        style.Direction == AppearanceDirection.Below ? triangleOutlineOffset : 0f
                                    );

                triangleForeground.Image.color.OverrideValue(style.BackgroundColor);
                triangleForeground.Image.sprite.OverrideValue(style.TriangleSprite);
            }
        }

        private float GetTriangleRotation(AppearanceDirection appearanceDirection)
        {
            using (_PRF_GetTriangleRotation.Auto())
            {
                switch (appearanceDirection)
                {
                    case AppearanceDirection.Above:
                        return 45f;
                    case AppearanceDirection.Left:
                        return 135f;
                    case AppearanceDirection.Right:
                        return -45f;
                    case AppearanceDirection.Below:
                        return -135f;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(appearanceDirection), appearanceDirection, null);
                }
            }
        }

        #region ITooltipControlConfig Members

        public OutlinedImageComponentGroupConfig Background
        {
            get => background;
            protected set => background = value;
        }

        public BasicUIComponentGroupConfig Triangle
        {
            get => triangle;
            protected set => triangle = value;
        }

        public ImageComponentGroupConfig TriangleBackground
        {
            get => triangleBackground;
            protected set => triangleBackground = value;
        }

        public ImageComponentGroupConfig TriangleForeground
        {
            get => triangleForeground;
            protected set => triangleForeground = value;
        }

        public TextMeshProUGUIComponentGroupConfig Text
        {
            get => text;
            protected set => text = value;
        }

        public TooltipStyleOverride StyleOverride => style;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_FormatRectTransform =
            new ProfilerMarker(_PRF_PFX + nameof(FormatRectTransform));

        private static readonly ProfilerMarker _PRF_FormatText = new ProfilerMarker(_PRF_PFX + nameof(FormatText));

        private static readonly ProfilerMarker _PRF_FormatTooltipBackground =
            new ProfilerMarker(_PRF_PFX + nameof(FormatTooltipBackground));

        private static readonly ProfilerMarker _PRF_FormatTooltipTriangle =
            new ProfilerMarker(_PRF_PFX + nameof(FormatTooltipTriangle));

        private static readonly ProfilerMarker _PRF_GetTriangleRotation =
            new ProfilerMarker(_PRF_PFX + nameof(GetTriangleRotation));

        #endregion
    }
}
