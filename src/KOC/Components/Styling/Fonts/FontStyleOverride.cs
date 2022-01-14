using System;
using Appalachia.Core.Overrides.Implementations;
using Appalachia.Prototype.KOC.Components.Styling.Base;
using Appalachia.Prototype.KOC.Components.Styling.Overrides;
using TMPro;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.Styling.Fonts
{
    [Serializable]
    public class
        FontStyleOverride : ApplicationStyleElementOverride<FontStyle, FontStyleOverride, IFontStyle>,
                            IFontStyle
    {
        #region Fields and Autoproperties

        [SerializeField] private OverridableTMP_FontAsset _font;

        [SerializeField] private OverridableInt _fontSize;

        [SerializeField] private OverridableBool _autoSize;

        [SerializeField] private OverridableVector2Int _fontRange;

        [SerializeField] private OverridableFontWeight _fontWeight;

        [SerializeField] private OverridableTextAlignmentOptions _alignment;

        [SerializeField] private OverridableHorizontalAlignmentOptions _horizontalAlignment;

        [SerializeField] private OverridableVerticalAlignmentOptions _verticalAlignment;

        [SerializeField] private OverridableColor _color;

        [SerializeField] private OverridableBool _enableWordWrapping;

        [SerializeField] private OverridableTextOverflowModes _overflowMode;

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

        protected override void RegisterOverrideSubscriptions()
        {
            using (_PRF_RegisterOverrideSubscriptions.Auto())
            {
                _font.OverridableChanged += _ => InvokeStyleChanged();
                _fontSize.OverridableChanged += _ => InvokeStyleChanged();
                _autoSize.OverridableChanged += _ => InvokeStyleChanged();
                _fontRange.OverridableChanged += _ => InvokeStyleChanged();
                _fontWeight.OverridableChanged += _ => InvokeStyleChanged();
                _alignment.OverridableChanged += _ => InvokeStyleChanged();
                _horizontalAlignment.OverridableChanged += _ => InvokeStyleChanged();
                _verticalAlignment.OverridableChanged += _ => InvokeStyleChanged();
                _color.OverridableChanged += _ => InvokeStyleChanged();
                _enableWordWrapping.OverridableChanged += _ => InvokeStyleChanged();
                _overflowMode.OverridableChanged += _ => InvokeStyleChanged();
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

        private static readonly ProfilerMarker _PRF_RegisterOverrideSubscriptions =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterOverrideSubscriptions));

        private static readonly ProfilerMarker _PRF_SyncWithDefault =
            new ProfilerMarker(_PRF_PFX + nameof(SyncWithDefault));

        #endregion
    }
}
