using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.CI.Integration.Assets;
using Appalachia.Prototype.KOC.Features.Crafting.Base;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Features.Crafting
{
    /// <summary>
    ///     A raw material used in crafting.  For example, white pine wood.
    /// </summary>
    [Serializable]
    public class CraftingMaterial : CraftingIconComponent<CraftingMaterial>
    {
        #region Fields and Autoproperties

        [NonSerialized] private bool _categoriesInitialized;

        [NonSerialized]
        [ShowInInspector]
        private List<CraftingMaterialCategory> _categories;

        #endregion

#if UNITY_EDITOR
        public void RefreshCategories()
        {
            if (_categories == null)
            {
                _categories = new List<CraftingMaterialCategory>();
            }

            var categories = AssetDatabaseManager.FindAssets<CraftingMaterialCategory>();

            _categories = categories.Where(c => c.materials.Contains(this)).ToList();
        }

        [OnInspectorGUI]
        private void SetupCategories()
        {
            if (_categoriesInitialized)
            {
                return;
            }

            _categoriesInitialized = true;

            RefreshCategories();
        }

        [UnityEditor.MenuItem(
            PKG.Menu.Appalachia.Components.Crafting.Material.Base,
            false,
            PKG.Menu.Appalachia.Components.Crafting.Material.Priority
        )]
        private static void MENU_CREATE()
        {
            var created = CreateNew<CraftingMaterial>();
            UnityEditor.Selection.activeObject = created;
        }
#endif
    }
}
