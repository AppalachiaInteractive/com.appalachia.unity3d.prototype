using System;
using Appalachia.Core.Collections;

namespace Appalachia.Prototype.KOC.Application.Scenes.Collections
{
    [Serializable]
    public sealed class BootloadedSceneList : AppaList<BootloadedScene>
    {
        public BootloadedSceneList()
        {
        }

        public BootloadedSceneList(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public BootloadedSceneList(AppaList<BootloadedScene> list) : base(list)
        {
        }

        public BootloadedSceneList(BootloadedScene[] values) : base(values)
        {
        }
    }
}
