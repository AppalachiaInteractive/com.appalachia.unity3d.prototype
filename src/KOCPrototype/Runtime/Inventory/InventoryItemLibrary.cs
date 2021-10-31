using System;
using System.Collections.Generic;
using Appalachia.Core.Scriptables;
using UnityEngine;

namespace Appalachia.Prototype.KOCPrototype.Inventory
{
    [Serializable]    
    public class InventoryItemLibrary : AppalachiaObject<InventoryItemLibrary>
    {
        public GameObject uiPrefab;

        public List<InventoryItemMetadata> items;

        [UnityEditor.MenuItem(PKG.Menu.Assets.Base + nameof(InventoryItemLibrary), priority = PKG.Menu.Assets.Priority)]
        public static void CreateAsset()
        {
            CreateNew();
        }
    }
}
