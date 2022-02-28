using Appalachia.Simulation.Wind;
using Appalachia.Spatial.Terrains;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime
{
    public partial class LifetimeComponentManager
    {
        #region Constants and Static Readonly

        private const string GROUP_WORLD = GROUP_BASE + PARENT_NAME_WORLD;

        private const string PARENT_NAME_WORLD = "World";

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup(GROUP_WORLD)]
        [SerializeField]
        private TerrainMetadataManager _terrainMetadataManager;

        [FoldoutGroup(GROUP_WORLD)]
        [SerializeField]
        private GlobalWindManager _globalWindManager;

        [FoldoutGroup(GROUP_WORLD), SerializeField]
        private GameObject _worldObject;

        public AppaEvent<GlobalWindManager>.Data GlobalWindManagerReady;
        public AppaEvent<TerrainMetadataManager>.Data TerrainMetadataManagerReady;

        #endregion

        public GameObject WorldObject => _worldObject;
        public GlobalWindManager GlobalWindManager => _globalWindManager;
        public TerrainMetadataManager TerrainMetadataManager => _terrainMetadataManager;

        private void InitializeWorld()
        {
            using (_PRF_InitializeWorld.Auto())
            {
                gameObject.GetOrAddChild(ref _worldObject, PARENT_NAME_WORLD, true);

                _worldObject.GetOrAddLifetimeComponentInChild(
                    ref _terrainMetadataManager,
                    nameof(Spatial.Terrains.TerrainMetadataManager)
                );
                _worldObject.GetOrAddLifetimeComponentInChild(
                    ref _globalWindManager,
                    nameof(Simulation.Wind.GlobalWindManager)
                );

                GlobalWindManagerReady.RaiseEvent(GlobalWindManager);
                TerrainMetadataManagerReady.RaiseEvent(TerrainMetadataManager);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitializeWorld =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeWorld));

        #endregion
    }
}
