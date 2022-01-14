using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Components.Menus.Metadata.Groups;

namespace Appalachia.Prototype.KOC.Components.Menus.Collections
{
    [Serializable]
    public sealed class UIMenuButtonMetadataList : AppaList<UIMenuButtonMetadataGroup>
    {
        public UIMenuButtonMetadataList()
        {
        }

        public UIMenuButtonMetadataList(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public UIMenuButtonMetadataList(AppaList<UIMenuButtonMetadataGroup> list) : base(list)
        {
        }

        public UIMenuButtonMetadataList(UIMenuButtonMetadataGroup[] values) : base(values)
        {
        }
    }
}
