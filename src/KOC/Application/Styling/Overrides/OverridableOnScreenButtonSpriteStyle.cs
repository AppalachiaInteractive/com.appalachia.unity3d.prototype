using System;
using Appalachia.Core.Overrides;
using Appalachia.Prototype.KOC.Application.Input.OnScreenButtons;

namespace Appalachia.Prototype.KOC.Application.Styling.Overrides
{
    [Serializable]
    public class
        OverridableOnScreenButtonSpriteStyle : Overridable<OnScreenButtonSpriteStyle,
            OverridableOnScreenButtonSpriteStyle>
    {
        public OverridableOnScreenButtonSpriteStyle(bool overrideEnabled, OnScreenButtonSpriteStyle value) :
            base(overrideEnabled, value)
        {
        }

        public OverridableOnScreenButtonSpriteStyle(
            Overridable<OnScreenButtonSpriteStyle, OverridableOnScreenButtonSpriteStyle> value) : base(value)
        {
        }

        public OverridableOnScreenButtonSpriteStyle() : base(false, OnScreenButtonSpriteStyle.Outline)
        {
        }
    }
}
