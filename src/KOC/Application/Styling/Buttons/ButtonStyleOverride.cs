using System;
using Appalachia.Core.Overrides.Implementations;
using Appalachia.Prototype.KOC.Application.Styling.Base;
using Appalachia.Prototype.KOC.Application.Styling.Overrides;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Styling.Buttons
{
    [Serializable]
    public class ButtonStyleOverride :
        ApplicationStyleElementOverride<ButtonStyle, ButtonStyleOverride, IButtonStyle>,
        IButtonStyle
    {
        #region Fields and Autoproperties

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableTextAlignmentOptions _alignment;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableFloat _fontSize;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableColor _normalColor;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableColor _highlightedColor;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableColor _pressedColor;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableColor _selectedColor;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableColor _disabledColor;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableFloat _colorMultiplier;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OverridableFloat _fadeDuration;

        #endregion

        public override void SyncWithDefault()
        {
            using (_PRF_SyncWithDefault.Auto())
            {
                if (!_alignment.overrideEnabled)
                {
                    _alignment.value = Defaults.Alignment;
                }

                if (!_fontSize.overrideEnabled)
                {
                    _fontSize.value = Defaults.FontSize;
                }

                if (!_normalColor.overrideEnabled)
                {
                    _normalColor.value = Defaults.NormalColor;
                }

                if (!_highlightedColor.overrideEnabled)
                {
                    _highlightedColor.value = Defaults.HighlightedColor;
                }

                if (!_pressedColor.overrideEnabled)
                {
                    _pressedColor.value = Defaults.PressedColor;
                }

                if (!_selectedColor.overrideEnabled)
                {
                    _selectedColor.value = Defaults.SelectedColor;
                }

                if (!_disabledColor.overrideEnabled)
                {
                    _disabledColor.value = Defaults.DisabledColor;
                }

                if (!_colorMultiplier.overrideEnabled)
                {
                    _colorMultiplier.value = Defaults.ColorMultiplier;
                }

                if (!_fadeDuration.overrideEnabled)
                {
                    _fadeDuration.value = Defaults.FadeDuration;
                }
            }
        }

        #region IButtonStyle Members

        public TextAlignmentOptions Alignment => _alignment.Get(Defaults.Alignment);
        public float FontSize => _fontSize.Get(Defaults.FontSize);
        public Color NormalColor => _normalColor.Get(Defaults.NormalColor);
        public Color HighlightedColor => _highlightedColor.Get(Defaults.HighlightedColor);
        public Color PressedColor => _pressedColor.Get(Defaults.PressedColor);
        public Color SelectedColor => _selectedColor.Get(Defaults.SelectedColor);
        public Color DisabledColor => _disabledColor.Get(Defaults.DisabledColor);
        public float ColorMultiplier => _colorMultiplier.Get(Defaults.ColorMultiplier);
        public float FadeDuration => _fadeDuration.Get(Defaults.FadeDuration);

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(ButtonStyleOverride) + ".";

        private static readonly ProfilerMarker _PRF_SyncWithDefault =
            new ProfilerMarker(_PRF_PFX + nameof(SyncWithDefault));

        #endregion
    }
}
