using System;
using Appalachia.Prototype.KOC.Application.Input.OnScreenButtons;
using Appalachia.Prototype.KOC.Application.Styling.Base;
using Appalachia.Prototype.KOC.Application.Styling.Fonts;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Styling.OnScreenButtons
{
    [Serializable]
    public class OnScreenButtonStyle : ApplicationStyleElementDefault<OnScreenButtonStyle,
                                           OnScreenButtonStyleOverride, IOnScreenButtonStyle>,
                                       IOnScreenButtonStyle
    {
        #region Fields and Autoproperties

        [SerializeField] private Color _spriteColor;
        [SerializeField] private Color _textColor;
        [SerializeField] private FontStyleOverride _font;
        [SerializeField] private OnScreenButtonSpriteStyle _spriteStyle;
        [SerializeField] private OnScreenButtonTextStyle _textStyle;

        #endregion

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                initializer.Initialize(
                    this,
                    nameof(_font),
                    _font == null,
                    () => { _font = LoadOrCreateNew<FontStyleOverride>("On Screen Buttons"); }
                );

                initializer.Initialize(
                    this,
                    nameof(OnScreenButtonSpriteStyle),
                    () => { _spriteStyle = OnScreenButtonSpriteStyle.Outline; }
                );

                initializer.Initialize(
                    this,
                    nameof(OnScreenButtonTextStyle),
                    () => { _textStyle = OnScreenButtonTextStyle.DisplayName; }
                );

                initializer.Initialize(this, nameof(_spriteColor), () => _spriteColor = Color.white);
                initializer.Initialize(this, nameof(_textColor),   () => _textColor = Color.white);

                _font.SyncWithDefault();
            }
        }

        #region IOnScreenButtonStyle Members

        public Color SpriteColor => _spriteColor;
        public Color TextColor => _textColor;
        public FontStyleOverride Font => _font;
        public OnScreenButtonSpriteStyle SpriteStyle => _spriteStyle;
        public OnScreenButtonTextStyle TextStyle => _textStyle;

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(OnScreenButtonStyle) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_RegisterOverride =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterOverride));

        #endregion
    }
}
