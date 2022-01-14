using System;
using Appalachia.Core.Overrides;
using TMPro;

namespace Appalachia.Prototype.KOC.Components.Styling.Overrides
{
    [Serializable]
    public class
        OverridableTextAlignmentOptions : Overridable<TextAlignmentOptions, OverridableTextAlignmentOptions>
    {
        public OverridableTextAlignmentOptions(bool overrideEnabled, TextAlignmentOptions value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableTextAlignmentOptions(
            Overridable<TextAlignmentOptions, OverridableTextAlignmentOptions> value) : base(value)
        {
        }

        public OverridableTextAlignmentOptions() : base(false, TextAlignmentOptions.Center)
        {
        }
    }
}
