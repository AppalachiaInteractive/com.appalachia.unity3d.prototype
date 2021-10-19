using System;

namespace Appalachia.UI.KOCPrototype.Runtime.Inventory
{
    [Serializable]
    public class InventoryItemInstance
    {
        public InventoryItemMetadata metadata;

        public InventoryState state;

        public float condition;

        public bool IsOwned => state.HasFlag(InventoryState.Owned);
        public bool IsEquipped => state.HasFlag(InventoryState.Equipped);
        public bool IsFavorited => state.HasFlag(InventoryState.Favorited);
        public bool IsDamaged => condition < .66f;
        public bool IsSeverelyDamaged => condition < .33f;
        public bool IsBroken => condition <= 0.0001f;

        public void SetOwned()
        {
            state = state.AddFlag(InventoryState.Owned);
        }

        public void UnsetOwned()
        {
            state = state.RemoveFlag(InventoryState.Owned);
        }

        public void SetEquipped()
        {
            state = state.AddFlag(InventoryState.Equipped);
        }

        public void UnsetEquipped()
        {
            state = state.RemoveFlag(InventoryState.Equipped);
        }

        public void SetFavorite()
        {
            state = state.AddFlag(InventoryState.Favorited);
        }

        public void UnsetFavorite()
        {
            state = state.RemoveFlag(InventoryState.Favorited);
        }
    }
}
