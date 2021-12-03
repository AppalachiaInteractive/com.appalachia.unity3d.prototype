using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.State;

namespace Appalachia.Prototype.KOC.Application.Collections
{
    [Serializable]
    public sealed class AppaList_ApplicationAreaState : AppaList<ApplicationAreaState>
    {
        public AppaList_ApplicationAreaState()
        {
        }

        public AppaList_ApplicationAreaState(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_ApplicationAreaState(AppaList<ApplicationAreaState> list) : base(list)
        {
        }

        public AppaList_ApplicationAreaState(ApplicationAreaState[] values) : base(values)
        {
        }
    }
}
