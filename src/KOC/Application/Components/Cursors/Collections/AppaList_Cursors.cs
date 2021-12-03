using System;
using Appalachia.Core.Collections;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors.Collections
{
    [Serializable]
    public sealed class AppaList_Cursors : AppaList<Cursors>
    {
        public AppaList_Cursors()
        {
        }

        public AppaList_Cursors(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) :
            base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_Cursors(AppaList<Cursors> list) : base(list)
        {
        }

        public AppaList_Cursors(Cursors[] values) : base(values)
        {
        }
    }
}