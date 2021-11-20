using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Areas;

namespace Appalachia.Prototype.KOC.Application.Collections
{
    [Serializable]
    public sealed class AppaList_ApplicationArea : AppaList<ApplicationArea>
    {
        public AppaList_ApplicationArea()
        {
        }

        public AppaList_ApplicationArea(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_ApplicationArea(AppaList<ApplicationArea> list) : base(list)
        {
        }

        public AppaList_ApplicationArea(ApplicationArea[] values) : base(values)
        {
        }
    }
}
