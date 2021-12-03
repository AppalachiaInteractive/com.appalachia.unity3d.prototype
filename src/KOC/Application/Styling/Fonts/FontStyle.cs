using System;
using Appalachia.Prototype.KOC.Application.Styling.Base;
using TMPro;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Styling.Fonts
{
    [Serializable]
    public class FontStyle : ApplicationStyleElementDefault<FontStyle, FontStyleOverride, IFontStyle>,
                             IFontStyle
    {
        #region Fields and Autoproperties

        [SerializeField] private TMP_FontAsset _font;
        [SerializeField] private int _fontSize;
        [SerializeField] private FontWeight _fontWeight;
        [SerializeField] private TextAlignmentOptions _alignment;
        [SerializeField] private HorizontalAlignmentOptions _horizontalAlignment;
        [SerializeField] private VerticalAlignmentOptions _verticalAlignment;
        [SerializeField] private Color _color = Color.white;

        #endregion

        #region IFontStyle Members

        public Color Color => _color;
        public FontWeight FontWeight => _fontWeight;
        public HorizontalAlignmentOptions HorizontalAlignment => _horizontalAlignment;
        public int FontSize => _fontSize;
        public TextAlignmentOptions Alignment => _alignment;
        public TMP_FontAsset Font => _font;
        public VerticalAlignmentOptions VerticalAlignment => _verticalAlignment;

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(FontStyle) + ".";

        private static readonly ProfilerMarker _PRF_RegisterOverride =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterOverride));

        #endregion
    }
}
