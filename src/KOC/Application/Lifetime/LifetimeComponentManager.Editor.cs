#if UNITY_EDITOR
using Appalachia.Editing.Debugging.Testing;
using Appalachia.Spatial.MeshBurial.Processing;
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

        private const string GROUP_EDITORONLY = GROUP_BASE + PARENT_NAME_EDITORONLY;

        private const string PARENT_NAME_EDITORONLY = "Editor Only";

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup(GROUP_EDITORONLY)]
        [SerializeField]
        private Bazooka _bazooka;

        [FoldoutGroup(GROUP_EDITORONLY)]
        [SerializeField]
        private MeshBurialExecutionManager _meshBurialExecutionManager;

        [FoldoutGroup(GROUP_EDITORONLY), SerializeField]
        private GameObject _editorObject;

        public AppaEvent<Bazooka>.Data BazookaReady;
        public AppaEvent<MeshBurialExecutionManager>.Data MeshBurialExecutionManagerReady;

        #endregion

        public Bazooka Bazooka => _bazooka;

        public GameObject EditorObject => _editorObject;

        public MeshBurialExecutionManager MeshBurialExecutionManager => _meshBurialExecutionManager;

        private void InitializeEditorOnly()
        {
            using (_PRF_InitializeEditorOnly.Auto())
            {
                gameObject.GetOrAddChild(ref _editorObject, PARENT_NAME_EDITORONLY, false);

                _editorObject.GetOrAddLifetimeComponentInChild(
                    ref _meshBurialExecutionManager,
                    nameof(MeshBurialExecutionManager)
                );
                _editorObject.GetOrAddLifetimeComponentInChild(ref _bazooka, nameof(Bazooka));
                
                BazookaReady.RaiseEvent(Bazooka);
                MeshBurialExecutionManagerReady.RaiseEvent(MeshBurialExecutionManager);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitializeEditorOnly =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeEditorOnly));

        #endregion
    }
}

#endif
