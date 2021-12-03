using System;
using Appalachia.Core.Collections;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Menus.Collections
{
    [Serializable]
    public sealed class AppaList_Selectable : AppaList<Selectable>
    {
        public AppaList_Selectable()
        {
        }

        public AppaList_Selectable(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_Selectable(AppaList<Selectable> list) : base(list)
        {
        }

        public AppaList_Selectable(Selectable[] values) : base(values)
        {
        }
    }
}