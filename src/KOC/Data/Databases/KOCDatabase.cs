using System;
using Appalachia.Data.Core.AccessLayer;
using Appalachia.Data.Core.Databases;
using Appalachia.Prototype.KOC.Data.Configuration;

namespace Appalachia.Prototype.KOC.Data.Databases
{
    [Serializable]
    public abstract class KOCDatabase<TDB> : AppaDatabase<TDB>
        where TDB : KOCDatabase<TDB>, new()
    {
        #region Fields and Autoproperties

        public DatabaseConfiguration Configuration { get; protected set; }

        #endregion

        internal static TDB CreateDatabase(DatabaseConfiguration configuration, DatabaseAccess access)
        {
            var instance = CreateDatabase(access);
            instance.Configuration = configuration;

            return instance;
        }
    }
}
