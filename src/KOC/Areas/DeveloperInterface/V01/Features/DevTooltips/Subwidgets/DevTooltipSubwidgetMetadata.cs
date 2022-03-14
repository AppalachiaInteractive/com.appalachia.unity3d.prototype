using System;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Styling;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets.Contracts;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Widgets;
using Appalachia.UI.ControlModel.Components;
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

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Subwidgets
{
    public sealed class DevTooltipSubwidgetMetadata : DeveloperInterfaceMetadata_V01.InstancedSubwidgetMetadata<
                                                          DevTooltipSubwidget, DevTooltipSubwidgetMetadata,
                                                          IDevTooltipSubwidget, IDevTooltipSubwidgetMetadata,
                                                          DevTooltipsWidget, DevTooltipsWidgetMetadata,
                                                          DevTooltipsFeature, DevTooltipsFeatureMetadata>,
                                                      IDevTooltipSubwidgetMetadata
    {
        #region Fields and Autoproperties

        protected override bool ShowsTooltip => false;
        
        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [HideIf(nameof(HideAllFields))]
        private DevTooltipStyleOverride _styleOverride;

        [OnValueChanged(nameof(OnChanged))]
        [SerializeField]
        [HideIf(nameof(HideAllFields))]
        private DevTooltipControlConfig _componentControlConfig;

        #endregion

        protected override void UpdateFunctionalityInternal(DevTooltipSubwidget subwidget)
        {
            using (_PRF_UpdateFunctionalityInternal.Auto())
            {
                DevTooltipControlConfig.Refresh(ref _componentControlConfig, this);

                _componentControlConfig.TooltipText.TextMeshProUGUI.fontStyle = WidgetMetadata.fontStyle;

                if (!subwidget.IsActive)
                {
                    subwidget.gameObject.SetActive(false);
                }

                var currentStyle = _styleOverride == null ? subwidget.CurrentStyle : _styleOverride;
                var currentTooltip = subwidget.CurrentTooltip;

                var textComponent = subwidget.control.tooltipText.TextMeshProUGUI;

                var requiredTextSize = currentTooltip.IsNullOrEmpty()
                    ? new Vector2(0, 0)
                    : textComponent.GetPreferredValues(currentTooltip);

                var tooltipRectTransform = _componentControlConfig.RectTransform;
                var tooltipBackground = _componentControlConfig.Background;
                var triangleParent = _componentControlConfig.TriangleParent;
                var triangleBackground = _componentControlConfig.TriangleBackground;
                var triangleForeground = _componentControlConfig.TriangleForeground;
                var tooltipText = _componentControlConfig.TooltipText;

                if (currentStyle == null)
                {
                    return;
                }

                FormatRectTransform(subwidget, currentStyle, tooltipRectTransform, requiredTextSize);

                FormatTooltipBackground(subwidget, currentStyle, tooltipBackground);

                FormatTooltipTriangle(subwidget, currentStyle, triangleParent, triangleForeground, triangleBackground);

                FormatText(subwidget, currentStyle, tooltipText);

                DevTooltipControlConfig.RefreshAndApply(
                    ref _componentControlConfig,
                    ref subwidget.control,
                    subwidget.canvas.GameObject,
                    name,
                    this
                );

                textComponent.text = currentTooltip;
            }
        }

        private void FormatRectTransform(
            DevTooltipSubwidget subwidget,
            IDevTooltipStyle currentStyle,
            RectTransformConfig tooltipRectTransform,
            Vector2 requiredTextSize)
        {
            using (_PRF_FormatRectTransform.Auto())
            {
                var requiredPaddingSize = currentStyle.TextPadding * 2f * Vector2.one;

                var requiredSize = requiredPaddingSize + requiredTextSize;

                var targetButton = subwidget.CurrentTarget;

                var targetRect = targetButton.rectTransform.GetScreenSpaceRect();

                subwidget.calculatedTargetCenter = targetRect.center;

                tooltipRectTransform.AnchorBottomLeft().ResetRotationAndScale();

                switch (currentStyle.Direction)
                {
                    case AppearanceDirection.Above:
                        subwidget.calculatedTooltipReferencePoint = targetRect.GetTopCenter();
                        subwidget.calculatedTooltipOffset = new Vector2(0f, currentStyle.DistanceFromTarget);
                        break;
                    case AppearanceDirection.Left:
                        subwidget.calculatedTooltipReferencePoint = targetRect.GetMiddleLeft();
                        subwidget.calculatedTooltipOffset = new Vector2(-currentStyle.DistanceFromTarget, 0f);
                        break;
                    case AppearanceDirection.Right:
                        subwidget.calculatedTooltipReferencePoint = targetRect.GetMiddleRight();
                        subwidget.calculatedTooltipOffset = new Vector2(currentStyle.DistanceFromTarget, 0f);
                        break;
                    case AppearanceDirection.Below:
                        subwidget.calculatedTooltipReferencePoint = targetRect.GetBottomCenter();
                        subwidget.calculatedTooltipOffset = new Vector2(0f, -currentStyle.DistanceFromTarget);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                subwidget.calculatedTooltipFinalPosition =
                    subwidget.calculatedTooltipReferencePoint + subwidget.calculatedTooltipOffset;

                tooltipRectTransform.SetPositionXYAndSize(subwidget.calculatedTooltipFinalPosition, requiredSize);

                switch (currentStyle.Direction)
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
                            nameof(currentStyle.Direction),
                            currentStyle.Direction,
                            null
                        );
                }
            }
        }

        private void FormatText(
            DevTooltipSubwidget subwidget,
            IDevTooltipStyle currentStyle,
            TextMeshProUGUIComponentGroupConfig tooltipText)
        {
            using (_PRF_FormatText.Auto())
            {
                tooltipText.RectTransform.ResetAnchorsRotationAndScale().ResetSize().Inset(currentStyle.TextPadding);

                tooltipText.TextMeshProUGUI.fontStyle.OverflowMode = TextOverflowModes.Truncate;
                tooltipText.TextMeshProUGUI.fontStyle.HorizontalAlignment = HorizontalAlignmentOptions.Left;
                tooltipText.TextMeshProUGUI.fontStyle.VerticalAlignment = VerticalAlignmentOptions.Top;
            }
        }

        private void FormatTooltipBackground(
            DevTooltipSubwidget subwidget,
            IDevTooltipStyle currentStyle,
            OutlineImageComponentGroupConfig tooltipBackground)
        {
            using (_PRF_FormatTooltipBackground.Auto())
            {
                tooltipBackground.RectTransform.FullScreen();

                tooltipBackground.Outline.effectColor.OverrideValue(currentStyle.OutlineColor);
                var v2Thickness = currentStyle.OutlineThickness * Vector2.one;
                tooltipBackground.Outline.effectDistance.OverrideValue(v2Thickness);
                tooltipBackground.Image.color.OverrideValue(currentStyle.BackgroundColor);
                tooltipBackground.Image.sprite.OverrideValue(null);
            }
        }

        private void FormatTooltipTriangle(
            DevTooltipSubwidget subwidget,
            IDevTooltipStyle currentStyle,
            RectTransformConfig triangleParent,
            ImageComponentGroupConfig triangleForeground,
            ImageComponentGroupConfig triangleBackground)
        {
            using (_PRF_FormatTooltipTriangle.Auto())
            {
                triangleForeground.ShouldEnableFunction = () => currentStyle.ShowTriangle;
                triangleBackground.ShouldEnableFunction = () => currentStyle.ShowTriangle;

                if (!currentStyle.ShowTriangle)
                {
                    return;
                }

                var size = Vector2.one * currentStyle.TriangleSize;

                var offset = currentStyle.Direction switch
                {
                    AppearanceDirection.Above => new Vector2(0f, .5f * currentStyle.TriangleSize),
                    AppearanceDirection.Left  => new Vector2(.5f * -currentStyle.TriangleSize, 0f),
                    AppearanceDirection.Right => new Vector2(.5f * currentStyle.TriangleSize, 0f),
                    AppearanceDirection.Below => new Vector2(0f, .5f * -currentStyle.TriangleSize),
                    _                         => throw new ArgumentOutOfRangeException()
                };

                triangleParent.ResetRotationAndScale();
                switch (currentStyle.Direction)
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
                            nameof(currentStyle.Direction),
                            currentStyle.Direction,
                            null
                        );
                }

                triangleParent.SetPositionXYAndSize(offset, size);

                triangleBackground.RectTransform.ResetAnchors()
                                  .ResetPivot()
                                  .ResetPositionAndScale()
                                  .SetRotation(0f, 0f, GetTriangleRotation(currentStyle.Direction))
                                  .ResetSize();

                triangleBackground.Image.color.OverrideValue(currentStyle.OutlineColor);
                triangleBackground.Image.sprite.OverrideValue(currentStyle.TriangleSprite);

                var triangleOutlineOffset = -3f * currentStyle.OutlineThickness;

                triangleForeground.RectTransform.ResetAnchors()
                                  .ResetPivot()
                                  .ResetScale()
                                  .SetRotation(0f, 0f, GetTriangleRotation(currentStyle.Direction))
                                  .SetPosition(
                                       currentStyle.Direction == AppearanceDirection.Left ? triangleOutlineOffset : 0f,
                                       currentStyle.Direction == AppearanceDirection.Right ? triangleOutlineOffset : 0f,
                                       currentStyle.Direction == AppearanceDirection.Above ? triangleOutlineOffset : 0f,
                                       currentStyle.Direction == AppearanceDirection.Below ? triangleOutlineOffset : 0f
                                   );

                triangleForeground.Image.color.OverrideValue(currentStyle.BackgroundColor);
                triangleForeground.Image.sprite.OverrideValue(currentStyle.TriangleSprite);
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

        #region IDevTooltipSubwidgetMetadata Members

        public DevTooltipStyleOverride StyleOverride => _styleOverride;

        public DevTooltipControlConfig ControlConfig => _componentControlConfig;

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
