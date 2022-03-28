using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Control.Contracts;
using Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Components;
using Appalachia.UI.ControlModel.ComponentGroups.Default;
using Appalachia.UI.ControlModel.ComponentGroups.Fadeable;
using Appalachia.UI.ControlModel.Controls.Default;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.UI.Functionality.Images.Groups.Outline;
using Appalachia.UI.Functionality.Text.Groups;
using Appalachia.Utility.Async;
using Appalachia.Utility.Colors;
using Appalachia.Utility.Events;
using Appalachia.Utility.Extensions;
using Drawing;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Control.Base
{
    /// <summary>
    ///     Defines the members necessary for creating and configuring
    ///     the components of a Tooltip control.
    /// </summary>
    /// <typeparam name="TControl">The control.</typeparam>
    /// <typeparam name="TConfig">Configuration for the control.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseTooltipControl<TControl, TConfig> : AppaUIControl<TControl, TConfig>,
                                                                     ITooltipControl
        where TControl : BaseTooltipControl<TControl, TConfig>, new()
        where TConfig : BaseTooltipControlConfig<TControl, TConfig>, new()

    {
        #region Constants and Static Readonly

        protected const int ORDER_TOOLTIP = ORDER_ROOT + 50;

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_TOOLTIP + 10)]
        [SerializeField]
        public OutlinedImageComponentGroup background;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_TOOLTIP + 19)]
        [SerializeField]
        public BasicUIComponentGroup triangle;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_TOOLTIP + 20)]
        [SerializeField]
        public ImageComponentGroup triangleBackground;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_TOOLTIP + 30)]
        [SerializeField]
        public ImageComponentGroup triangleForeground;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_TOOLTIP + 40)]
        [SerializeField]
        public TextMeshProUGUIComponentGroup tooltipText;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_TOOLTIP + 41)]
        [SerializeField]
        public FadeableComponentGroup tooltip;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_OBJECTS + 10)]
        [SerializeField]
        [ReadOnly]
        private GameObject _triangleParent;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_OBJECTS + 19)]
        [SerializeField]
        [ReadOnly]
        private GameObject _triangleBackgroundParent;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_OBJECTS + 20)]
        [SerializeField]
        [ReadOnly]
        private GameObject _triangleForegroundParent;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_OBJECTS + 30)]
        [SerializeField]
        [ReadOnly]
        private GameObject _backgroundParent;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_OBJECTS + 40)]
        [SerializeField]
        [ReadOnly]
        private GameObject _tooltipParent;

        [FoldoutGroup(GROUP_COMP)]
        [PropertyOrder(ORDER_OBJECTS + 41)]
        [SerializeField]
        [ReadOnly]
        private GameObject _tooltipTextParent;

        [FormerlySerializedAs("_currentTarget")]
        [SerializeField]
        [ReadOnly]
        private AppaTooltipTarget _target;

        public Vector2 calculatedTooltipReferencePoint;
        public Vector2 calculatedTooltipOffset;
        public Vector2 calculatedTooltipFinalPosition;
        public Vector2 calculatedTargetCenter;

        [SerializeField]
        [ReadOnly]
        public string tooltipContent;

        #endregion

        public bool IsActive => CurrentAlpha > float.Epsilon;

        public float CurrentAlpha => this.canvasGroup.alpha;

        /// <inheritdoc />
        public override void DestroySafely()
        {
            using (_PRF_DestroySafely.Auto())
            {
                if (background != null)
                {
                    background.DestroySafely();
                }

                if (triangle != null)
                {
                    triangle.DestroySafely();
                }

                if (triangleBackground != null)
                {
                    triangleBackground.DestroySafely();
                }

                if (triangleForeground != null)
                {
                    triangleForeground.DestroySafely();
                }

                if (tooltipText != null)
                {
                    tooltipText.DestroySafely();
                }

                if (tooltip != null)
                {
                    tooltip.DestroySafely();
                }
            }
        }

        /// <inheritdoc />
        public override void Disable()
        {
            using (_PRF_Disable.Auto())
            {
                base.Disable();

                if (background != null)
                {
                    background.Disable();
                }

                if (triangle != null)
                {
                    triangle.Disable();
                }

                if (triangleBackground != null)
                {
                    triangleBackground.Disable();
                }

                if (triangleForeground != null)
                {
                    triangleForeground.Disable();
                }

                if (tooltipText != null)
                {
                    tooltipText.Disable();
                }

                if (tooltip != null)
                {
                    tooltip.Disable();
                }
            }
        }

        /// <inheritdoc />
        public override void Enable(TConfig config)
        {
            using (_PRF_Enable.Auto())
            {
                base.Enable(config);

                if (background != null)
                {
                    background.Enable(config.Background);
                }

                if (triangle != null)
                {
                    triangle.Enable(config.Triangle);
                }

                if (triangleBackground != null)
                {
                    triangleBackground.Enable(config.TriangleBackground);
                }

                if (triangleForeground != null)
                {
                    triangleForeground.Enable(config.TriangleForeground);
                }

                if (tooltipText != null)
                {
                    tooltipText.Enable(config.TooltipText);
                }

                if (tooltip != null)
                {
                    tooltip.enabled = true;
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

        public void SetCurrentTarget(RectTransform newTarget)
        {
            using (_PRF_SetCurrentTarget.Auto())
            {
                if (newTarget == null)
                {
                    return;
                }

                if (_target != null)
                {
                    if (_target.rectTransform == newTarget)
                    {
                        return;
                    }
                }

                AppaTooltipTarget newTargetComponent = null;

                newTarget.gameObject.GetOrAddComponent(ref newTargetComponent);

                _target = newTargetComponent;
                OnChanged();
            }
        }

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

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
            }
        }

        /// <inheritdoc />
        protected override void OnRefresh()
        {
            using (_PRF_OnRefresh.Auto())
            {
                base.OnRefresh();

                _tooltipParent = gameObject;
                FadeableComponentGroup.Refresh(ref tooltip, TooltipParent, nameof(tooltip));

                var tooltipGameObject = tooltip.gameObject;

                _backgroundParent = tooltipGameObject;
                _triangleParent = tooltipGameObject;
                _tooltipTextParent = tooltipGameObject;

                OutlinedImageComponentGroup.Refresh(ref background, BackgroundParent, nameof(background));
                BasicUIComponentGroup.Refresh(ref triangle, TriangleParent, nameof(triangle));
                TextMeshProUGUIComponentGroup.Refresh(ref tooltipText, TooltipTextParent, nameof(tooltipText));

                background.transform.SetSiblingIndex(0);
                triangle.transform.SetSiblingIndex(1);
                tooltipText.transform.SetSiblingIndex(2);

                var triangleGameObject = triangle.gameObject;
                _triangleBackgroundParent = triangleGameObject;
                _triangleForegroundParent = triangleGameObject;

                ImageComponentGroup.Refresh(
                    ref triangleBackground,
                    TriangleBackgroundParent,
                    nameof(triangleBackground)
                );
                ImageComponentGroup.Refresh(
                    ref triangleForeground,
                    TriangleForegroundParent,
                    nameof(triangleForeground)
                );
            }
        }

        #region ITooltipControl Members

        public AppaTooltipTarget Target => _target;

        public GameObject TooltipParent => _tooltipParent;

        public ImageComponentGroup TriangleBackground => triangleBackground;
        public ImageComponentGroup TriangleForeground => triangleForeground;
        public OutlinedImageComponentGroup Background => background;
        public FadeableComponentGroup Tooltip => tooltip;
        public TextMeshProUGUIComponentGroup TooltipText => tooltipText;
        public BasicUIComponentGroup Triangle => triangle;

        public GameObject TriangleBackgroundParent => _triangleBackgroundParent;

        public GameObject TriangleForegroundParent => _triangleForegroundParent;

        public GameObject BackgroundParent => _backgroundParent;

        public GameObject TooltipTextParent => _tooltipTextParent;

        public GameObject TriangleParent => _triangleParent;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_Activate = new ProfilerMarker(_PRF_PFX + nameof(Activate));

        private static readonly ProfilerMarker _PRF_Deactivate = new ProfilerMarker(_PRF_PFX + nameof(Deactivate));

        private static readonly ProfilerMarker _PRF_SetCurrentTarget =
            new ProfilerMarker(_PRF_PFX + nameof(SetCurrentTarget));

        private static readonly ProfilerMarker _PRF_VisualizeRectangles =
            new ProfilerMarker(_PRF_PFX + nameof(VisualizeRectangles));

        #endregion
    }
}
