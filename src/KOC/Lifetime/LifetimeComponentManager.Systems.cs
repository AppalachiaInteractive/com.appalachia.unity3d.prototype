using Appalachia.Core.Execution;
using Appalachia.Core.Execution.Hooks;
using Appalachia.Prototype.KOC.Data;
using Appalachia.UI.Core.Components.Data;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime
{
    public partial class LifetimeComponentManager
    {
        #region Constants and Static Readonly

        private const string GROUP_SYSTEMS = GROUP_BASE + PARENT_NAME_SYSTEMS;

        private const string PARENT_NAME_SYSTEMS = "Systems";

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup(GROUP_SYSTEMS)]
        [SerializeField]
        private CleanupManager _cleanupManager;

        [FoldoutGroup(GROUP_SYSTEMS)]
        [SerializeField]
        private DatabaseManager _databasManager;

        [FoldoutGroup(GROUP_SYSTEMS)]
        [SerializeField]
        private AudioListener _audioListener;

        [FoldoutGroup(GROUP_SYSTEMS)]
        [SerializeField]
        private Camera _clearCamera;

        [FoldoutGroup(GROUP_SYSTEMS)]
        [SerializeField]
        private FrameEnd _frameEnd;

        [FoldoutGroup(GROUP_SYSTEMS)]
        [SerializeField]
        private FrameStart _frameStart;

        [FoldoutGroup(GROUP_SYSTEMS), SerializeField]
        private GameObject _systemsObject;

        #endregion

        public AudioListener AudioListener => _audioListener;
        public CleanupManager CleanupManager => _cleanupManager;
        public DatabaseManager DatabasManager => _databasManager;
        public FrameEnd FrameEnd => _frameEnd;
        public FrameStart FrameStart => _frameStart;

        public GameObject SystemsObject => _systemsObject;

        private void InitializeSystems()
        {
            using (_PRF_InitializeSystems.Auto())
            {
                gameObject.GetOrAddChild(ref _systemsObject, PARENT_NAME_SYSTEMS, false);

                _systemsObject.GetOrAddComponentInChild(ref _clearCamera, nameof(_clearCamera));
                _systemsObject.GetOrAddLifetimeComponentInChild(ref _frameStart, nameof(FrameStart));
                _systemsObject.GetOrAddLifetimeComponentInChild(ref _frameEnd,   nameof(FrameEnd));

                _systemsObject.GetOrAddLifetimeComponentInChild(ref _audioListener,  nameof(AudioListener));
                _systemsObject.GetOrAddLifetimeComponentInChild(ref _databasManager, nameof(DatabaseManager));

                CameraData.RefreshAndUpdateComponent(
                    ref _lifetimeMetadata.clearCameraData,
                    lifetimeMetadata,
                    _clearCamera
                );

                _systemsObject.GetOrAddLifetimeComponentInChild(ref _cleanupManager, nameof(CleanupManager));
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitializeSystems =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeSystems));

        #endregion
    }
}
