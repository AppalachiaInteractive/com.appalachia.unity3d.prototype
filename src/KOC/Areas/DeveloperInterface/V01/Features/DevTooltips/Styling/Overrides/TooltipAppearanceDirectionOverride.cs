using System;
using Appalachia.Core.Objects.Models;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Styling.Overrides
{
    [Serializable]
    public sealed class
        OverridableTooltipAppearanceDirection : Overridable<TooltipAppearanceDirection,
            OverridableTooltipAppearanceDirection>
    {
        public OverridableTooltipAppearanceDirection() : base(false, default)
        {
        }

        public OverridableTooltipAppearanceDirection(bool overriding, TooltipAppearanceDirection value) : base(
            overriding,
            value
        )
        {
        }

        public OverridableTooltipAppearanceDirection(OverridableTooltipAppearanceDirection value) : base(value)
        {
        }
    }
}
