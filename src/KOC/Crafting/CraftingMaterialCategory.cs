using System;
using System.Collections.Generic;
using Appalachia.Prototype.KOC.Crafting.Base;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Crafting
{
    /// <summary>
    ///     A flexible category of raw materials.  For example, wood.
    /// </summary>
    [Serializable]
    public class CraftingMaterialCategory : CraftingIconComponent<CraftingMaterialCategory>
    {
        #region Fields and Autoproperties

        public List<CraftingMaterial> materials;

        #endregion

#if UNITY_EDITOR
        [ButtonGroup]
        public void NewMaterial()
        {
            if (materials == null)
            {
                materials = new List<CraftingMaterial>();
            }

            materials.Add(CreateNew<CraftingMaterial>());
        }

        [UnityEditor.MenuItem(
            PKG.Menu.Appalachia.Components.Crafting.MaterialCategory.Base,
            false,
            PKG.Menu.Appalachia.Components.Crafting.MaterialCategory.Priority
        )]
        private static void MENU_CREATE()
        {
            var created = CreateNew<CraftingMaterialCategory>();
            UnityEditor.Selection.activeObject = created;
        }
#endif
    }
}
