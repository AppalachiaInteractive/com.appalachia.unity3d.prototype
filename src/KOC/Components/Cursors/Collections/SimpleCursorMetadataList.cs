using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Components.Cursors.Metadata;

namespace Appalachia.Prototype.KOC.Components.Cursors.Collections
{
    [Serializable]
    public sealed class SimpleCursorMetadataList : AppaList<SimpleCursorMetadata>
    {
        public SimpleCursorMetadataList()
        {
        }

        public SimpleCursorMetadataList(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public SimpleCursorMetadataList(AppaList<SimpleCursorMetadata> list) : base(list)
        {
        }

        public SimpleCursorMetadataList(SimpleCursorMetadata[] values) : base(values)
        {
        }
    }
}
