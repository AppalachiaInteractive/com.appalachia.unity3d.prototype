using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Root;
using Sirenix.OdinInspector;

namespace Appalachia.Prototype.KOC.Features.Crafting
{
    [Serializable]
    public class CraftingRecipeElement : AppalachiaSimpleBase
    {
        public CraftingRecipeElement()
        {
            options = new List<CraftingRecipeElementOption>();
        }

        #region Fields and Autoproperties

        public List<CraftingRecipeElementOption> options;

        #endregion

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
