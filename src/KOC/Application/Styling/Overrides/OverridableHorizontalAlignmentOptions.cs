using System;
using Appalachia.Core.Overrides;
using TMPro;

namespace Appalachia.Prototype.KOC.Application.Styling.Overrides
{
    [Serializable]
    public class OverridableHorizontalAlignmentOptions : Overridable<HorizontalAlignmentOptions,
        OverridableHorizontalAlignmentOptions>
    {
        public OverridableHorizontalAlignmentOptions(bool overrideEnabled, HorizontalAlignmentOptions value) :
            base(overrideEnabled, value)
        {
        }

        public OverridableHorizontalAlignmentOptions(
            Overridable<HorizontalAlignmentOptions, OverridableHorizontalAlignmentOptions> value) : base(
            value
        )
        {
        }

        public OverridableHorizontalAlignmentOptions() : base(false, HorizontalAlignmentOptions.Center)
        {
        }
    }
}
