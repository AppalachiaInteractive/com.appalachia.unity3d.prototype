using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Root;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Crafting
{
    [Serializable]
    public class CraftingRecipeElement : AppalachiaSimpleBase
    {
        public CraftingRecipeElement()
        {
            options = new List<CraftingRecipeElementOption>();
        }

        

        public List<CraftingRecipeElementOption> options;


        [ButtonGroup]
        public void NewOptions()
        {
            if (options == null)
            {
                options = new List<CraftingRecipeElementOption>();
            }

            options.Add(new CraftingRecipeElementOption());
        }
    }
}
