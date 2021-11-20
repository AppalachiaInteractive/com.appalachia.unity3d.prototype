using System;
using System.Collections.Generic;

namespace Appalachia.Prototype.KOC.Inventory
{
    [Serializable]
    public class CharacterInventoryInstance
    {
        

        public InventoryItemInstanceUI selectedItem;
        public List<InventoryItemInstance> items;

    

        public IEnumerable<InventoryItemInstance> GetAllItems()
        {
            return GetItems(InventoryState.Owned);
        }

        public IEnumerable<InventoryItemInstance> GetAllItems(InventoryCategory category)
        {
            return GetItems(InventoryState.Owned, true, category);
        }

        public IEnumerable<InventoryItemInstance> GetAllItems(
            InventoryCategory category,
            InventorySubcategory subcategory)
        {
            return GetItems(InventoryState.Owned, true, category, true, subcategory);
        }

        public IEnumerable<InventoryItemInstance> GetEquippedItems()
        {
            return GetItems(InventoryState.Equipped);
        }

        public IEnumerable<InventoryItemInstance> GetEquippedItems(InventoryCategory category)
        {
            return GetItems(InventoryState.Equipped, true, category);
        }

        public IEnumerable<InventoryItemInstance> GetEquippedItems(
            InventoryCategory category,
            InventorySubcategory subcategory)
        {
            return GetItems(InventoryState.Equipped, true, category, true, subcategory);
        }

        public IEnumerable<InventoryItemInstance> GetFavoritedItems()
        {
            return GetItems(InventoryState.Favorited);
        }

        public IEnumerable<InventoryItemInstance> GetFavoritedItems(InventoryCategory category)
        {
            return GetItems(InventoryState.Favorited, true, category);
        }

        public IEnumerable<InventoryItemInstance> GetFavoritedItems(
            InventoryCategory category,
            InventorySubcategory subcategory)
        {
            return GetItems(InventoryState.Favorited, true, category, true, subcategory);
        }

        public IEnumerable<InventoryItemInstance> GetUnequippedItems()
        {
            return GetItems(InventoryState.Equipped, false);
        }

        public IEnumerable<InventoryItemInstance> GetUnequippedItems(InventoryCategory category)
        {
            return GetItems(InventoryState.Equipped, false, category);
        }

        public IEnumerable<InventoryItemInstance> GetUnequippedItems(
            InventoryCategory category,
            InventorySubcategory subcategory)
        {
            return GetItems(InventoryState.Equipped, false, category, true, subcategory);
        }

        public IEnumerable<InventoryItemInstance> GetUnfavoritedItems()
        {
            return GetItems(InventoryState.Favorited, false);
        }

        public IEnumerable<InventoryItemInstance> GetUnfavoritedItems(InventoryCategory category)
        {
            return GetItems(InventoryState.Favorited, false, category);
        }

        public IEnumerable<InventoryItemInstance> GetUnfavoritedItems(
            InventoryCategory category,
            InventorySubcategory subcategory)
        {
            return GetItems(InventoryState.Favorited, false, category, true, subcategory);
        }

        public void Validate()
        {
            if (items == null)
            {
                items = new List<InventoryItemInstance>();
            }
        }

        private IEnumerable<InventoryItemInstance> GetItems(
            InventoryState state,
            bool stateInclusive = true,
            InventoryCategory? category = null,
            bool categoryInclusive = true,
            InventorySubcategory? subcategory = null,
            bool subcategoryInclusive = true)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (state.HasFlag(InventoryState.Equipped))
                {
                    if (stateInclusive && !item.IsEquipped)
                    {
                        continue;
                    }

                    if (!stateInclusive && item.IsEquipped)
                    {
                        continue;
                    }
                }

                if (state.HasFlag(InventoryState.Favorited))
                {
                    if (stateInclusive && !item.IsFavorited)
                    {
                        continue;
                    }

                    if (!stateInclusive && item.IsFavorited)
                    {
                        continue;
                    }
                }

                if (category != null)
                {
                    if (categoryInclusive && (item.metadata.category != category))
                    {
                        continue;
                    }

                    if (!categoryInclusive && (item.metadata.category == category))
                    {
                        continue;
                    }
                }

                if (subcategory != null)
                {
                    if (subcategoryInclusive && (item.metadata.subcategory != subcategory))
                    {
                        continue;
                    }

                    if (!subcategoryInclusive && (item.metadata.subcategory == subcategory))
                    {
                        continue;
                    }
                }

                yield return item;
            }
        }
    }
}
