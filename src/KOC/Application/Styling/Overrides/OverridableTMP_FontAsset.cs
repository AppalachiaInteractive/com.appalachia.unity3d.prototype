using System;
using Appalachia.Core.Overrides;
using TMPro;

namespace Appalachia.Prototype.KOC.Application.Styling.Overrides
{
    [Serializable]
    public class OverridableTMP_FontAsset : Overridable<TMP_FontAsset, OverridableTMP_FontAsset>
    {
        public OverridableTMP_FontAsset(bool overrideEnabled, TMP_FontAsset value) : base(overrideEnabled, value)
        {
        }

        public OverridableTMP_FontAsset(Overridable<TMP_FontAsset, OverridableTMP_FontAsset> value) : base(value)
        {
        }

        public OverridableTMP_FontAsset() : base(false, null)
        {
        }
    }
}