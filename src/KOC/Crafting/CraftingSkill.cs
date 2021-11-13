using System;
using Appalachia.Prototype.KOC.Crafting.Base;

namespace Appalachia.Prototype.KOC.Crafting
{
    [Serializable]
    public class CraftingSkill : CraftingIconComponent<CraftingSkill>
    {
#if UNITY_EDITOR
        [UnityEditor.MenuItem(PKG.Menu.Appalachia.Components.Crafting.Skill.Base, priority =  
            PKG.Menu.Appalachia.Components.Crafting.Skill.Priority)]
        private static void MENU_CREATE()
        {
            var created = CreateNew();
            UnityEditor.Selection.activeObject = created;
        }
#endif
    }
}
