using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Scenes;

namespace Appalachia.Prototype.KOC.Application.Collections
{
    [Serializable]
    public sealed class AppaList_SceneBootloadData : AppaList<SceneBootloadData>
    {
        public AppaList_SceneBootloadData()
        {
        }

        public AppaList_SceneBootloadData(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false) : base(
            capacity,
            capacityIncreaseMultiplier,
            noTracking
        )
        {
        }

        public AppaList_SceneBootloadData(AppaList<SceneBootloadData> list) : base(list)
        {
        }

        public AppaList_SceneBootloadData(SceneBootloadData[] values) : base(values)
        {
        }
    }
    
}