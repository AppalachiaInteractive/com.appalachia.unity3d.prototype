using System;
using Appalachia.Core.Objects.Root;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Inventory
{
    [Serializable]
    public class InventoryItemMetadata : AppalachiaObject<InventoryItemMetadata>
    {
        #region Fields and Autoproperties

        public InventoryCategory category;
        public InventorySpecialCategory specialCategory;
        public InventorySubcategory subcategory;
        public string itemName;

        public Texture2D preview;

        #endregion

        public InventoryItemInstance CreateInstance()
        {
            return new InventoryItemInstance {condition = 1f, metadata = this, state = InventoryState.Owned};
        }

        #region Menu Items

#if UNITY_EDITOR
        [UnityEditor.MenuItem(
            PKG.Menu.Assets.Base + nameof(InventoryItemMetadata),
            priority = PKG.Menu.Assets.Priority
        )]
        public static void CreateAsset()
        {
            CreateNew<InventoryItemMetadata>();
        }
#endif

        #endregion
    }
}
