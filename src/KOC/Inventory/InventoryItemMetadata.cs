using System;
using Appalachia.Core.Scriptables;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Inventory
{
    [Serializable]
    public class InventoryItemMetadata : AppalachiaObject<InventoryItemMetadata>
    {
        public Texture2D preview;
        public string itemName;

        public InventoryCategory category;
        public InventorySubcategory subcategory;
        public InventorySpecialCategory specialCategory;

        public InventoryItemInstance CreateInstance()
        {
            return new InventoryItemInstance
            {
                condition = 1f, metadata = this, state = InventoryState.Owned
            };
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem(PKG.Menu.Assets.Base + nameof(InventoryItemMetadata), priority = PKG.Menu.Assets.Priority)]
        public static void CreateAsset()
        {
            CreateNew();
        }
#endif
    }
}
