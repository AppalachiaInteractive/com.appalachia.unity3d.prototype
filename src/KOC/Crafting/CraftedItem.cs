using System;
using Appalachia.Prototype.KOC.Crafting.Base;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Crafting
{
    [Serializable]
    public class CraftedItem : CraftingIconComponent<CraftedItem>
    {
        #region Fields and Autoproperties

        public GameObject product;

        #endregion

        #region Menu Items

#if UNITY_EDITOR
        [UnityEditor.MenuItem(
            PKG.Menu.Appalachia.Components.Crafting.Item.Base,
            false,
            PKG.Menu.Appalachia.Components.Crafting.Item.Priority
        )]
        private static void MENU_CREATE()
        {
            var created = CreateNew<CraftedItem>();
            UnityEditor.Selection.activeObject = created;
        }
#endif

        #endregion
    }
}
