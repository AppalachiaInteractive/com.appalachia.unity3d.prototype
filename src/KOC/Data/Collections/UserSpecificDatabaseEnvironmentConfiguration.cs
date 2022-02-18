using System;
using Appalachia.Core.Collections.Context;
using Appalachia.Prototype.KOC.Data.Configuration;
using Appalachia.Utility.Colors;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Data.Collections
{
    [Serializable]
    public class UserSpecificDatabaseEnvironmentConfiguration : UserSpecific<DatabaseEnvironmentConfiguration,
        AppaList_DatabaseEnvironmentConfiguration>
    {
        /// <inheritdoc />
        protected override Color GetDisplayColor(string key, DatabaseEnvironmentConfiguration value)
        {
            return Colors.WhiteSmokeGray96;
        }

        /// <inheritdoc />
        protected override string GetDisplaySubtitle(string key, DatabaseEnvironmentConfiguration value)
        {
            return key;
        }

        /// <inheritdoc />
        protected override string GetDisplayTitle(string key, DatabaseEnvironmentConfiguration value)
        {
            if (value != null)
            {
                return value.name;
            }

            return "NULL";
        }
    }
}
