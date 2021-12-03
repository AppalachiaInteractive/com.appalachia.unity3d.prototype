using System;
using Appalachia.Core.Overrides.Implementations;
using Appalachia.Prototype.KOC.Application.Styling.Base;
using Appalachia.Prototype.KOC.Application.Styling.Overrides;
using TMPro;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Styling.Buttons
{
    public interface IButtonStyle : IApplicationStyle
    {
        Color DisabledColor { get; }
        Color HighlightedColor { get; }
        Color NormalColor { get; }
        Color PressedColor { get; }
        Color SelectedColor { get; }
        float ColorMultiplier { get; }
        float FadeDuration { get; }
        float FontSize { get; }
        TextAlignmentOptions Alignment { get; }

        public void Apply(Button component)
        {
            using (_PRF_Apply.Auto())
            {
            }
        }

        public void Apply(TextMeshProUGUI component)
        {
            using (_PRF_Apply.Auto())
            {
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(IButtonStyle) + ".";

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion
    }

    [Serializable]
    public class ButtonStyle : ApplicationStyleElementDefault<ButtonStyle, ButtonStyleOverride, IButtonStyle>,
                               IButtonStyle
    {
        #region Fields and Autoproperties

        private TextAlignmentOptions _alignment;
        private float _fontSize;
        private Color _normalColor;
        private Color _highlightedColor;
        private Color _pressedColor;
        private Color _selectedColor;
        private Color _disabledColor;
        private float _colorMultiplier;
        private float _fadeDuration;

        #endregion

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                initializer.Initialize(
                    this,
                    nameof(_normalColor),
                    () => _normalColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1.0f)
                );
                initializer.Initialize(
                    this,
                    nameof(_highlightedColor),
                    () => _highlightedColor = new Color(245f / 255f, 245f / 255f, 245f / 255f, 1.0f)
                );
                initializer.Initialize(
                    this,
                    nameof(_pressedColor),
                    () => _pressedColor = new Color(200f / 255f, 200f / 255f, 200f / 255f, 1.0f)
                );
                initializer.Initialize(
                    this,
                    nameof(_selectedColor),
                    () => _selectedColor = new Color(245f / 255f, 245f / 255f, 245f / 255f, 1.0f)
                );
                initializer.Initialize(
                    this,
                    nameof(_disabledColor),
                    () => _disabledColor = new Color(200f / 255f, 200f / 255f, 200f / 255f, 0.5f)
                );

                initializer.Initialize(this, nameof(_colorMultiplier), () => _colorMultiplier = 1.0f);
                initializer.Initialize(this, nameof(_fadeDuration),    () => _fadeDuration = .1f);
                initializer.Initialize(this, nameof(_fontSize),        () => _fontSize = 14f);
                initializer.Initialize(
                    this,
                    nameof(_alignment),
                    () => _alignment = TextAlignmentOptions.Center
                );
            }
        }

        #region IButtonStyle Members

        public TextAlignmentOptions Alignment => _alignment;
        public float FontSize => _fontSize;
        public Color NormalColor => _normalColor;
        public Color HighlightedColor => _highlightedColor;
        public Color PressedColor => _pressedColor;
        public Color SelectedColor => _selectedColor;
        public Color DisabledColor => _disabledColor;
        public float ColorMultiplier => _colorMultiplier;
        public float FadeDuration => _fadeDuration;

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(ButtonStyle) + ".";

        private static readonly ProfilerMarker _PRF_RegisterOverride =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterOverride));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }

    [Serializable]
    public class ButtonStyleOverride :
        ApplicationStyleElementOverride<ButtonStyle, ButtonStyleOverride, IButtonStyle>,
        IButtonStyle
    {
        #region Fields and Autoproperties

        private OverridableTextAlignmentOptions _alignment;
        private OverridableFloat _fontSize;
        private OverridableColor _normalColor;
        private OverridableColor _highlightedColor;
        private OverridableColor _pressedColor;
        private OverridableColor _selectedColor;
        private OverridableColor _disabledColor;
        private OverridableFloat _colorMultiplier;
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
