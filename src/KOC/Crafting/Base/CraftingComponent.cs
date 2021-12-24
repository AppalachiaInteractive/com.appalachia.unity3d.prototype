using System;

namespace Appalachia.Prototype.KOC.Crafting.Base
{
    [Serializable]
    public abstract class CraftingComponent<T> : AutonamedIdentifiableAppalachiaObject
        where T : CraftingComponent<T>
    {
    }
}
