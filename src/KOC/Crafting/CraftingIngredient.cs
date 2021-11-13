using System;
using Appalachia.Core.Attributes.Editing;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Crafting
{
    [Serializable]
    [InlineEditor]
    public class CraftingIngredient
    {
        [SmartLabel]
        [HideIf(nameof(_showMaterial))]
        public CraftedItem item;

        [SmartLabel]
        [EnumToggleButtons]
        public CraftingIngredientType ingredientType;

        [SmartLabel]
        [ShowIf(nameof(_showMaterial))]
        public CraftingMaterialCategory material;

        [SmartLabel]
        [ShowIf(nameof(_showMaterial))]
        public float materialAmount = 1.0f;

        [SmartLabel]
        [HideIf(nameof(_showMaterial))]
        public int itemCount = 1;

        private bool _showMaterial => ingredientType == CraftingIngredientType.MaterialCategory;

#if UNITY_EDITOR
        [ButtonGroup]
        public void NewCraftedItem()
        {
            item = CraftedItem.CreateNew();
        }

        [ButtonGroup]
        public void NewMaterialCategory()
        {
            material = CraftingMaterialCategory.CreateNew();
        }
#endif
    }
}
