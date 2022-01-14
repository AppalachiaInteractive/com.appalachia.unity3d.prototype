using System;
using Appalachia.Core.Collections;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Components.Menus.Collections
{
    [Serializable]
    public sealed class GraphicsList : AppaList<Graphic>
    {
        public GraphicsList()
        {
        }

        public GraphicsList(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) :
            base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public GraphicsList(AppaList<Graphic> list) : base(list)
        {
        }

        public GraphicsList(Graphic[] values) : base(values)
        {
        }
    }
}
