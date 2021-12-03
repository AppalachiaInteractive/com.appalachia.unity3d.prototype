using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Scenes;

namespace Appalachia.Prototype.KOC.Application.Collections
{
    [Serializable]
    public sealed class AppaList_AreaSceneInformation : AppaList<AreaSceneInformation>
    {
        public AppaList_AreaSceneInformation()
        {
        }

        public AppaList_AreaSceneInformation(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_AreaSceneInformation(AppaList<AreaSceneInformation> list) : base(list)
        {
        }

        public AppaList_AreaSceneInformation(AreaSceneInformation[] values) : base(values)
        {
        }
    }
}
