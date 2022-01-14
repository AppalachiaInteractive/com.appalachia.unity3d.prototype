using Appalachia.Utility.Enums;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Extensions
{
    public static class RectTransformExtensions
    {
        public static void Reset(this RectTransform rectTransform, RectResetOptions options)
        {
            if (options.Has(RectResetOptions.Position))
            {
                rectTransform.position = Vector3.zero;
            }

            if (options.Has(RectResetOptions.LocalPosition))
            {
                rectTransform.localPosition = Vector3.zero;
            }

            if (options.Has(RectResetOptions.AnchoredPosition))
            {
                rectTransform.anchoredPosition = Vector2.zero;
            }

            if (options.Has(RectResetOptions.AnchoredPosition3D))
            {
                rectTransform.anchoredPosition3D = Vector3.zero;
            }

            if (options.Has(RectResetOptions.SizeDelta))
            {
                rectTransform.sizeDelta = Vector2.zero;
            }

            if (options.Has(RectResetOptions.AnchorMin))
            {
                rectTransform.anchorMin = Vector2.zero;
            }

            if (options.Has(RectResetOptions.AnchorMax))
            {
                rectTransform.anchorMax = Vector2.one;
            }

            if (options.Has(RectResetOptions.OffsetMin))
            {
                rectTransform.offsetMin = Vector2.zero;
            }

            if (options.Has(RectResetOptions.OffsetMax))
            {
                rectTransform.offsetMax = Vector2.zero;
            }

            if (options.Has(RectResetOptions.Pivot))
            {
                rectTransform.pivot = Vector2.one * .5f;
            }

            if (options.Has(RectResetOptions.Scale))
            {
                rectTransform.localScale = Vector3.one;
            }

            if (options.Has(RectResetOptions.Rotation))
            {
                rectTransform.rotation = Quaternion.identity;
            }
        }
    }
}
