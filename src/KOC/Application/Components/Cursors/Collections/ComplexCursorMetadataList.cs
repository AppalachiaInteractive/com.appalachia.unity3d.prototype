using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Components.Cursors.Metadata;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors.Collections
{
    [Serializable]
    public sealed class ComplexCursorMetadataList : AppaList<ComplexCursorMetadata>
    {
        public ComplexCursorMetadataList()
        {
        }

        public ComplexCursorMetadataList(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public ComplexCursorMetadataList(AppaList<ComplexCursorMetadata> list) : base(list)
        {
        }

        public ComplexCursorMetadataList(ComplexCursorMetadata[] values) : base(values)
        {
        }
    }
}
