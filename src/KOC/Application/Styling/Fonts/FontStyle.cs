using System;
using Appalachia.Prototype.KOC.Application.Styling.Base;
using Sirenix.OdinInspector;
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

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private TMP_FontAsset _font;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private bool _autoSize;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        [DisableIf(nameof(_autoSize))]
        [PropertyRange(8, 256)]
        private int _fontSize;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        [EnableIf(nameof(_autoSize))]
        [MinMaxSlider(8, 256, true)]
        private Vector2Int _fontRange;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private FontWeight _fontWeight;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private TextAlignmentOptions _alignment;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private HorizontalAlignmentOptions _horizontalAlignment;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private VerticalAlignmentOptions _verticalAlignment;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private Color _color = Color.white;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private bool _enableWordWrapping;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private TextOverflowModes _overflowMode; 
        
        #endregion

        #region IFontStyle Members

        public Color Color => _color;
        public FontWeight FontWeight => _fontWeight;
        public HorizontalAlignmentOptions HorizontalAlignment => _horizontalAlignment;

        public bool AutoSize => _autoSize;
        public int FontSize => _fontSize;

        public Vector2Int FontRange => _fontRange;
        
        public TextAlignmentOptions Alignment => _alignment;
        public TMP_FontAsset Font => _font;
        public VerticalAlignmentOptions VerticalAlignment => _verticalAlignment;

        public bool EnableWordWrapping => _enableWordWrapping;

        public TextOverflowModes OverflowMode => _overflowMode;
        
        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(FontStyle) + ".";

        private static readonly ProfilerMarker _PRF_RegisterOverride =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterOverride));

        #endregion
    }
}
