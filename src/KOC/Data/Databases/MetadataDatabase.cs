using System;
using Appalachia.Data.Core;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Data.Databases
{
    [Serializable]
    public class MetadataDatabase : KOCDatabase<MetadataDatabase>
    {
        /// <inheritdoc />
        public override DatabaseType Type => DatabaseType.Metadata;

        /// <inheritdoc />
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

        /// <inheritdoc />
        protected override void RegisterCollections()
        {
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_Dispose = new ProfilerMarker(_PRF_PFX + nameof(Dispose));

        #endregion
    }
}
