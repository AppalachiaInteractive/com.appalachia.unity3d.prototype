using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Components.Cursors.Metadata;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors.Collections
{
    [Serializable]
    public sealed class AppaList_CursorMetadata : AppaList<CursorMetadata>
    {
        public AppaList_CursorMetadata()
        {
        }

        public AppaList_CursorMetadata(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_CursorMetadata(AppaList<CursorMetadata> list) : base(list)
        {
        }

        public AppaList_CursorMetadata(CursorMetadata[] values) : base(values)
        {
        }
    }
}