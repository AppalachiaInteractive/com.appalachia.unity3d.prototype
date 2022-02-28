using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Model;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Collections
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
