using System;
using Appalachia.Core.Overrides;
using TMPro;

namespace Appalachia.Prototype.KOC.Application.Styling.Overrides
{
    [Serializable]
    public class
        OverridableVerticalAlignmentOptions : Overridable<VerticalAlignmentOptions,
            OverridableVerticalAlignmentOptions>
    {
        public OverridableVerticalAlignmentOptions(bool overrideEnabled, VerticalAlignmentOptions value) :
            base(overrideEnabled, value)
        {
        }

        public OverridableVerticalAlignmentOptions(
            Overridable<VerticalAlignmentOptions, OverridableVerticalAlignmentOptions> value) : base(value)
        {
        }

        public OverridableVerticalAlignmentOptions() : base(false, VerticalAlignmentOptions.Middle)
        {
        }
    }
}
