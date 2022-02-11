using System;
using Appalachia.Core.Collections;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer.Models
{
    [Serializable]
    public sealed class ColorList : AppaList<Color>
    {
        public ColorList()
        {
        }

        public ColorList(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public ColorList(AppaList<Color> list) : base(list)
        {
        }

        public ColorList(Color[] values) : base(values)
        {
        }
    }
}
