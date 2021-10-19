using System;
using UnityEngine;

namespace Appalachia.UI.KOCPrototype.Runtime.Inventory
{
    [Serializable]
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "KOC/Inventory/Item", order = 100)]
    public class InventoryItemMetadata : ScriptableObject
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
    }
}
