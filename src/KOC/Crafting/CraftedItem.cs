using System;
using Appalachia.Prototype.KOC.Crafting.Base;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Crafting
{
    [Serializable]
    public class CraftedItem : CraftingIconComponent<CraftedItem>
    {
        public GameObject product;
            
            
#if UNITY_EDITOR
        [UnityEditor.MenuItem(
            PKG.Menu.Appalachia.Components.Crafting.Item.Base,
            false,
            PKG.Menu.Appalachia.Components.Crafting.Item.Priority
        )]
        private static void MENU_CREATE()
        {
            var created = CreateNew();
            UnityEditor.Selection.activeObject = created;
        }
#endif
    }
}
