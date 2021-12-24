using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Scenes;

namespace Appalachia.Prototype.KOC.Application.Collections
{
    [Serializable]
    public sealed class SceneReferenceList : AppaList<SceneReference>
    {
        public SceneReferenceList()
        {
        }

        public SceneReferenceList(int capacity, float capacityIncreaseMultiplier = 2, bool noTracking = false)
            : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public SceneReferenceList(AppaList<SceneReference> list) : base(list)
        {
        }

        public SceneReferenceList(SceneReference[] values) : base(values)
        {
        }
    }
}
