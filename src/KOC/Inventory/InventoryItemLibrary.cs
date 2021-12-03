using System;
using System.Collections.Generic;
using Appalachia.Core.Scriptables;
using Appalachia.Prototype.KOC.Application;
using Appalachia.Prototype.KOC.Application.Scriptables;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Inventory
{
    [Serializable]
    public class InventoryItemLibrary : AppalachiaApplicationObject
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
