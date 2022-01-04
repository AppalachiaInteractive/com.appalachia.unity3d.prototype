using System;
using Appalachia.Data.Core;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Data.Databases
{
    [Serializable]
    public class MetadataDatabase : KOCDatabase<MetadataDatabase>
    {
        public override DatabaseType Type => DatabaseType.Metadata;

        protected override void Dispose(bool disposing)
        {
            using (_PRF_Dispose.Auto())
            {
                base.Dispose(true);

                if (disposing)
                {
                }
            }
        }

        protected override void RegisterCollections()
        {
        }

        #region Profiling

        private const string _PRF_PFX = nameof(MetadataDatabase) + ".";
        private static readonly ProfilerMarker _PRF_Dispose = new ProfilerMarker(_PRF_PFX + nameof(Dispose));

        #endregion
    }
}
