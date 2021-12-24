using System;
using Appalachia.Core.Collections;

namespace Appalachia.Prototype.KOC.Application.Scenes.Collections
{
    [Serializable]
    public sealed class SceneReferenceElementList : AppaList<SceneReferenceElement>
    {
        public SceneReferenceElementList()
        {
        }

        public SceneReferenceElementList(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public SceneReferenceElementList(AppaList<SceneReferenceElement> list) : base(list)
        {
        }

        public SceneReferenceElementList(SceneReferenceElement[] values) : base(values)
        {
        }
    }
}
