using System;
using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Prototype.KOCPrototype.Inventory
{
    [Serializable]
    [CreateAssetMenu(
        fileName = "InventoryLibrary",
        menuName = "KOC/Inventory/Library",
        order = 100
    )]
    public class InventoryItemLibrary : ScriptableObject
    {
        public GameObject uiPrefab;

        public List<InventoryItemMetadata> items;
    }
}
