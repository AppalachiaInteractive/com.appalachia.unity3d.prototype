namespace Appalachia.Prototype.KOCPrototype.Inventory
{
    public static class InventoryStateExtensions
    {
        public static InventoryState AddFlag(this InventoryState initial, InventoryState adding)
        {
            return initial | adding;
        }

        public static InventoryState RemoveFlag(
            this InventoryState initial,
            InventoryState removing)
        {
            return initial & ~removing;
        }
    }
}
