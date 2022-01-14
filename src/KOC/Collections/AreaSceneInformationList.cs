using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Scenes;

namespace Appalachia.Prototype.KOC.Collections
{
    [Serializable]
    public sealed class AreaSceneInformationList : AppaList<AreaSceneInformation>
    {
        public AreaSceneInformationList()
        {
        }

        public AreaSceneInformationList(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AreaSceneInformationList(AppaList<AreaSceneInformation> list) : base(list)
        {
        }

        public AreaSceneInformationList(AreaSceneInformation[] values) : base(values)
        {
        }
    }
}
