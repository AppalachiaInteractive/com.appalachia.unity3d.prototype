using System;
using Appalachia.Core.Collections;

namespace Appalachia.Prototype.KOC.Application.Scenes
{
    [Serializable]
    public sealed class AppaList_BootloadedScene : AppaList<BootloadedScene>
    {
        public AppaList_BootloadedScene()
        {
        }

        public AppaList_BootloadedScene(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_BootloadedScene(AppaList<BootloadedScene> list) : base(list)
        {
        }

        public AppaList_BootloadedScene(BootloadedScene[] values) : base(values)
        {
        }
    }
}
