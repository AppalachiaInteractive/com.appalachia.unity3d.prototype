using System;
using Appalachia.Prototype.KOC.Crafting.Base;

namespace Appalachia.Prototype.KOC.Crafting
{
    [Serializable]
    public class CraftingSkill : CraftingIconComponent<CraftingSkill>
    {
        #region Menu Items

#if UNITY_EDITOR
        [UnityEditor.MenuItem(
            PKG.Menu.Appalachia.Components.Crafting.Skill.Base,
            priority = PKG.Menu.Appalachia.Components.Crafting.Skill.Priority
        )]
        private static void MENU_CREATE()
        {
            var created = CreateNew<CraftingSkill>();
            UnityEditor.Selection.activeObject = created;
        }
#endif

        #endregion
    }
}
