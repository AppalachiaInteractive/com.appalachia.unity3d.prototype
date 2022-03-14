using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.UI.ControlModel.Controls;
using Appalachia.UI.Functionality.Images.Groups.Default;
using Appalachia.UI.Functionality.Images.Groups.Outline;
using Appalachia.UI.Functionality.Text.Groups;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets
{
    /// <summary>
    ///     Defines the members necessary for creating and configuring
    ///     the components of a Tooltip control.
    /// </summary>
    /// <typeparam name="TControl">The control.</typeparam>
    /// <typeparam name="TConfig">Configuration for the control.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class BaseDevTooltipControl<TControl, TConfig> : AppaUIControl<TControl, TConfig>,
                                                                     IDevTooltipControl
        where TControl : BaseDevTooltipControl<TControl, TConfig>, new()
        where TConfig : BaseDevTooltipControlConfig<TControl, TConfig>, new()

    {
        #region Constants and Static Readonly

        protected const int ORDER_TOOLTIP = ORDER_ROOT + 50;

        #endregion

        #region Fields and Autoproperties

        [PropertyOrder(ORDER_TOOLTIP + 10)]
        [SerializeField]
        public OutlineImageComponentGroup background;

        [PropertyOrder(ORDER_TOOLTIP + 18)]
        [SerializeField]
        public GameObject triangleParentObject;

        [PropertyOrder(ORDER_TOOLTIP + 19)]
        [SerializeField]
        public RectTransform triangleParent;

        [PropertyOrder(ORDER_TOOLTIP + 20)]
        [SerializeField]
        public ImageComponentGroup triangleBackground;

        [PropertyOrder(ORDER_TOOLTIP + 30)]
        [SerializeField]
        public ImageComponentGroup triangleForeground;

        [PropertyOrder(ORDER_TOOLTIP + 40)]
        [SerializeField]
        public TextMeshProUGUIComponentGroup tooltipText;

        [PropertyOrder(ORDER_TOOLTIP + 41)]
        [SerializeField]
        public CanvasGroup canvasGroup;

        #endregion

        public CanvasGroup CanvasGroup => canvasGroup;

        /// <inheritdoc />
        public override void DestroySafely()
        {
            using (_PRF_DestroySafely.Auto())
            {
                background?.DestroySafely();
                triangleBackground?.DestroySafely();
                triangleForeground?.DestroySafely();
                tooltipText?.DestroySafely();
                canvasGroup.DestroySafely();
            }
        }

        /// <inheritdoc />
        public override void Disable()
        {
            using (_PRF_Disable.Auto())
            {
                base.Disable();

                background?.Disable();
                triangleBackground?.Disable();
                triangleForeground?.Disable();
                tooltipText?.Disable();
                canvasGroup.enabled = false;
            }
        }

        /// <inheritdoc />
        public override void Enable(TConfig config)
        {
            using (_PRF_Enable.Auto())
            {
                base.Enable(config);

                background?.Enable(config.Background);
                triangleBackground?.Enable(config.TriangleBackground);
                triangleForeground?.Enable(config.TriangleForeground);
                tooltipText?.Enable(config.TooltipText);
                canvasGroup.enabled = true;
            }
        }

        #region IDevTooltipControl Members

        /// <inheritdoc />
        public override void Refresh()
        {
            using (_PRF_Refresh.Auto())
            {
                base.Refresh();

                GameObject.GetOrAddComponent(ref canvasGroup);
            }
        }

        public ImageComponentGroup TriangleBackground => triangleBackground;
        public ImageComponentGroup TriangleForeground => triangleForeground;
        public OutlineImageComponentGroup Background => background;
        public TextMeshProUGUIComponentGroup TooltipText => tooltipText;

        #endregion
    }
}
