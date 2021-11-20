using System;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Scriptables;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Crafting
{
    [Serializable]
    [InlineEditor]
    public class CraftingIngredient
    {
        #region Fields and Autoproperties

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

        #endregion

        private bool _showMaterial => ingredientType == CraftingIngredientType.MaterialCategory;

#if UNITY_EDITOR
        [ButtonGroup]
        public void NewCraftedItem()
        {
            item = AppalachiaObject.CreateNew<CraftedItem>();
        }

        [ButtonGroup]
        public void NewMaterialCategory()
        {
            material = AppalachiaObject.CreateNew<CraftingMaterialCategory>();
        }
#endif
    }
}
