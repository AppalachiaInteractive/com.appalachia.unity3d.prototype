using System;
using Appalachia.Core.Collections;

namespace Appalachia.Prototype.KOC.Components.Cursors.Collections
{
    [Serializable]
    public sealed class SimpleCursorsList : AppaList<SimpleCursors>
    {
        public SimpleCursorsList()
        {
        }

        public SimpleCursorsList(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false)
            : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public SimpleCursorsList(AppaList<SimpleCursors> list) : base(list)
        {
        }

        public SimpleCursorsList(SimpleCursors[] values) : base(values)
        {
        }
    }
}
