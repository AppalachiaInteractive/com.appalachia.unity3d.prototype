using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.UI.Core.Components.Sets;
using Appalachia.UI.Core.Components.Subsets;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Sets
{
    /// <summary>
    ///     Defines the members necessary for creating and configuring
    ///     the components of a Tooltip component set.
    /// </summary>
    /// <typeparam name="TComponentSet">The component set.</typeparam>
    /// <typeparam name="TComponentSetData">Styling metadata for the set.</typeparam>
    /// <typeparam name="TIComponentSetData">The interface that the style implements.</typeparam>
    [Serializable]
    [SmartLabelChildren]
    public abstract class
        BaseDevTooltipComponentSet<TComponentSet, TComponentSetData, TIComponentSetData> :
            UIComponentSet<TComponentSet, TComponentSetData>,
            IDevTooltipComponentSet
        where TComponentSet : BaseDevTooltipComponentSet<TComponentSet, TComponentSetData, TIComponentSetData>, new()
        where TComponentSetData : BaseDevTooltipComponentSetData<TComponentSet, TComponentSetData, TIComponentSetData>,
        TIComponentSetData, new()
        where TIComponentSetData : IDevTooltipComponentSetData
    {
        #region Constants and Static Readonly

        protected const int ORDER_TOOLTIP = ORDER_RECT + 50;

        #endregion

        #region Fields and Autoproperties

        [PropertyOrder(ORDER_TOOLTIP + 10)]
        [SerializeField]
        public OutlinedImageSubset background;

        [PropertyOrder(ORDER_TOOLTIP + 18)]
        [SerializeField]
        public GameObject triangleParentObject;

        [PropertyOrder(ORDER_TOOLTIP + 19)]
        [SerializeField]
        public RectTransform triangleParent;

        [PropertyOrder(ORDER_TOOLTIP + 20)]
        [SerializeField]
        public ImageSubset triangleBackground;

        [PropertyOrder(ORDER_TOOLTIP + 30)]
        [SerializeField]
        public ImageSubset triangleForeground;

        [PropertyOrder(ORDER_TOOLTIP + 40)]
        [SerializeField]
        public TextMeshProUGUISubset tooltipText;

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
                base.DestroySafely();

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

        /// <param name="data"></param>
        /// <inheritdoc />
        public override void Enable(TComponentSetData data)
        {
            using (_PRF_Enable.Auto())
            {
                base.Enable(data);

                background?.Enable(data.Background);
                triangleBackground?.Enable(data.TriangleBackground);
                triangleForeground?.Enable(data.TriangleForeground);
                tooltipText?.Enable(data.TooltipText);
                canvasGroup.enabled = true;
            }
        }

        /// <inheritdoc />
        protected override void OnGetOrAddComponents()
        {
            using (_PRF_OnGetOrAddComponents.Auto())
            {
                base.OnGetOrAddComponents();

                GameObject.GetOrAddComponent(ref canvasGroup);
            }
        }

        #region IDevTooltipComponentSet Members

        public ImageSubset TriangleBackground => triangleBackground;
        public ImageSubset TriangleForeground => triangleForeground;
        public OutlinedImageSubset Background => background;
        public TextMeshProUGUISubset TooltipText => tooltipText;

        #endregion
    }
}
