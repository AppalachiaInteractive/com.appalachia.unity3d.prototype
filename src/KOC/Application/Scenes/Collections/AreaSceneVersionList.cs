using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Areas;

namespace Appalachia.Prototype.KOC.Application.Scenes.Collections
{
    [Serializable]
    public sealed class AreaSceneVersionList : AppaList<AreaVersion>
    {
        public AreaSceneVersionList()
        {
        }

        public AreaSceneVersionList(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AreaSceneVersionList(AppaList<AreaVersion> list) : base(list)
        {
        }

        public AreaSceneVersionList(AreaVersion[] values) : base(values)
        {
        }
    }
}
