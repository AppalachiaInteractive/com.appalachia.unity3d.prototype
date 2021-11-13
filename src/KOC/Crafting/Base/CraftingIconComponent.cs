using System;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Crafting.Base
{
    [Serializable]
    public abstract class CraftingIconComponent<T> : CraftingComponent<T>
        where T : CraftingIconComponent<T>
    {
        public Texture2D icon;
    }
}
