using System;
using Appalachia.Core.Overrides;
using TMPro;

namespace Appalachia.Prototype.KOC.Application.Styling.Overrides
{
    [Serializable]
    public class OverridableFontWeight : Overridable<FontWeight, OverridableFontWeight>
    {
        public OverridableFontWeight(bool overrideEnabled, FontWeight value) : base(overrideEnabled, value)
        {
        }

        public OverridableFontWeight(Overridable<FontWeight, OverridableFontWeight> value) : base(value)
        {
        }

        public OverridableFontWeight() : base(false, FontWeight.Regular)
        {
        }
    }
}