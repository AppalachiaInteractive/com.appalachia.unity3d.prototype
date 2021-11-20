using System;
using Appalachia.CI.Integration.Assets;
using Appalachia.Prototype.KOC.Crafting.Base;

namespace Appalachia.Prototype.KOC.Crafting
{
    [Serializable]
    public class CraftingTool : CraftingIconComponent<CraftingTool>
    {
        #region Menu Items

#if UNITY_EDITOR
        [UnityEditor.MenuItem(
            PKG.Menu.Appalachia.Components.Crafting.Tool.Base,
            priority = PKG.Menu.Appalachia.Components.Crafting.Tool.Priority
        )]
        private static void MENU_CREATE()
        {
            var created = CreateNew<CraftingTool>();
            AssetDatabaseManager.SetSelection(created);
        }
#endif

        #endregion
    }
}
