using System;
using Appalachia.Core.Collections;

namespace Appalachia.Prototype.KOC.Components.Cursors.Collections
{
    [Serializable]
    public sealed class CursorInstanceList : AppaList<ComplexCursorInstance>
    {
        public CursorInstanceList()
        {
        }

        public CursorInstanceList(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false)
            : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public CursorInstanceList(AppaList<ComplexCursorInstance> list) : base(list)
        {
        }

        public CursorInstanceList(ComplexCursorInstance[] values) : base(values)
        {
        }
    }
}
