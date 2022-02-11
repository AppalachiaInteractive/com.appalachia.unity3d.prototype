using System;
using Appalachia.Core.Objects.Models;
using Appalachia.Prototype.KOC.Components.OnScreenButtons;

namespace Appalachia.Prototype.KOC.Components.Styling.Overrides
{
    [Serializable]
    public class
        OverridableOnScreenButtonSpriteStyle : Overridable<OnScreenButtonSpriteStyle,
            OverridableOnScreenButtonSpriteStyle>
    {
        public OverridableOnScreenButtonSpriteStyle(bool overriding, OnScreenButtonSpriteStyle value) : base(
            overriding,
            value
        )
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
