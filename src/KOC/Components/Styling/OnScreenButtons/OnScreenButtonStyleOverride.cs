using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Overrides.Implementations;
using Appalachia.Prototype.KOC.Components.OnScreenButtons;
using Appalachia.Prototype.KOC.Components.Styling.Overrides;
using Appalachia.UI.Core.Styling.Elements;
using Appalachia.UI.Core.Styling.Fonts;
using Appalachia.UI.Core.Styling.Overrides;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.Styling.OnScreenButtons
{
    [Serializable]
    [SmartLabelChildren, SmartLabel]
    [CreateAssetMenu(
        fileName = "New " + nameof(OnScreenButtonStyleOverride),
        menuName = PKG.Prefix + nameof(OnScreenButtonStyleOverride),
        order = PKG.Menu.Assets.Priority
    )]
    public class OnScreenButtonStyleOverride : StyleElementOverride<OnScreenButtonStyle,
                                                   OnScreenButtonStyleOverride, IOnScreenButtonStyle>,
                                               IOnScreenButtonStyle
    {
        #region Fields and Autoproperties

        [SerializeField, OnValueChanged(nameof(OnChanged)), SmartLabelChildren, SmartLabel]
        private OverridableColor _spriteColor;

        [SerializeField, OnValueChanged(nameof(OnChanged)), SmartLabelChildren, SmartLabel]
        private OverridableColor _textColor;

        [SerializeField, OnValueChanged(nameof(OnChanged)), SmartLabelChildren, SmartLabel]
        private OverridableFontStyleOverride _font;

        [SerializeField, OnValueChanged(nameof(OnChanged)), SmartLabelChildren, SmartLabel]
        private OverridableOnScreenButtonSpriteStyle _spriteStyle;

        [SerializeField, OnValueChanged(nameof(OnChanged)), SmartLabelChildren, SmartLabel]
        private OverridableOnScreenButtonTextStyle _textStyle;

        #endregion

        /// <inheritdoc />
        public override void SyncWithDefault()
        {
            using (_PRF_SyncWithDefault.Auto())
            {
                if (!_spriteColor.Overriding)
                {
                    _spriteColor.Value = Defaults.SpriteColor;
                }

                if (!_textColor.Overriding)
                {
                    _textColor.Value = Defaults.TextColor;
                }

                if (!_font.Overriding)
                {
                    _font.Value = Defaults.Font;
                }

                if (!_spriteStyle.Overriding)
                {
                    _spriteStyle.Value = Defaults.SpriteStyle;
                }

                if (!_textStyle.Overriding)
                {
                    _textStyle.Value = Defaults.TextStyle;
                }
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                initializer.Do(this, nameof(_spriteColor), () => _spriteColor = new(false, default));
                initializer.Do(this, nameof(_textColor),   () => _textColor = new(false, default));
                initializer.Do(this, nameof(_font),        () => _font = new(false, default));
                initializer.Do(this, nameof(_spriteStyle), () => _spriteStyle = new(false, default));
                initializer.Do(this, nameof(_textStyle),   () => _textStyle = new(false, default));
                
                SyncWithDefault();
            }
        }

        /// <inheritdoc />
        protected override void RegisterOverrideSubscriptions()
        {
            using (_PRF_RegisterOverrideSubscriptions.Auto())
            {
                _spriteColor.Changed.Event += OnChanged;
                _textColor.Changed.Event += OnChanged;
                _font.Changed.Event += OnChanged;
                _spriteStyle.Changed.Event += OnChanged;
                _textStyle.Changed.Event += OnChanged;
            }
        }

        #region IOnScreenButtonStyle Members

        public Color SpriteColor
        {
            get => _spriteColor.Get(Defaults.SpriteColor);
            set => _spriteColor.OverrideValue(value);
        }

        public Color TextColor
        {
            get => _textColor.Get(Defaults.TextColor);
            set => _textColor.OverrideValue(value);
        }

        public FontStyleOverride Font
        {
            get => _font.Get(Defaults.Font);
            set => _font.OverrideValue(value);
        }

        public OnScreenButtonSpriteStyle SpriteStyle
        {
            get => _spriteStyle.Get(Defaults.SpriteStyle);
            set => _spriteStyle.OverrideValue(value);
        }

        public OnScreenButtonTextStyle TextStyle
        {
            get => _textStyle.Get(Defaults.TextStyle);
            set => _textStyle.OverrideValue(value);
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterOverrideSubscriptions =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterOverrideSubscriptions));

        private static readonly ProfilerMarker _PRF_SyncWithDefault =
            new ProfilerMarker(_PRF_PFX + nameof(SyncWithDefault));

        #endregion
    }
}
