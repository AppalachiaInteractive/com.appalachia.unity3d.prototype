using System;
using Appalachia.Core.Collections;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Menus.Collections
{
    [Serializable]
    public sealed class AppaList_Graphic : AppaList<Graphic>
    {
        public AppaList_Graphic()
        {
        }

        public AppaList_Graphic(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) :
            base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_Graphic(AppaList<Graphic> list) : base(list)
        {
        }

        public AppaList_Graphic(Graphic[] values) : base(values)
        {
        }
    }
}
