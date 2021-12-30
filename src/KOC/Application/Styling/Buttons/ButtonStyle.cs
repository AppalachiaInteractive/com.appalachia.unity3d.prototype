using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Styling.Base;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Styling.Buttons
{
    [Serializable]
    public class ButtonStyle : ApplicationStyleElementDefault<ButtonStyle, ButtonStyleOverride, IButtonStyle>,
                               IButtonStyle
    {
        #region Fields and Autoproperties

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private TextAlignmentOptions _alignment;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private float _fontSize;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private Color _normalColor;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private Color _highlightedColor;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private Color _pressedColor;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private Color _selectedColor;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private Color _disabledColor;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private float _colorMultiplier;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private float _fadeDuration;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                await initializer.Do(
                    this,
                    nameof(_normalColor),
                    () => _normalColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1.0f)
                );
                await initializer.Do(
                    this,
                    nameof(_highlightedColor),
                    () => _highlightedColor = new Color(245f / 255f, 245f / 255f, 245f / 255f, 1.0f)
                );
                await initializer.Do(
                    this,
                    nameof(_pressedColor),
                    () => _pressedColor = new Color(200f / 255f, 200f / 255f, 200f / 255f, 1.0f)
                );
                await initializer.Do(
                    this,
                    nameof(_selectedColor),
                    () => _selectedColor = new Color(245f / 255f, 245f / 255f, 245f / 255f, 1.0f)
                );
                await initializer.Do(
                    this,
                    nameof(_disabledColor),
                    () => _disabledColor = new Color(200f / 255f, 200f / 255f, 200f / 255f, 0.5f)
                );

                await initializer.Do(this, nameof(_colorMultiplier), () => _colorMultiplier = 1.0f);
                await initializer.Do(this, nameof(_fadeDuration),    () => _fadeDuration = .1f);
                await initializer.Do(this, nameof(_fontSize),        () => _fontSize = 14f);
                await initializer.Do(
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

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_RegisterOverride =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterOverride));

        #endregion
    }
}
