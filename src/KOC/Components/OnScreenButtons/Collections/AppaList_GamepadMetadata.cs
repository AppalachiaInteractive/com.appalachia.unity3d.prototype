using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Components.OnScreenButtons.Devices;

namespace Appalachia.Prototype.KOC.Components.OnScreenButtons.Collections
{
    [Serializable]
    public sealed class AppaList_GamepadMetadata : AppaList<GamepadMetadata>
    {
        public AppaList_GamepadMetadata()
        {
        }

        public AppaList_GamepadMetadata(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_GamepadMetadata(AppaList<GamepadMetadata> list) : base(list)
        {
        }

        public AppaList_GamepadMetadata(GamepadMetadata[] values) : base(values)
        {
        }
    }
}
