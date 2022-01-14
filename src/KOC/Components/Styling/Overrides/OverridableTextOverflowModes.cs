using System;
using Appalachia.Core.Overrides;
using TMPro;

namespace Appalachia.Prototype.KOC.Components.Styling.Overrides
{
    [Serializable]
    public class OverridableTextOverflowModes : Overridable<TextOverflowModes, OverridableTextOverflowModes>
    {
        public OverridableTextOverflowModes(bool overrideEnabled, TextOverflowModes value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableTextOverflowModes(
            Overridable<TextOverflowModes, OverridableTextOverflowModes> value) : base(value)
        {
        }

        public OverridableTextOverflowModes() : base(false, TextOverflowModes.Overflow)
        {
        }
    }
}
