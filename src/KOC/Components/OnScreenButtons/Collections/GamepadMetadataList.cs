using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Components.OnScreenButtons.Devices;

namespace Appalachia.Prototype.KOC.Components.OnScreenButtons.Collections
{
    [Serializable]
    public sealed class GamepadMetadataList : AppaList<GamepadMetadata>
    {
        public GamepadMetadataList()
        {
        }

        public GamepadMetadataList(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public GamepadMetadataList(AppaList<GamepadMetadata> list) : base(list)
        {
        }

        public GamepadMetadataList(GamepadMetadata[] values) : base(values)
        {
        }
    }
}
