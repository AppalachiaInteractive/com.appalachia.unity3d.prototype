using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Menus.Metadata;
using Appalachia.Prototype.KOC.Application.Menus.Metadata.Groups;

namespace Appalachia.Prototype.KOC.Application.Menus.Collections
{
    [Serializable]
    public sealed class AppaList_UIMenuButtonMetadata : AppaList<UIMenuButtonMetadataGroup>
    {
        public AppaList_UIMenuButtonMetadata()
        {
        }

        public AppaList_UIMenuButtonMetadata(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_UIMenuButtonMetadata(AppaList<UIMenuButtonMetadataGroup> list) : base(list)
        {
        }

        public AppaList_UIMenuButtonMetadata(UIMenuButtonMetadataGroup[] values) : base(values)
        {
        }
    }
}