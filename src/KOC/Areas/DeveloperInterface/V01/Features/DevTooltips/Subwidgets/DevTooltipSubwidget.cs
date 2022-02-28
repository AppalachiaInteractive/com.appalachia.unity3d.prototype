using System;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Styling;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Contracts;
using Appalachia.UI.Controls.Components.Buttons;
using Appalachia.UI.Controls.Extensions;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Components.Subsets;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Events;
using Appalachia.Utility.Extensions;
using Drawing;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets
{
    public sealed class DevTooltipSubwidget : DeveloperInterfaceManager_V01.ControlledSubwidget<
                                                  DevTooltipSubwidget, DevTooltipsWidget,
                                                  DevTooltipsWidgetMetadata, DevTooltipsFeature,
                                                  DevTooltipsFeatureMetadata>,
                                              IRectVisualizer
    {
        #region Fields and Autoproperties

        [SerializeField]
        [ReadOnly]
        public DevTooltipComponentSet componentSet;

        [SerializeField]
        [TextArea]
        [ReadOnly]
        private string _currentTooltip;

        [SerializeField]
        [ReadOnly]
        private AppaButton _currentTarget;

        [SerializeField]
        [ReadOnly]
        private DevTooltipStyleOverride _currentStyle;

        private Vector2 calculatedTooltipReferencePoint;
        private Vector2 calculatedTooltipOffset;
        private Vector2 calculatedTooltipFinalPosition;
        private Vector2 calculatedTargetCenter;

        [SerializeField]
        [OnValueChanged(nameof(OnChanged))]
        //[HideInInspector]
        private DevTooltipComponentSetData componentSetData;

        #endregion

        public AppaButton CurrentTarget => _currentTarget;

        public IDevTooltipStyle CurrentStyle => _currentStyle;

        public string CurrentTooltip => _currentTooltip;

        public void SetCurrentStyle(DevTooltipStyleOverride newStyle)
        {
            using (_PRF_UpdateCurrentStyle.Auto())
            {
                if (_currentStyle == newStyle)
                {
                    return;
                }

                _currentStyle = newStyle;
                OnChanged();
            }
        }

        public void SetCurrentTarget(AppaButton newTarget)
        {
            using (_PRF_UpdateCurrentTarget.Auto())
            {
                if (_currentTarget == newTarget)
                {
                    return;
                }

                _currentTarget = newTarget;
                OnChanged();
            }
        }

        public void SetCurrentTooltip(string newTooltip)
        {
            using (_PRF_UpdateCurrentTooltip.Auto())
            {
                if (_currentTooltip == newTooltip)
                {
                    return;
                }

                _currentTooltip = newTooltip;
                OnChanged();
            }
        }

        protected override void OnUpdateSubwidget()
        {
            using (_PRF_OnUpdateSubwidget.Auto())
            {
                DevTooltipComponentSetData.RefreshAndUpdateComponentSet(
                    ref componentSetData,
                    ref componentSet,
                    gameObject,
                    name
                );
                
                var textComponent = componentSet.tooltipText.TextMeshProUGUI;

                var tooltipRectTransform = componentSetData.RectTransform;
                var tooltipBackground = componentSetData.Background;
                var triangleParent = componentSetData.TriangleParent;
                var triangleBackground = componentSetData.TriangleBackground;
                var triangleForeground = componentSetData.TriangleForeground;
                var tooltipText = componentSetData.TooltipText;

                FormatRectTransform(textComponent, tooltipRectTransform);

                FormatTooltipBackground(tooltipBackground);

                FormatTooltipTriangle(triangleParent, triangleForeground, triangleBackground);

                FormatText(textComponent, tooltipText);
            }
        }

        /// <inheritdoc />
        protected override async AppaTask WhenEnabled()
        {
            await base.WhenEnabled();

            using (_PRF_WhenEnabled.Auto())
            {
                DevTooltipComponentSetData.RefreshAndUpdateComponentSet(
                    ref componentSetData,
                    ref componentSet,
                    gameObject,
                    name
                );
            }
        }

        private void FormatRectTransform(
            TextMeshProUGUI textComponent,
            RectTransformData tooltipRectTransform)
        {
            using (_PRF_FormatRectTransform.Auto())
            {
                var requiredPaddingSize = CurrentStyle.TextPadding * 2f * Vector2.one;
                var requiredTextSize = textComponent.GetPreferredValues(CurrentTooltip);

                var requiredSize = requiredPaddingSize + requiredTextSize;

                var targetButton = CurrentTarget;

                var targetRect = targetButton.rectTransform.GetScreenSpaceRect();

                calculatedTargetCenter = targetRect.center;

                tooltipRectTransform.AnchorBottomLeft().ResetRotationAndScale();

                switch (CurrentStyle.Direction)
                {
                    case TooltipAppearanceDirection.Above:
                        calculatedTooltipReferencePoint = targetRect.GetTopCenter();
                        calculatedTooltipOffset = new Vector2(0f, CurrentStyle.DistanceFromTarget);
                        break;
                    case TooltipAppearanceDirection.Left:
                        calculatedTooltipReferencePoint = targetRect.GetMiddleLeft();
                        calculatedTooltipOffset = new Vector2(-CurrentStyle.DistanceFromTarget, 0f);
                        break;
                    case TooltipAppearanceDirection.Right:
                        calculatedTooltipReferencePoint = targetRect.GetMiddleRight();
                        calculatedTooltipOffset = new Vector2(CurrentStyle.DistanceFromTarget, 0f);
                        break;
                    case TooltipAppearanceDirection.Below:
                        calculatedTooltipReferencePoint = targetRect.GetBottomCenter();
                        calculatedTooltipOffset = new Vector2(0f, -CurrentStyle.DistanceFromTarget);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                calculatedTooltipFinalPosition = calculatedTooltipReferencePoint + calculatedTooltipOffset;

                tooltipRectTransform.SetPositionXYAndSize(calculatedTooltipFinalPosition, requiredSize);

                switch (CurrentStyle.Direction)
                {
                    case TooltipAppearanceDirection.Above:
                        tooltipRectTransform.PivotBottomCenter();
                        break;
                    case TooltipAppearanceDirection.Left:
                        tooltipRectTransform.PivotMiddleRight();
                        break;
                    case TooltipAppearanceDirection.Right:
                        tooltipRectTransform.PivotMiddleLeft();
                        break;
                    case TooltipAppearanceDirection.Below:
                        tooltipRectTransform.PivotTopCenter();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(CurrentStyle.Direction),
                            CurrentStyle.Direction,
                            null
                        );
                }
            }
        }

        private void FormatText(TextMeshProUGUI textComponent, TextMeshProUGUISubsetData tooltipText)
        {
            using (_PRF_FormatText.Auto())
            {
                textComponent.text = CurrentTooltip;

                tooltipText.RectTransform.ResetAnchorsRotationAndScale()
                           .ResetSize()
                           .Inset(CurrentStyle.TextPadding);

                tooltipText.TextMeshProUGUI.fontStyle.OverflowMode = TextOverflowModes.Truncate;
                tooltipText.TextMeshProUGUI.fontStyle.HorizontalAlignment = HorizontalAlignmentOptions.Left;
                tooltipText.TextMeshProUGUI.fontStyle.VerticalAlignment = VerticalAlignmentOptions.Top;
            }
        }

        private void FormatTooltipBackground(OutlinedImageSubsetData tooltipBackground)
        {
            using (_PRF_FormatTooltipBackground.Auto())
            {
                tooltipBackground.RectTransform.FullScreen();

                tooltipBackground.Outline.effectColor.OverrideValue(CurrentStyle.OutlineColor);
                var v2Thickness = CurrentStyle.OutlineThickness * Vector2.one;
                tooltipBackground.Outline.effectDistance.OverrideValue(v2Thickness);
                tooltipBackground.Image.color.OverrideValue(CurrentStyle.BackgroundColor);
                tooltipBackground.Image.sprite.OverrideValue(null);
            }
        }

        private void FormatTooltipTriangle(
            RectTransformData triangleParent,
            ImageSubsetData triangleForeground,
            ImageSubsetData triangleBackground)
        {
            using (_PRF_FormatTooltipTriangle.Auto())
            {
                if (!CurrentStyle.ShowTriangle)
                {
                    triangleForeground.Enabled = false;
                    triangleBackground.Enabled = false;
                    return;
                }

                triangleForeground.Enabled = true;
                triangleBackground.Enabled = true;

                var size = Vector2.one * CurrentStyle.TriangleSize;

                var offset = CurrentStyle.Direction switch
                {
                    TooltipAppearanceDirection.Above => new Vector2(0f, .5f * CurrentStyle.TriangleSize),
                    TooltipAppearanceDirection.Left  => new Vector2(.5f * -CurrentStyle.TriangleSize, 0f),
                    TooltipAppearanceDirection.Right => new Vector2(.5f * CurrentStyle.TriangleSize, 0f),
                    TooltipAppearanceDirection.Below => new Vector2(0f, .5f * -CurrentStyle.TriangleSize),
                    _                                => throw new ArgumentOutOfRangeException()
                };

                triangleParent.ResetRotationAndScale();
                switch (CurrentStyle.Direction)
                {
                    case TooltipAppearanceDirection.Above:
                        triangleParent.AnchorBottomCenter().PivotTopCenter();
                        break;
                    case TooltipAppearanceDirection.Left:
                        triangleParent.AnchorMiddleRight().PivotMiddleLeft();
                        break;
                    case TooltipAppearanceDirection.Right:
                        triangleParent.AnchorMiddleLeft().PivotMiddleRight();
                        break;
                    case TooltipAppearanceDirection.Below:
                        triangleParent.AnchorTopCenter().PivotBottomCenter();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(CurrentStyle.Direction),
                            CurrentStyle.Direction,
                            null
                        );
                }

                triangleParent.SetPositionXYAndSize(offset, size);

                triangleBackground.RectTransform.ResetAnchors()
                                  .ResetPivot()
                                  .ResetPositionAndScale()
                                  .SetRotation(0f, 0f, GetTriangleRotation(CurrentStyle.Direction))
                                  .ResetSize();

                triangleBackground.Image.color.OverrideValue(CurrentStyle.OutlineColor);
                triangleBackground.Image.sprite.OverrideValue(CurrentStyle.TriangleSprite);

                var triangleOutlineOffset = -3f * CurrentStyle.OutlineThickness;

                triangleForeground.RectTransform.ResetAnchors()
                                  .ResetPivot()
                                  .ResetScale()
                                  .SetRotation(0f, 0f, GetTriangleRotation(CurrentStyle.Direction))
                                  .SetPosition(
                                       CurrentStyle.Direction == TooltipAppearanceDirection.Left
                                           ? triangleOutlineOffset
                                           : 0f,
                                       CurrentStyle.Direction == TooltipAppearanceDirection.Right
                                           ? triangleOutlineOffset
                                           : 0f,
                                       CurrentStyle.Direction == TooltipAppearanceDirection.Above
                                           ? triangleOutlineOffset
                                           : 0f,
                                       CurrentStyle.Direction == TooltipAppearanceDirection.Below
                                           ? triangleOutlineOffset
                                           : 0f
                                   );

                triangleForeground.Image.color.OverrideValue(CurrentStyle.BackgroundColor);
                triangleForeground.Image.sprite.OverrideValue(CurrentStyle.TriangleSprite);
            }
        }

        private float GetTriangleRotation(TooltipAppearanceDirection appearanceDirection)
        {
            using (_PRF_GetTriangleRotation.Auto())
            {
                switch (appearanceDirection)
                {
                    case TooltipAppearanceDirection.Above:
                        return 45f;
                    case TooltipAppearanceDirection.Left:
                        return 135f;
                    case TooltipAppearanceDirection.Right:
                        return -45f;
                    case TooltipAppearanceDirection.Below:
                        return -135f;
                    default:
                        throw new ArgumentOutOfRangeException(
                            nameof(appearanceDirection),
                            appearanceDirection,
                            null
                        );
                }
            }
        }

        #region IRectVisualizer Members

        public void VisualizeRectangles(AppaEvent<CommandBuilder>.Args drawArgs)
        {
            using (_PRF_VisualizeRectangles.Auto())
            {
                var draw = drawArgs.value;
                var alpha = 1f;
                var targetColor = Color.green.ScaleA(alpha);
                var arrowColor = Color.cyan.ScaleA(alpha);

                using (draw.WithLineWidth(2f))
                {
                    draw.Line(calculatedTargetCenter, calculatedTooltipReferencePoint, targetColor);

                    draw.Line(calculatedTooltipReferencePoint, calculatedTooltipFinalPosition, arrowColor);
                }
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_FormatRectTransform =
            new ProfilerMarker(_PRF_PFX + nameof(FormatRectTransform));

        private static readonly ProfilerMarker _PRF_FormatText =
            new ProfilerMarker(_PRF_PFX + nameof(FormatText));

        private static readonly ProfilerMarker _PRF_FormatTooltipBackground =
            new ProfilerMarker(_PRF_PFX + nameof(FormatTooltipBackground));

        private static readonly ProfilerMarker _PRF_FormatTooltipTriangle =
            new ProfilerMarker(_PRF_PFX + nameof(FormatTooltipTriangle));

        private static readonly ProfilerMarker _PRF_GetTriangleRotation =
            new ProfilerMarker(_PRF_PFX + nameof(GetTriangleRotation));

        private static readonly ProfilerMarker _PRF_UpdateCurrentStyle =
            new ProfilerMarker(_PRF_PFX + nameof(SetCurrentStyle));

        private static readonly ProfilerMarker _PRF_UpdateCurrentTarget =
            new ProfilerMarker(_PRF_PFX + nameof(SetCurrentTarget));

        private static readonly ProfilerMarker _PRF_UpdateCurrentTooltip =
            new ProfilerMarker(_PRF_PFX + nameof(SetCurrentTooltip));

        private static readonly ProfilerMarker _PRF_VisualizeRectangles =
            new ProfilerMarker(_PRF_PFX + nameof(VisualizeRectangles));

        #endregion
    }
}
