using System;
using Appalachia.Prototype.KOC.Crafting.Base;

namespace Appalachia.Prototype.KOC.Crafting
{
    [Serializable]
    public class CraftingKnowledge : CraftingIconComponent<CraftingKnowledge>
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem(
            PKG.Menu.Appalachia.Components.Crafting.Knowledge.Base,
            false,
            PKG.Menu.Appalachia.Components.Crafting.Knowledge.Priority
        )]
        private static void MENU_CREATE()
        {
            var created = CreateNew();
            UnityEditor.Selection.activeObject = created;
        }
#endif
    }
}
