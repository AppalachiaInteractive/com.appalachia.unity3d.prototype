using System;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Features.Crafting.Base
{
    [Serializable]
    public abstract class CraftingIconComponent<T> : CraftingComponent<T>
        where T : CraftingIconComponent<T>
    {
        #region Fields and Autoproperties

        public Texture2D icon;

        #endregion
    }
}
