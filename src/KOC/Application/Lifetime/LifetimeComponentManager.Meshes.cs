using Appalachia.Jobs.MeshData;
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

        private const string GROUP_MESHES = GROUP_BASE + PARENT_NAME_MESHES;

        private const string PARENT_NAME_MESHES = "Meshes";

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup(GROUP_MESHES)]
        [SerializeField]
        private MeshObjectManager _meshObjectManager;

        [FoldoutGroup(GROUP_MESHES), SerializeField]
        private GameObject _meshesObject;

        #endregion

        public GameObject MeshesObject => _meshesObject;
        public MeshObjectManager MeshObjectManager => _meshObjectManager;

        public AppaEvent<MeshObjectManager>.Data MeshObjectManagerReady;
        
        private void InitializeMeshes()
        {
            using (_PRF_InitializeMeshes.Auto())
            {
                gameObject.GetOrAddChild(ref _meshesObject, PARENT_NAME_MESHES, false);

                _meshesObject.GetOrAddLifetimeComponentInChild(
                    ref _meshObjectManager,
                    nameof(MeshObjectManager)
                );

                MeshObjectManagerReady.RaiseEvent(MeshObjectManager);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitializeMeshes =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeMeshes));

        #endregion
    }
}
