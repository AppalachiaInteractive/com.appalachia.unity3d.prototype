using System;

namespace Appalachia.Prototype.KOC.Extensions
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
}
