using System;
using System.Collections.Generic;
using Appalachia.Prototype.KOC.Crafting.Base;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Crafting
{
    [Serializable]
    public class Craftable : CraftingIconComponent<Craftable>
    {
        #region Fields and Autoproperties

        public List<CraftedItem> craftedItems;
        public List<CraftingRecipe> recipes;

        #endregion

#if UNITY_EDITOR
        [ButtonGroup]
        public void NewCraftedItem()
        {
            if (craftedItems == null)
            {
                craftedItems = new List<CraftedItem>();
            }

            craftedItems.Add(CreateNew<CraftedItem>());
        }

        [ButtonGroup]
        public void NewCraftingRecipe()
        {
            if (recipes == null)
            {
                recipes = new List<CraftingRecipe>();
            }

            recipes.Add(new CraftingRecipe());
        }

        [UnityEditor.MenuItem(
            PKG.Menu.Appalachia.Components.Crafting.Craftable.Base,
            false,
            PKG.Menu.Appalachia.Components.Crafting.Craftable.Priority
        )]
        private static void MENU_CREATE()
        {
            var created = CreateNew<Craftable>();
            UnityEditor.Selection.activeObject = created;
        }
#endif
    }
}
