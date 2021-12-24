using System;
using Appalachia.Core.Overrides;
using Appalachia.Prototype.KOC.Application.Styling.Fonts;

namespace Appalachia.Prototype.KOC.Application.Styling.Overrides
{
    [Serializable]
    public class OverridableFontStyleOverride : Overridable<FontStyleOverride, OverridableFontStyleOverride>
    {
        public OverridableFontStyleOverride(FontStyleOverride value) : base(false, value)
        {
        }

        public OverridableFontStyleOverride(bool overrideEnabled, FontStyleOverride value) : base(
            overrideEnabled,
            value
        )
        {
        }

        public OverridableFontStyleOverride(
            Overridable<FontStyleOverride, OverridableFontStyleOverride> value) : base(value)
        {
        }

        public OverridableFontStyleOverride() : base(false, null)
        {
        }
    }
}