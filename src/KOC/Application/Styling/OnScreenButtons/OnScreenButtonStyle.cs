using System;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Input.OnScreenButtons;
using Appalachia.Prototype.KOC.Application.Styling.Base;
using Appalachia.Prototype.KOC.Application.Styling.Fonts;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
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

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private Color _spriteColor;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private Color _textColor;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private FontStyleOverride _font;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OnScreenButtonSpriteStyle _spriteStyle;

        [SerializeField, OnValueChanged(nameof(InvokeStyleChanged))]
        private OnScreenButtonTextStyle _textStyle;

        #endregion

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

#if UNITY_EDITOR
                await initializer.Do(
                    this,
                    nameof(_font),
                    _font == null,
                    () => { _font = LoadOrCreateNew("On Screen Buttons"); }
                );
                await initializer.Do(
                    this,
                    nameof(OnScreenButtonSpriteStyle),
                    () => { _spriteStyle = OnScreenButtonSpriteStyle.Outline; }
                );

                await initializer.Do(
                    this,
                    nameof(OnScreenButtonTextStyle),
                    () => { _textStyle = OnScreenButtonTextStyle.DisplayName; }
                );

                await initializer.Do(this, nameof(_spriteColor), () => _spriteColor = Color.white);
                await initializer.Do(this, nameof(_textColor),   () => _textColor = Color.white);

#endif
                
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

        

        private static readonly ProfilerMarker _PRF_RegisterOverride =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterOverride));

        #endregion
    }
}
