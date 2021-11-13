using System;

namespace Appalachia.Prototype.KOC.Inventory
{
    [Flags]
    public enum InventoryState
    {
        Owned = 0x0001,
        Equipped = 0x0002,
        Favorited = 0x0004
    }
}
