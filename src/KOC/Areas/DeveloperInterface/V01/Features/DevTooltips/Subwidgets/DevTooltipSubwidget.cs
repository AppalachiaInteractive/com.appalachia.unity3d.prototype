using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Features.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets2;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Styling;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Widgets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Contracts;
using Appalachia.UI.Controls.Common;
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
                                                  DevTooltipSubwidget, DevTooltipsWidget, DevTooltipsWidgetMetadata,
                                                  DevTooltipsFeature, DevTooltipsFeatureMetadata>,
                                              IRectVisualizer,
                                              IActivable
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

        [OnValueChanged(nameof(OnChanged))]

        //[HideInInspector]
        [SerializeField]
        private DevTooltipComponentSetData componentSetData;

        #endregion

        public AppaButton CurrentTarget => _currentTarget;

        public IDevTooltipStyle CurrentStyle => _currentStyle;

        public string CurrentTooltip => _currentTooltip;

        public override void OnClicked()
        {
            using (_PRF_OnClicked.Auto())
            {
            }
        }

        public void SetCurrentStyle(DevTooltipStyleOverride newStyle)
        {
            using (_PRF_SetCurrentStyle.Auto())
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

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
            }
        }

        protected override void OnUpdateSubwidget()
        {
            using (_PRF_OnUpdateSubwidget.Auto())
            {
                if (!IsActive)
                {
                    Deactivate();
                }

                if (!gameObject.activeSelf)
                {
                    Activate();
                }

                if (componentSetData is { TooltipText: { TextMeshProUGUI: { } } } data)
                {
                    data.TooltipText.TextMeshProUGUI.fontStyle = Metadata.fontStyle;
                }

                DevTooltipComponentSetData.RefreshAndUpdate(ref componentSetData, ref componentSet, gameObject, name);

                if (!IsActive)
                {
                    gameObject.SetActive(false);
                }

                var textComponent = componentSet.tooltipText.TextMeshProUGUI;

                var tooltipRectTransform = componentSetData.RectTransform;
                var tooltipBackground = componentSetData.Background;
                var triangleParent = componentSetData.TriangleParent;
                var triangleBackground = componentSetData.TriangleBackground;
                var triangleForeground = componentSetData.TriangleForeground;
                var tooltipText = componentSetData.TooltipText;

                if (_currentStyle == null)
                {
                    return;
                }

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
                DevTooltipComponentSetData.RefreshAndUpdate(ref componentSetData, ref componentSet, gameObject, name);
            }
        }

        private void FormatRectTransform(TextMeshProUGUI textComponent, RectTransformData tooltipRectTransform)
        {
            using (_PRF_FormatRectTransform.Auto())
            {
                var requiredPaddingSize = CurrentStyle.TextPadding * 2f * Vector2.one;
                Vector2 requiredTextSize;
                requiredTextSize = textComponent.text.IsNullOrEmpty()
                    ? new Vector2(0, 0)
                    : textComponent.GetPreferredValues(CurrentTooltip);

                var requiredSize = requiredPaddingSize + requiredTextSize;

                var targetButton = CurrentTarget;

                var targetRect = targetButton.rectTransform.GetScreenSpaceRect();

                calculatedTargetCenter = targetRect.center;

                tooltipRectTransform.AnchorBottomLeft().ResetRotationAndScale();

                switch (CurrentStyle.Direction)
                {
                    case AppearanceDirection.Above:
                        calculatedTooltipReferencePoint = targetRect.GetTopCenter();
                        calculatedTooltipOffset = new Vector2(0f, CurrentStyle.DistanceFromTarget);
                        break;
                    case AppearanceDirection.Left:
                        calculatedTooltipReferencePoint = targetRect.GetMiddleLeft();
                        calculatedTooltipOffset = new Vector2(-CurrentStyle.DistanceFromTarget, 0f);
                        break;
                    case AppearanceDirection.Right:
                        calculatedTooltipReferencePoint = targetRect.GetMiddleRight();
                        calculatedTooltipOffset = new Vector2(CurrentStyle.DistanceFromTarget, 0f);
                        break;
                    case AppearanceDirection.Below:
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
                    case AppearanceDirection.Above:
                        tooltipRectTransform.PivotBottomCenter();
                        break;
                    case AppearanceDirection.Left:
                        tooltipRectTransform.PivotMiddleRight();
                        break;
                    case AppearanceDirection.Right:
                        tooltipRectTransform.PivotMiddleLeft();
                        break;
                    case AppearanceDirection.Below:
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

                tooltipText.RectTransform.ResetAnchorsRotationAndScale().ResetSize().Inset(CurrentStyle.TextPadding);

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
                    AppearanceDirection.Above => new Vector2(0f, .5f * CurrentStyle.TriangleSize),
                    AppearanceDirection.Left  => new Vector2(.5f * -CurrentStyle.TriangleSize, 0f),
                    AppearanceDirection.Right => new Vector2(.5f * CurrentStyle.TriangleSize, 0f),
                    AppearanceDirection.Below => new Vector2(0f, .5f * -CurrentStyle.TriangleSize),
                    _                         => throw new ArgumentOutOfRangeException()
                };

                triangleParent.ResetRotationAndScale();
                switch (CurrentStyle.Direction)
                {
                    case AppearanceDirection.Above:
                        triangleParent.AnchorBottomCenter().PivotTopCenter();
                        break;
                    case AppearanceDirection.Left:
                        triangleParent.AnchorMiddleRight().PivotMiddleLeft();
                        break;
                    case AppearanceDirection.Right:
                        triangleParent.AnchorMiddleLeft().PivotMiddleRight();
                        break;
                    case AppearanceDirection.Below:
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
                                       CurrentStyle.Direction == AppearanceDirection.Left ? triangleOutlineOffset : 0f,
                                       CurrentStyle.Direction == AppearanceDirection.Right ? triangleOutlineOffset : 0f,
                                       CurrentStyle.Direction == AppearanceDirection.Above ? triangleOutlineOffset : 0f,
                                       CurrentStyle.Direction == AppearanceDirection.Below ? triangleOutlineOffset : 0f
                                   );

                triangleForeground.Image.color.OverrideValue(CurrentStyle.BackgroundColor);
                triangleForeground.Image.sprite.OverrideValue(CurrentStyle.TriangleSprite);
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

        #region IActivable Members

        public void Deactivate()
        {
            using (_PRF_Deactivate.Auto())
            {
                if (gameObject.activeSelf)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        public void Activate()
        {
            using (_PRF_Activate.Auto())
            {
                if (!gameObject.activeSelf)
                {
                    gameObject.SetActive(true);
                }
            }
        }

        public void Toggle()
        {
            using (_PRF_Toggle.Auto())
            {
                if (!IsActive)
                {
                    Activate();
                }
                else
                {
                    Deactivate();
                }
            }
        }

        public bool IsActive => Widget.ActiveSubwidget == this;

        #endregion

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

        private static readonly ProfilerMarker _PRF_Activate = new ProfilerMarker(_PRF_PFX + nameof(Activate));

        private static readonly ProfilerMarker _PRF_Deactivate = new ProfilerMarker(_PRF_PFX + nameof(Deactivate));

        private static readonly ProfilerMarker _PRF_FormatRectTransform =
            new ProfilerMarker(_PRF_PFX + nameof(FormatRectTransform));

        private static readonly ProfilerMarker _PRF_FormatText = new ProfilerMarker(_PRF_PFX + nameof(FormatText));

        private static readonly ProfilerMarker _PRF_FormatTooltipBackground =
            new ProfilerMarker(_PRF_PFX + nameof(FormatTooltipBackground));

        private static readonly ProfilerMarker _PRF_FormatTooltipTriangle =
            new ProfilerMarker(_PRF_PFX + nameof(FormatTooltipTriangle));

        private static readonly ProfilerMarker _PRF_GetTriangleRotation =
            new ProfilerMarker(_PRF_PFX + nameof(GetTriangleRotation));

        private static readonly ProfilerMarker _PRF_SetCurrentStyle =
            new ProfilerMarker(_PRF_PFX + nameof(SetCurrentStyle));

        private static readonly ProfilerMarker _PRF_Toggle = new ProfilerMarker(_PRF_PFX + nameof(Toggle));

        private static readonly ProfilerMarker _PRF_UpdateCurrentTarget =
            new ProfilerMarker(_PRF_PFX + nameof(SetCurrentTarget));

        private static readonly ProfilerMarker _PRF_UpdateCurrentTooltip =
            new ProfilerMarker(_PRF_PFX + nameof(SetCurrentTooltip));

        private static readonly ProfilerMarker _PRF_VisualizeRectangles =
            new ProfilerMarker(_PRF_PFX + nameof(VisualizeRectangles));

        #endregion
    }
}
