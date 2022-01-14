using System;
using Appalachia.Core.Collections;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Components.Menus.Collections
{
    [Serializable]
    public sealed class SelectablesList : AppaList<Selectable>
    {
        public SelectablesList()
        {
        }

        public SelectablesList(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) :
            base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public SelectablesList(AppaList<Selectable> list) : base(list)
        {
        }

        public SelectablesList(Selectable[] values) : base(values)
        {
        }
    }
}
