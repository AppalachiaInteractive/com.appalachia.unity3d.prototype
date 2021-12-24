using Appalachia.Prototype.KOC.Application.Styling.Base;
using TMPro;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Styling.Fonts
{
    public interface IFontStyle : IApplicationStyle
    {
        public bool AutoSize { get; }
        public Color Color { get; }
        public FontWeight FontWeight { get; }
        public HorizontalAlignmentOptions HorizontalAlignment { get; }
        public int FontSize { get; }
        public TextAlignmentOptions Alignment { get; }

        public TMP_FontAsset Font { get; }

        public Vector2Int FontRange { get; }
        public VerticalAlignmentOptions VerticalAlignment { get; }

        public bool EnableWordWrapping { get; }

        public TextOverflowModes OverflowMode { get; }

        public void Apply(TextMeshPro component)
        {
            using (_PRF_Apply.Auto())
            {
                component.font = Font;
                component.fontSize = FontSize;
                component.fontWeight = FontWeight;
                component.alignment = Alignment;
                component.horizontalAlignment = HorizontalAlignment;
                component.verticalAlignment = VerticalAlignment;
                component.enableAutoSizing = AutoSize;
                component.fontSizeMin = FontRange.x;
                component.fontSizeMax = FontRange.y;
                component.overflowMode = OverflowMode;
                component.enableWordWrapping = EnableWordWrapping;
            }
        }

        public void Apply(TextMeshProUGUI component)
        {
            using (_PRF_Apply.Auto())
            {
                component.font = Font;
                component.fontSize = FontSize;
                component.fontWeight = FontWeight;
                component.alignment = Alignment;
                component.horizontalAlignment = HorizontalAlignment;
                component.verticalAlignment = VerticalAlignment;
                component.enableAutoSizing = AutoSize;
                component.fontSizeMin = FontRange.x;
                component.fontSizeMax = FontRange.y;
                component.overflowMode = OverflowMode;
                component.enableWordWrapping = EnableWordWrapping;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(IFontStyle) + ".";

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion
    }
}
