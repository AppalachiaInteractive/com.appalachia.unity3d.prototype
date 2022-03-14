using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.UI.ControlModel.Components;
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
    ///     Defines the metadata necessary for configuring a
    ///     <see cref="BaseDevTooltipControl{TControl,TConfig}" />.
    /// </summary>
    /// <typeparam name="TControl">The control.</typeparam>
    /// <typeparam name="TConfig">Configuration for the control.</typeparam>
    
    [Serializable]
    [SmartLabelChildren]
    public abstract class
        BaseDevTooltipControlConfig<TControl, TConfig> : AppaUIControlConfig<TControl, TConfig>,
            IDevTooltipControlConfig
        where TControl : BaseDevTooltipControl<TControl, TConfig>, new()
        where TConfig: BaseDevTooltipControlConfig<TControl, TConfig>,
         new()
        
    {
        #region Constants and Static Readonly

        protected const int ORDER_TOOLTIP = ORDER_RECT + 50;
        private const string BACKGROUND_OBJECT_NAME = "Background";
        private const string TOOLTIP_TEXT_OBJECT_NAME = "Tooltip Text";
        private const string TRIANGLE_BACKGROUND_OBJECT_NAME = "Triangle Background";
        private const string TRIANGLE_FOREGROUND_OBJECT_NAME = "Triangle Foreground";
        private const string TRIANGLE_OBJECT_NAME = "Triangle";

        #endregion

        #region Fields and Autoproperties

        [PropertyOrder(ORDER_TOOLTIP + 10)]
        [SerializeField]
        private OutlineImageComponentGroupConfig _background;

        [PropertyOrder(ORDER_TOOLTIP + 19)]
        [SerializeField]
        private RectTransformConfig _triangleParent;

        [PropertyOrder(ORDER_TOOLTIP + 20)]
        [SerializeField]
        private ImageComponentGroupConfig _triangleBackground;

        [PropertyOrder(ORDER_TOOLTIP + 30)]
        [SerializeField]
        private ImageComponentGroupConfig _triangleForeground;

        [PropertyOrder(ORDER_TOOLTIP + 40)]
        [SerializeField]
        private TextMeshProUGUIComponentGroupConfig _tooltipText;

        [PropertyOrder(ORDER_TOOLTIP + 41)]
        [SerializeField]
        private CanvasGroupConfig _canvasGroup;

        #endregion

        public CanvasGroupConfig CanvasGroup
        {
            get => _canvasGroup;
            protected set => _canvasGroup = value;
        }

        public RectTransformConfig TriangleParent
        {
            get => _triangleParent;
            protected set => _triangleParent = value;
        }

        /// <inheritdoc />
        protected override void OnApply(TControl control)
        {
            using (_PRF_OnApply.Auto())
            {
                base.OnApply(control);

                OutlineImageComponentGroupConfig.RefreshAndApply(
                    ref _background,
                    Owner,
                    ref control.background,
                    control.GameObject,
                    BACKGROUND_OBJECT_NAME
                );

                control.GameObject.GetOrAddChild(
                    ref control.triangleParentObject,
                    TRIANGLE_OBJECT_NAME,
                    true
                );

                control.triangleParent = control.triangleParentObject.transform as RectTransform;

                RectTransformConfig.RefreshAndApply(ref _triangleParent, Owner, control.triangleParent);

                ImageComponentGroupConfig.RefreshAndApply(
                    ref _triangleBackground,
                    Owner,
                    ref control.triangleBackground,
                    control.triangleParentObject,
                    TRIANGLE_BACKGROUND_OBJECT_NAME
                );

                ImageComponentGroupConfig.RefreshAndApply(
                    ref _triangleForeground,
                    Owner,
                    ref control.triangleForeground,
                    control.triangleParentObject,
                    TRIANGLE_FOREGROUND_OBJECT_NAME
                );

                TextMeshProUGUIComponentGroupConfig.RefreshAndApply(
                    ref _tooltipText,
                    Owner,
                    ref control.tooltipText,
                    control.GameObject,
                    TOOLTIP_TEXT_OBJECT_NAME
                );

                CanvasGroupConfig.RefreshAndApply(ref _canvasGroup, Owner, control.canvasGroup);

                RectTransform.DisableAllFields = false;
                _background.RectTransform.DisableAllFields = false;
                _triangleBackground.RectTransform.DisableAllFields = false;
                _triangleForeground.RectTransform.DisableAllFields = false;
                _tooltipText.RectTransform.DisableAllFields = false;

                RectTransform.HideAllFields = true;
                _background.RectTransform.HideAllFields = true;
                _triangleBackground.RectTransform.HideAllFields = true;
                _triangleForeground.RectTransform.HideAllFields = true;
                _tooltipText.RectTransform.HideAllFields = true;
            }
        }

        #region IDevTooltipControlConfig Members

        public OutlineImageComponentGroupConfig Background
        {
            get => _background;
            protected set => _background = value;
        }

        public ImageComponentGroupConfig TriangleBackground
        {
            get => _triangleBackground;
            protected set => _triangleBackground = value;
        }

        public ImageComponentGroupConfig TriangleForeground
        {
            get => _triangleForeground;
            protected set => _triangleForeground = value;
        }

        public TextMeshProUGUIComponentGroupConfig TooltipText
        {
            get => _tooltipText;
            protected set => _tooltipText = value;
        }

        #endregion
    }
}
