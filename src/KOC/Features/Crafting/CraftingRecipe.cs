using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Root;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Features.Crafting
{
    [Serializable]
    public class CraftingRecipe : AppalachiaSimpleBase
    {
        public CraftingRecipe()
        {
            elements = new List<CraftingRecipeElement>();
        }

        #region Fields and Autoproperties

        public List<CraftingRecipeElement> elements;

        #endregion

        [ButtonGroup]
        public void NewElement()
        {
            if (elements == null)
            {
                elements = new List<CraftingRecipeElement>();
            }

            elements.Add(new CraftingRecipeElement());
        }
    }
}