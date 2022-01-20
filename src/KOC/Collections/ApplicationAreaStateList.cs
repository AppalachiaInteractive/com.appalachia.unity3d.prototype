using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.State;

namespace Appalachia.Prototype.KOC.Collections
{
    [Serializable]
    public sealed class ApplicationAreaStateList : AppaList<ApplicationAreaState>
    {
        public ApplicationAreaStateList()
        {
        }

        public ApplicationAreaStateList(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public ApplicationAreaStateList(AppaList<ApplicationAreaState> list) : base(list)
        {
        }

        public ApplicationAreaStateList(ApplicationAreaState[] values) : base(values)
        {
        }
    }
}
