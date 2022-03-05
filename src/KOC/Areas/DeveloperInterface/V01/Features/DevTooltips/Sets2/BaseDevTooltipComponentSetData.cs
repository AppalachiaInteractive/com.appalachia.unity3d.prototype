using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets;
using Appalachia.UI.Core.Components.Data;
using Appalachia.UI.Core.Components.Sets2;
using Appalachia.UI.Core.Components.Subsets;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets2
{
    /// <summary>
    ///     Defines the metadata necessary for configuring a
    ///     <see cref="BaseDevTooltipComponentSet{TComponentSet,TComponentSetData,TIComponentSetData}" />.
    /// </summary>
    /// <typeparam name="TComponentSet">The component set.</typeparam>
    /// <typeparam name="TComponentSetData">Styling metadata for the set.</typeparam>
    /// <typeparam name="TIComponentSetData">The interface that the style implements.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class
        BaseDevTooltipComponentSetData<TComponentSet, TComponentSetData, TIComponentSetData> :
            UIComponentSetData<TComponentSet, TComponentSetData>,
            IDevTooltipComponentSetData
        where TComponentSet : BaseDevTooltipComponentSet<TComponentSet, TComponentSetData, TIComponentSetData>, new()
        where TComponentSetData : BaseDevTooltipComponentSetData<TComponentSet, TComponentSetData, TIComponentSetData>,
        TIComponentSetData, new()
        where TIComponentSetData : IDevTooltipComponentSetData
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
        private OutlinedImageSubsetData _background;

        [PropertyOrder(ORDER_TOOLTIP + 19)]
        [SerializeField]
        private RectTransformData _triangleParent;

        [PropertyOrder(ORDER_TOOLTIP + 20)]
        [SerializeField]
        private ImageSubsetData _triangleBackground;

        [PropertyOrder(ORDER_TOOLTIP + 30)]
        [SerializeField]
        private ImageSubsetData _triangleForeground;

        [PropertyOrder(ORDER_TOOLTIP + 40)]
        [SerializeField]
        private TextMeshProUGUISubsetData _tooltipText;

        [PropertyOrder(ORDER_TOOLTIP + 41)]
        [SerializeField]
        private CanvasGroupData _canvasGroup;

        #endregion

        public CanvasGroupData CanvasGroup
        {
            get => _canvasGroup;
            protected set => _canvasGroup = value;
        }

        public RectTransformData TriangleParent
        {
            get => _triangleParent;
            protected set => _triangleParent = value;
        }

        /// <inheritdoc />
        public override void ApplyToComponentSet(TComponentSet componentSet)
        {
            using (_PRF_ApplyToComponentSet.Auto())
            {
                base.ApplyToComponentSet(componentSet);

                OutlinedImageSubsetData.RefreshAndUpdate(
                    ref _background,
                    Owner,
                    ref componentSet.background,
                    componentSet.GameObject,
                    BACKGROUND_OBJECT_NAME
                );

                componentSet.GameObject.GetOrAddChild(
                    ref componentSet.triangleParentObject,
                    TRIANGLE_OBJECT_NAME,
                    true
                );

                componentSet.triangleParent = componentSet.triangleParentObject.transform as RectTransform;

                RectTransformData.RefreshAndUpdate(ref _triangleParent, Owner, componentSet.triangleParent);

                ImageSubsetData.RefreshAndUpdate(
                    ref _triangleBackground,
                    Owner,
                    ref componentSet.triangleBackground,
                    componentSet.triangleParentObject,
                    TRIANGLE_BACKGROUND_OBJECT_NAME
                );

                ImageSubsetData.RefreshAndUpdate(
                    ref _triangleForeground,
                    Owner,
                    ref componentSet.triangleForeground,
                    componentSet.triangleParentObject,
                    TRIANGLE_FOREGROUND_OBJECT_NAME
                );

                TextMeshProUGUISubsetData.RefreshAndUpdate(
                    ref _tooltipText,
                    Owner,
                    ref componentSet.tooltipText,
                    componentSet.GameObject,
                    TOOLTIP_TEXT_OBJECT_NAME
                );

                CanvasGroupData.RefreshAndUpdate(ref _canvasGroup, Owner, componentSet.canvasGroup);

                RectTransform.disableUpdates = false;
                _background.RectTransform.disableUpdates = false;
                _triangleBackground.RectTransform.disableUpdates = false;
                _triangleForeground.RectTransform.disableUpdates = false;
                _tooltipText.RectTransform.disableUpdates = false;

                RectTransform.hideInInspector = true;
                _background.RectTransform.hideInInspector = true;
                _triangleBackground.RectTransform.hideInInspector = true;
                _triangleForeground.RectTransform.hideInInspector = true;
                _tooltipText.RectTransform.hideInInspector = true;
            }
        }

        #region IDevTooltipComponentSetData Members

        public OutlinedImageSubsetData Background
        {
            get => _background;
            protected set => _background = value;
        }

        public ImageSubsetData TriangleBackground
        {
            get => _triangleBackground;
            protected set => _triangleBackground = value;
        }

        public ImageSubsetData TriangleForeground
        {
            get => _triangleForeground;
            protected set => _triangleForeground = value;
        }

        public TextMeshProUGUISubsetData TooltipText
        {
            get => _tooltipText;
            protected set => _tooltipText = value;
        }

        #endregion
    }
}
