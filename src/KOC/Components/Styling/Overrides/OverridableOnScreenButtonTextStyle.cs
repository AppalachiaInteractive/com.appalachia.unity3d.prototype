using System;
using Appalachia.Core.Overrides;
using Appalachia.Prototype.KOC.Components.OnScreenButtons;

namespace Appalachia.Prototype.KOC.Components.Styling.Overrides
{
    [Serializable]
    public class
        OverridableOnScreenButtonTextStyle : Overridable<OnScreenButtonTextStyle,
            OverridableOnScreenButtonTextStyle>
    {
        public OverridableOnScreenButtonTextStyle(bool overrideEnabled, OnScreenButtonTextStyle value) : base(
            overrideEnabled,
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
