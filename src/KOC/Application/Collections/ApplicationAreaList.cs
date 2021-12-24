using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Areas;

namespace Appalachia.Prototype.KOC.Application.Collections
{
    [Serializable]
    public sealed class ApplicationAreaList : AppaList<ApplicationArea>
    {
        public ApplicationAreaList()
        {
        }

        public ApplicationAreaList(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public ApplicationAreaList(AppaList<ApplicationArea> list) : base(list)
        {
        }

        public ApplicationAreaList(ApplicationArea[] values) : base(values)
        {
        }
    }
}
