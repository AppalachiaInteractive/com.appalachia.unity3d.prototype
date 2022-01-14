using System;
using Appalachia.Core.Objects.Root;

namespace Appalachia.Prototype.KOC.Features.Inventory
{
    [Serializable]
    public class InventoryItemInstance : AppalachiaSimpleBase
    {
        #region Fields and Autoproperties

        public float condition;
        public InventoryItemMetadata metadata;

        public InventoryState state;

        #endregion

        public bool IsBroken => condition <= 0.0001f;
        public bool IsDamaged => condition < .66f;
        public bool IsEquipped => state.HasFlag(InventoryState.Equipped);
        public bool IsFavorited => state.HasFlag(InventoryState.Favorited);

        public bool IsOwned => state.HasFlag(InventoryState.Owned);
        public bool IsSeverelyDamaged => condition < .33f;

        public void SetEquipped()
        {
            state = state.AddFlag(InventoryState.Equipped);
        }

        public void SetFavorite()
        {
            state = state.AddFlag(InventoryState.Favorited);
        }

        public void SetOwned()
        {
            state = state.AddFlag(InventoryState.Owned);
        }

        public void UnsetEquipped()
        {
            state = state.RemoveFlag(InventoryState.Equipped);
        }

        public void UnsetFavorite()
        {
            state = state.RemoveFlag(InventoryState.Favorited);
        }

        public void UnsetOwned()
        {
            state = state.RemoveFlag(InventoryState.Owned);
        }
    }
}
