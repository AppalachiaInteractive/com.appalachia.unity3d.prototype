using System;
using Appalachia.Core.Objects.Models;

namespace Appalachia.Prototype.KOC.Components.Styling.OnScreenButtons
{
    [Serializable]
    public class OverridableOnScreenButtonStyleTypes : Overridable<OnScreenButtonStyleTypes, OverridableOnScreenButtonStyleTypes>
    {
        public OverridableOnScreenButtonStyleTypes(OnScreenButtonStyleTypes value) : base(false, value)
        {
        }

        public OverridableOnScreenButtonStyleTypes(bool overriding, OnScreenButtonStyleTypes value) : base(
            overriding,
            value
        )
        {
        }

        public OverridableOnScreenButtonStyleTypes(
            Overridable<OnScreenButtonStyleTypes, OverridableOnScreenButtonStyleTypes> value) : base(value)
        {
        }

        public OverridableOnScreenButtonStyleTypes() : base(false, default)
        {
        }
    }
}
