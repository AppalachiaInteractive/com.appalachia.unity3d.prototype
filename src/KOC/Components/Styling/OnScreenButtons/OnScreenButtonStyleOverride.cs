using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Overrides.Implementations;
using Appalachia.Prototype.KOC.Components.OnScreenButtons;
using Appalachia.Prototype.KOC.Components.Styling.Overrides;
using Appalachia.UI.Core.Styling.Elements;
using Appalachia.UI.Core.Styling.Fonts;
using Appalachia.UI.Core.Styling.Overrides;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.Styling.OnScreenButtons
{
    [Serializable]
    [SmartLabelChildren, SmartLabel]
    public class OnScreenButtonStyleOverride : StyleElementOverride<OnScreenButtonStyle,
                                                   OnScreenButtonStyleOverride, IOnScreenButtonStyle>,
                                               IOnScreenButtonStyle
    {
        #region Fields and Autoproperties

        [SerializeField, OnValueChanged(nameof(InvokeSettingsChanged)), SmartLabelChildren, SmartLabel]
        private OverridableColor _spriteColor;

        [SerializeField, OnValueChanged(nameof(InvokeSettingsChanged)), SmartLabelChildren, SmartLabel]
        private OverridableColor _textColor;

        [SerializeField, OnValueChanged(nameof(InvokeSettingsChanged)), SmartLabelChildren, SmartLabel]
        private OverridableFontStyleOverride _font;

        [SerializeField, OnValueChanged(nameof(InvokeSettingsChanged)), SmartLabelChildren, SmartLabel]
        private OverridableOnScreenButtonSpriteStyle _spriteStyle;

        [SerializeField, OnValueChanged(nameof(InvokeSettingsChanged)), SmartLabelChildren, SmartLabel]
        private OverridableOnScreenButtonTextStyle _textStyle;

        #endregion

        public override void SyncWithDefault()
        {
            using (_PRF_SyncWithDefault.Auto())
            {
                if (!_spriteColor.overrideEnabled)
                {
                    _spriteColor.value = Defaults.SpriteColor;
                }

                if (!_textColor.overrideEnabled)
                {
                    _textColor.value = Defaults.TextColor;
                }

                if (!_font.overrideEnabled)
                {
                    _font.value = Defaults.Font;
                }

                if (!_spriteStyle.overrideEnabled)
                {
                    _spriteStyle.value = Defaults.SpriteStyle;
                }

                if (!_textStyle.overrideEnabled)
                {
                    _textStyle.value = Defaults.TextStyle;
                }
            }
        }

        protected override void RegisterOverrideSubscriptions()
        {
            using (_PRF_RegisterOverrideSubscriptions.Auto())
            {
                _spriteColor.OverridableChanged += _ => InvokeSettingsChanged();
                _textColor.OverridableChanged += _ => InvokeSettingsChanged();
                _font.OverridableChanged += _ => InvokeSettingsChanged();
                _spriteStyle.OverridableChanged += _ => InvokeSettingsChanged();
                _textStyle.OverridableChanged += _ => InvokeSettingsChanged();
            }
        }

        #region IOnScreenButtonStyle Members

        public Color SpriteColor => _spriteColor.Get(Defaults.SpriteColor);
        public Color TextColor => _textColor.Get(Defaults.TextColor);
        public FontStyleOverride Font => _font.Get(Defaults.Font);
        public OnScreenButtonSpriteStyle SpriteStyle => _spriteStyle.Get(Defaults.SpriteStyle);
        public OnScreenButtonTextStyle TextStyle => _textStyle.Get(Defaults.TextStyle);

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_RegisterOverrideSubscriptions =
            new ProfilerMarker(_PRF_PFX + nameof(RegisterOverrideSubscriptions));

        private static readonly ProfilerMarker _PRF_SyncWithDefault =
            new ProfilerMarker(_PRF_PFX + nameof(SyncWithDefault));

        #endregion
    }
}
