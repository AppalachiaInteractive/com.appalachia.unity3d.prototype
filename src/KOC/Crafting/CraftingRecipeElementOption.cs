using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Scriptables;
using Appalachia.Core.Timing;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Crafting
{
    [Serializable]
    public class CraftingRecipeElementOption
    {
        public CraftingRecipeElementOption()
        {
            craftingTime = Duration.ONE_MINUTE();
            ingredients = new List<CraftingIngredient>();
            knowledges = new List<CraftingKnowledge>();
            skills = new List<CraftingSkill>();
            tools = new List<CraftingTool>();
        }

        #region Fields and Autoproperties

        [SmartLabel] public Duration craftingTime;

        public List<CraftingIngredient> ingredients;

        [InlineEditor] public List<CraftingKnowledge> knowledges;

        [InlineEditor] public List<CraftingSkill> skills;

        [InlineEditor] public List<CraftingTool> tools;

        #endregion

#if UNITY_EDITOR
        [ButtonGroup]
        public void NewIngredient()
        {
            if (ingredients == null)
            {
                ingredients = new List<CraftingIngredient>();
            }

            ingredients.Add(new CraftingIngredient());
        }

        [ButtonGroup]
        public void NewKnowledge()
        {
            if (knowledges == null)
            {
                knowledges = new List<CraftingKnowledge>();
            }

            knowledges.Add(AppalachiaObject.CreateNew<CraftingKnowledge>());
        }

        [ButtonGroup]
        public void NewSkill()
        {
            if (skills == null)
            {
                skills = new List<CraftingSkill>();
            }

            skills.Add(AppalachiaObject.CreateNew<CraftingSkill>());
        }

        [ButtonGroup]
        public void NewTool()
        {
            if (tools == null)
            {
                tools = new List<CraftingTool>();
            }

            tools.Add(AppalachiaObject.CreateNew<CraftingTool>());
        }
#endif
    }
}
