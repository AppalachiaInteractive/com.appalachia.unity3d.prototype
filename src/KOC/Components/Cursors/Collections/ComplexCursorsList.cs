using System;
using Appalachia.Core.Collections;

namespace Appalachia.Prototype.KOC.Components.Cursors.Collections
{
    [Serializable]
    public sealed class ComplexCursorsList : AppaList<ComplexCursors>
    {
        public ComplexCursorsList()
        {
        }

        public ComplexCursorsList(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false)
            : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public ComplexCursorsList(AppaList<ComplexCursors> list) : base(list)
        {
        }

        public ComplexCursorsList(ComplexCursors[] values) : base(values)
        {
        }
    }
}
