using System;
using Appalachia.Core.Overrides.Implementations;
using Appalachia.Prototype.KOC.Application.Styling.Base;
using Appalachia.Prototype.KOC.Application.Styling.Overrides;
using TMPro;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Styling.Fonts
{
    [Serializable]
    public class
        FontStyleOverride : ApplicationStyleElementOverride<FontStyle, FontStyleOverride, IFontStyle>,
                            IFontStyle
    {
        #region Fields and Autoproperties

        [SerializeField] private OverridableTMP_FontAsset _font;
        [SerializeField] private OverridableInt _fontSize;
        [SerializeField] private OverridableFontWeight _fontWeight;
        [SerializeField] private OverridableTextAlignmentOptions _alignment;
        [SerializeField] private OverridableHorizontalAlignmentOptions _horizontalAlignment;
        [SerializeField] private OverridableVerticalAlignmentOptions _verticalAlignment;
        [SerializeField] private OverridableColor _color;

        #endregion

        public override void SyncWithDefault()
        {
            using (_PRF_SyncWithDefault.Auto())
            {
                if (!_font.overrideEnabled)
                {
                    _font.value = Defaults.Font;
                }

                if (!_fontSize.overrideEnabled)
                {
                    _fontSize.value = Defaults.FontSize;
                }

                if (!_fontWeight.overrideEnabled)
                {
                    _fontWeight.value = Defaults.FontWeight;
                }

                if (!_alignment.overrideEnabled)
                {
                    _alignment.value = Defaults.Alignment;
                }

                if (!_horizontalAlignment.overrideEnabled)
                {
                    _horizontalAlignment.value = Defaults.HorizontalAlignment;
                }

                if (!_verticalAlignment.overrideEnabled)
                {
                    _verticalAlignment.value = Defaults.VerticalAlignment;
                }
            }
        }

        #region IFontStyle Members

        public Color Color => _color.Get(Defaults.Color);

        public FontWeight FontWeight => _fontWeight.Get(Defaults.FontWeight);

        public HorizontalAlignmentOptions HorizontalAlignment =>
            _horizontalAlignment.Get(Defaults.HorizontalAlignment);

        public int FontSize => _fontSize.Get(Defaults.FontSize);

        public TextAlignmentOptions Alignment => _alignment.Get(Defaults.Alignment);

        public TMP_FontAsset Font => _font.Get(Defaults.Font);

        public VerticalAlignmentOptions VerticalAlignment =>
            _verticalAlignment.Get(Defaults.VerticalAlignment);

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(FontStyleOverride) + ".";

        private static readonly ProfilerMarker _PRF_SyncWithDefault =
            new ProfilerMarker(_PRF_PFX + nameof(SyncWithDefault));

        #endregion
    }
}
