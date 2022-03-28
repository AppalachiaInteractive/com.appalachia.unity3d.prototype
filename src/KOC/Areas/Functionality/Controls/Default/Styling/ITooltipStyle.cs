using Appalachia.UI.Core.Layout;
using Appalachia.UI.Styling.Elements;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.Functionality.Controls.Tooltips.Styling
{
    public interface ITooltipStyle : IStyleElement
    {
        AppearanceDirection Direction { get; }
        bool ShowTriangle { get; }
        Color BackgroundColor { get; }
        Color OutlineColor { get; }
        float DistanceFromTarget { get; }
        float TriangleSize { get; }
        float OutlineThickness { get; }
        float TextPadding { get; }
        Sprite TriangleSprite { get; }

        #region Profiling

        private const string _PRF_PFX = nameof(ITooltipStyle) + ".";

        #endregion
    }
}
