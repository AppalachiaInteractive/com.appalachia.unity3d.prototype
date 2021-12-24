using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Data.Configuration;

namespace Appalachia.Prototype.KOC.Data.Collections
{
    [Serializable]
    public sealed class AppaList_DatabaseEnvironmentConfiguration : AppaList<DatabaseEnvironmentConfiguration>
    {
        public AppaList_DatabaseEnvironmentConfiguration()
        {
        }

        public AppaList_DatabaseEnvironmentConfiguration(
            int capacity,
            float capacityIncreaseMultiplier = 2,
            bool noTracking = false) : base(capacity, capacityIncreaseMultiplier, noTracking)
        {
        }

        public AppaList_DatabaseEnvironmentConfiguration(AppaList<DatabaseEnvironmentConfiguration> list) :
            base(list)
        {
        }

        public AppaList_DatabaseEnvironmentConfiguration(DatabaseEnvironmentConfiguration[] values) : base(
            values
        )
        {
        }
    }
}
