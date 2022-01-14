using Appalachia.Core.Overrides;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.Menus.Overrides
{
    public class Texture2DOverride : Overridable<Texture2D, Texture2DOverride>
    {
        public Texture2DOverride(bool overrideEnabled, Texture2D value) : base(overrideEnabled, value)
        {
        }

        public Texture2DOverride(Overridable<Texture2D, Texture2DOverride> value) : base(value)
        {
        }

        public Texture2DOverride() : base(false, default)
        {
        }
    }
}
