using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.State;

namespace Appalachia.Prototype.KOC.Application.Collections
{
    [Serializable]
    public sealed class AppaList_ApplicationSubstate : AppaList<ApplicationSubstate>
    {
        public AppaList_ApplicationSubstate()
        {
        }

        public AppaList_ApplicationSubstate(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_ApplicationSubstate(AppaList<ApplicationSubstate> list) : base(list)
        {
        }

        public AppaList_ApplicationSubstate(ApplicationSubstate[] values) : base(values)
        {
        }
    }
}