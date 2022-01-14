using System;
using Appalachia.Data.Core;
using Appalachia.Data.Core.AccessLayer;
using Appalachia.Prototype.KOC.Data.Configuration;
using Unity.Profiling;

namespace Appalachia.Prototype.KOC.Data.Databases
{
    [Serializable]
    public class GameStateDatabase : KOCDatabase<GameStateDatabase>
    {
        #region Fields and Autoproperties

        public long GameId { get; private set; }

        #endregion

        public override DatabaseType Type => DatabaseType.GameState;

        public static GameStateDatabase InitializeDatabase(
            DatabaseConfiguration configuration,
            DatabaseAccess access,
            long gameId)
        {
            var instance = InitializeDatabase(configuration, access);
            instance.GameId = gameId;
            return instance;
        }

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


        private static readonly ProfilerMarker _PRF_Dispose = new ProfilerMarker(_PRF_PFX + nameof(Dispose));

        #endregion
    }
}
