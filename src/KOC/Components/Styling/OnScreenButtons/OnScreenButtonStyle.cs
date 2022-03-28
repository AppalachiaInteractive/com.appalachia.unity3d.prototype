using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Components.OnScreenButtons;
using Appalachia.UI.Styling.Elements;
using Appalachia.UI.Styling.Fonts;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.Styling.OnScreenButtons
{
    [Serializable]
    public class OnScreenButtonStyle : StyleElementDefault<OnScreenButtonStyle,
                                           OnScreenButtonStyleOverride, IOnScreenButtonStyle, OnScreenButtonStyleTypes>,
                                       IOnScreenButtonStyle
    {
        #region Fields and Autoproperties

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private Color _spriteColor;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private Color _textColor;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private FontStyleTypes _font;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private OnScreenButtonSpriteStyle _spriteStyle;

        [SerializeField, OnValueChanged(nameof(OnChanged))]
        private OnScreenButtonTextStyle _textStyle;

        #endregion

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

#if UNITY_EDITOR

            initializer.Do(
                this,
                nameof(_font),
                _font == default,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        _font = FontStyleTypes.OnScreenButton;
                    }
                }
            );

            initializer.Do(
                this,
                nameof(OnScreenButtonSpriteStyle),
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        _spriteStyle = OnScreenButtonSpriteStyle.Outline;
                    }
                }
            );

            initializer.Do(
                this,
                nameof(OnScreenButtonTextStyle),
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        _textStyle = OnScreenButtonTextStyle.DisplayName;
                    }
                }
            );

            initializer.Do(
                this,
                nameof(_spriteColor),
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        _spriteColor = Color.white;
                    }
                }
            );
            initializer.Do(
                this,
                nameof(_textColor),
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        _textColor = Color.white;
                    }
                }
            );

#endif
        }

        #region IOnScreenButtonStyle Members

        public Color SpriteColor => _spriteColor;
        public Color TextColor => _textColor;
        public FontStyleTypes Font => _font;
        public OnScreenButtonSpriteStyle SpriteStyle => _spriteStyle;
        public OnScreenButtonTextStyle TextStyle => _textStyle;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterOverride =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterOverride));

        #endregion
    }
}
