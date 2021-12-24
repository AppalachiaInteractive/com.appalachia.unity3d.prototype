using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Root;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Inventory
{
    [Serializable]
    public class InventoryItemLibrary : AppalachiaObject<InventoryItemLibrary>
    {
        #region Fields and Autoproperties

        public GameObject uiPrefab;

        public List<InventoryItemMetadata> items;

        #endregion

        #region Menu Items

#if UNITY_EDITOR
        [UnityEditor.MenuItem(
            PKG.Menu.Assets.Base + nameof(InventoryItemLibrary),
            priority = PKG.Menu.Assets.Priority
        )]
        public static void CreateAsset()
        {
            CreateNew<InventoryItemLibrary>();
        }
#endif

        #endregion
    }
}
