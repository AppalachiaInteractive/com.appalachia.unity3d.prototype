using System;
using Appalachia.Core.Objects.Models;
using Appalachia.Prototype.KOC.Components.OnScreenButtons;

namespace Appalachia.Prototype.KOC.Components.Styling.Overrides
{
    [Serializable]
    public class
        OverridableOnScreenButtonTextStyle : Overridable<OnScreenButtonTextStyle,
            OverridableOnScreenButtonTextStyle>
    {
        public OverridableOnScreenButtonTextStyle(bool overriding, OnScreenButtonTextStyle value) : base(
            overriding,
            value
        )
        {
        }

        public OverridableOnScreenButtonTextStyle(
            Overridable<OnScreenButtonTextStyle, OverridableOnScreenButtonTextStyle> value) : base(value)
        {
        }

        public OverridableOnScreenButtonTextStyle() : base(false, OnScreenButtonTextStyle.DisplayName)
        {
        }
    }
}
