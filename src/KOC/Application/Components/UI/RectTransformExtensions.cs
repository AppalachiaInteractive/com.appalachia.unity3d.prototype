using System;
using Appalachia.Utility.Enums;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components.UI
{
    [Flags]
    public enum RectResetOptions
    {
        None = 0,
        Position = 1 << 0,
        LocalPosition = 1 << 1,
        AnchoredPosition = 1 << 2,
        AnchoredPosition3D = 1 << 3,
        SizeDelta = 1 << 4,
        AnchorMin = 1 << 5,
        AnchorMax = 1 << 6,
        OffsetMin = 1 << 7,
        OffsetMax = 1 << 18,
        Pivot = 1 << 9,
        Scale = 1 << 10,
        Rotation = 1 << 11,

        Anchors = AnchorMin | AnchorMax,
        Offsets = OffsetMin | OffsetMax,
        Positions = Position | LocalPosition | AnchoredPosition | AnchoredPosition3D,
        Transforms = Positions | Scale | Rotation,
        
        All = ~0 
    }

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
