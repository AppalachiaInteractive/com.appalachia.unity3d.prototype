using System;
using Appalachia.Core.Overrides.Implementations;
using Appalachia.Prototype.KOC.Application.Styling.Base;
using Appalachia.Prototype.KOC.Application.Styling.Overrides;
using Sirenix.OdinInspector;
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

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableTMP_FontAsset _font;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableInt _fontSize;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableBool _autoSize;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableVector2Int _fontRange;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableFontWeight _fontWeight;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableTextAlignmentOptions _alignment;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableHorizontalAlignmentOptions _horizontalAlignment;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableVerticalAlignmentOptions _verticalAlignment;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableColor _color;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableBool _enableWordWrapping;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableTextOverflowModes _overflowMode;

        #endregion

        public override void SyncWithDefault()
        {
            using (_PRF_SyncWithDefault.Auto())
            {
                if (!_font.overrideEnabled)
                {
                    _font.value = Defaults.Font;
                }

                if (!_autoSize.overrideEnabled)
                {
                    _autoSize.value = Defaults.AutoSize;
                }

                if (!_fontRange.overrideEnabled)
                {
                    _fontRange.value = Defaults.FontRange;
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

                if (!_enableWordWrapping.overrideEnabled)
                {
                    _enableWordWrapping.value = Defaults.EnableWordWrapping;
                }

                if (!_overflowMode.overrideEnabled)
                {
                    _overflowMode.value = Defaults.OverflowMode;
                }
            }
        }

        #region IFontStyle Members

        public bool AutoSize => _autoSize.Get(Defaults.AutoSize);

        public Color Color => _color.Get(Defaults.Color);

        public FontWeight FontWeight => _fontWeight.Get(Defaults.FontWeight);

        public HorizontalAlignmentOptions HorizontalAlignment =>
            _horizontalAlignment.Get(Defaults.HorizontalAlignment);

        public int FontSize => _fontSize.Get(Defaults.FontSize);

        public TextAlignmentOptions Alignment => _alignment.Get(Defaults.Alignment);

        public TMP_FontAsset Font => _font.Get(Defaults.Font);
        public Vector2Int FontRange => _fontRange.Get(Defaults.FontRange);

        public VerticalAlignmentOptions VerticalAlignment =>
            _verticalAlignment.Get(Defaults.VerticalAlignment);

        public bool EnableWordWrapping => _enableWordWrapping.Get(Defaults.EnableWordWrapping);

        public TextOverflowModes OverflowMode => _overflowMode.Get(Defaults.OverflowMode);

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(FontStyleOverride) + ".";

        private static readonly ProfilerMarker _PRF_SyncWithDefault =
            new ProfilerMarker(_PRF_PFX + nameof(SyncWithDefault));

        #endregion
    }
}
