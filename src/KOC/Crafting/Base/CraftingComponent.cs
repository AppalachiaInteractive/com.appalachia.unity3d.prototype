using System;
using Appalachia.Core.Objects.Scriptables;

namespace Appalachia.Prototype.KOC.Crafting.Base
{
    [Serializable]
    public abstract class CraftingComponent<T> : AutonamedIdentifiableAppalachiaObject<T>
        where T : CraftingComponent<T>
    {
    }
}
