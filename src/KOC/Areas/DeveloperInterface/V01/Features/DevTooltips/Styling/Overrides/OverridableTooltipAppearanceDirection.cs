using System;
using Appalachia.Core.Objects.Models;
using Appalachia.UI.Core.Layout;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DevTooltips.Styling.Overrides
{
    [Serializable]
    public sealed class
        OverridableTooltipAppearanceDirection : Overridable<AppearanceDirection, OverridableTooltipAppearanceDirection>
    {
        public OverridableTooltipAppearanceDirection() : base(false, default)
        {
        }

        public OverridableTooltipAppearanceDirection(bool overriding, AppearanceDirection value) : base(
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
