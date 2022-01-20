using System;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Attributes;
using Appalachia.CI.Integration.Core;
using Appalachia.Core.Attributes;
using Appalachia.Core.Execution;
using Appalachia.Core.Execution.Hooks;
using Appalachia.Core.Objects.Root;
using Appalachia.Jobs.MeshData;
using Appalachia.Prototype.KOC.Data;
using Appalachia.Rendering.Prefabs.Rendering;
using Appalachia.Simulation.Wind;
using Appalachia.Spatial.Terrains;
using Appalachia.UI.Controls.Cursors;
using Appalachia.UI.Controls.Sets.Background;
using Appalachia.UI.Controls.Sets.RootCanvas;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using GPUInstancer;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering.PostProcessing;

namespace Appalachia.Prototype.KOC.Components
{
    [Serializable, DoNotReorderFields]
    [CallStaticConstructorInEditor]
    public sealed partial class LifetimeComponents : AppalachiaBase<LifetimeComponents>
    {
        #region Constants and Static Readonly

        private const string PARENT_NAME_EDITORONLY = "Editor Only";
        private const string PARENT_NAME_EVENTSANDINPUT = "Events And Input";
        private const string PARENT_NAME_GRAPHICS = "Graphics";
        private const string PARENT_NAME_MESHES = "Meshes";
        private const string PARENT_NAME_SYSTEMS = "Systems";
        private const string PARENT_NAME_UI = "UI";
        private const string PARENT_NAME_WORLD = "World";

        #endregion

        static LifetimeComponents()
        {
            RegisterDependency<LifetimeMetadata>(i => { _lifetimeMetadata = i; });
        }

        public LifetimeComponents()
        {
        }

        public LifetimeComponents(LifetimeComponentManager owner) : base(owner)
        {
        }

        #region Static Fields and Autoproperties

        [FoldoutGroup("Metadata")]
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        [ShowInInspector]
        private static LifetimeMetadata _lifetimeMetadata;

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup("UI")]
        [SerializeField]
        private RootCanvasComponentSet _rootCanvas;

        [FoldoutGroup("UI")]
        [SerializeField]
        private BackgroundComponentSet _rootBackground;

        [FoldoutGroup("World")]
        [SerializeField]
        private TerrainMetadataManager _terrainMetadataManager;

        [FoldoutGroup("World")]
        [SerializeField]
        private GlobalWindManager _globalWindManager;

        [FoldoutGroup("Graphics")]
        [SerializeField]
        private PostProcessVolume _postProcessVolume;

        [FoldoutGroup("Graphics")]
        [SerializeField]
        private PrefabRenderingManager _prefabRenderingManager;

        [FoldoutGroup("Graphics")]
        [SerializeField]
        private GPUInstancerPrefabManager _gpuInstancerPrefabManager;

        [FoldoutGroup("Events & Input")]
        [SerializeField]
        private EventSystem _eventSystem;

        [FoldoutGroup("Events & Input")]
        [SerializeField]
        private InputSystemUIInputModule _inputSystemUIInputModule;

        [FoldoutGroup("Events & Input")]
        [SerializeField]
        private PlayerInput _playerInput;

        [FoldoutGroup("Systems")]
        [SerializeField]
        private CleanupManager _cleanupManager;

        [FoldoutGroup("Systems")]
        [SerializeField]
        private CursorManager _cursorManager;

        [FoldoutGroup("Systems")]
        [SerializeField]
        private DatabaseManager _databasManager;

        [FoldoutGroup("Systems")]
        [SerializeField]
        private AudioListener _audioListener;

        [FoldoutGroup("Systems")]
        [SerializeField]
        private Camera _clearCamera;

        [FoldoutGroup("Systems")]
        [SerializeField]
        private FrameEnd _frameEnd;

        [FoldoutGroup("Systems")]
        [SerializeField]
        private FrameStart _frameStart;

        private GameObject _gameObject;

        [SerializeField] private AppalachiaRepository _repository;

        [FoldoutGroup("Meshes")]
        [SerializeField]
        private MeshObjectManager _meshObjectManager;

        #endregion

        public AudioListener AudioListener => _audioListener;
        public CleanupManager CleanupManager => _cleanupManager;
        public CursorManager CursorManager => _cursorManager;
        public DatabaseManager DatabasManager => _databasManager;
        public EventSystem EventSystem => _eventSystem;
        public FrameEnd FrameEnd => _frameEnd;
        public FrameStart FrameStart => _frameStart;
        public GlobalWindManager GlobalWindManager => _globalWindManager;
        public GPUInstancerPrefabManager GpuInstancerPrefabManager => _gpuInstancerPrefabManager;
        public InputSystemUIInputModule InputSystemUIInputModule => _inputSystemUIInputModule;
        public LifetimeMetadata LifetimeMetadata => _lifetimeMetadata;

        public MeshObjectManager MeshObjectManager => _meshObjectManager;
        public PlayerInput PlayerInput => _playerInput;
        public PostProcessVolume PostProcessVolume => _postProcessVolume;
        public PrefabRenderingManager PrefabRenderingManager => _prefabRenderingManager;
        public RootCanvasComponentSet RootCanvas => _rootCanvas;

        public TerrainMetadataManager TerrainMetadataManager => _terrainMetadataManager;

        public async AppaTask Initialize(LifetimeComponentManager manager)
        {
            var gameObject = manager.gameObject;

            Context.Log.Info(nameof(Initialize), gameObject);

            await AppaTask.WaitUntil(() => DependenciesAreReady);

            _gameObject = gameObject;

            using (_PRF_Initialize.Auto())
            {
                GameObject graphicsObject = null;
                GameObject systemsObject = null;
                GameObject uiObject = null;
                GameObject eventsObject = null;
                GameObject meshesObject = null;
                GameObject worldObject = null;
                GameObject editorObject = null;

                try
                {
                    gameObject.GetOrAddChild(ref graphicsObject, PARENT_NAME_GRAPHICS, false);
                    InitializeGraphics(graphicsObject);
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        nameof(InitializeGraphics).GenericMethodException(manager),
                        manager,
                        ex
                    );
                    throw;
                }

                try
                {
                    gameObject.GetOrAddChild(ref systemsObject, PARENT_NAME_SYSTEMS, false);
                    InitializeSystems(systemsObject);
                }
                catch (Exception ex)
                {
                    Context.Log.Error(nameof(InitializeSystems).GenericMethodException(manager), manager, ex);
                    throw;
                }

                try
                {
                    gameObject.GetOrAddChild(ref uiObject, PARENT_NAME_UI, false);
                    InitializeUI(uiObject);
                }
                catch (Exception ex)
                {
                    Context.Log.Error(nameof(InitializeUI).GenericMethodException(manager), manager, ex);
                    throw;
                }

                try
                {
                    gameObject.GetOrAddChild(ref eventsObject, PARENT_NAME_EVENTSANDINPUT, false);
                    InitializeEventsAndInput(eventsObject, manager);
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        nameof(InitializeEventsAndInput).GenericMethodException(manager),
                        manager,
                        ex
                    );
                    throw;
                }

                try
                {
                    gameObject.GetOrAddChild(ref meshesObject, PARENT_NAME_MESHES, false);
                    InitializeMeshes(meshesObject);
                }
                catch (Exception ex)
                {
                    Context.Log.Error(nameof(InitializeMeshes).GenericMethodException(manager), manager, ex);
                    throw;
                }

                try
                {
                    gameObject.GetOrAddChild(ref worldObject, PARENT_NAME_WORLD, true);
                    InitializeWorld(worldObject);
                }
                catch (Exception ex)
                {
                    Context.Log.Error(nameof(InitializeWorld).GenericMethodException(manager), manager, ex);
                    throw;
                }

#if UNITY_EDITOR
                try
                {
                    gameObject.GetOrAddChild(ref editorObject, PARENT_NAME_EDITORONLY, false);
                    InitializeEditorOnly(editorObject);
                }
                catch (Exception ex)
                {
                    Context.Log.Error(
                        nameof(InitializeEditorOnly).GenericMethodException(manager),
                        manager,
                        ex
                    );
                    throw;
                }
#endif

                systemsObject.transform.SetSiblingIndex(0);
                worldObject.transform.SetSiblingIndex(1);
                meshesObject.transform.SetSiblingIndex(2);
                eventsObject.transform.SetSiblingIndex(3);
                graphicsObject.transform.SetSiblingIndex(4);
                uiObject.transform.SetSiblingIndex(5);

#if UNITY_EDITOR
                editorObject.transform.SetAsLastSibling();
#endif
            }

            await AppaTask.CompletedTask;
        }

        public void RunAsMainScene(GameObject gameObject)
        {
            using (_PRF_RunAsMainScene.Auto())
            {
                SetFlags(gameObject, ApplicationManager.MAIN_SCENE_FLAGS);
            }
        }

        public void RunAsSubScene(GameObject gameObject)
        {
            using (_PRF_RunAsSubScene.Auto())
            {
                SetFlags(gameObject, ApplicationManager.SUBSCENE_FLAGS);
            }
        }

        private static void SetFlags(GameObject gameObject, HideFlags flags)
        {
            using (_PRF_SetFlags.Auto())
            {
                if (gameObject.hideFlags != flags)
                {
                    gameObject.hideFlags = flags;
                }
            }
        }

        private void InitializeEventsAndInput(GameObject gameObject, LifetimeComponentManager manager)
        {
            using (_PRF_InitializeEventsAndInput.Auto())
            {
                gameObject.GetOrAddLifetimeComponentInChild(ref _eventSystem, nameof(EventSystem));

                _eventSystem.gameObject.GetOrAddComponent(ref _inputSystemUIInputModule);
                _eventSystem.gameObject.GetOrAddComponent(ref _playerInput);

                _eventSystem.enabled = false;
                _eventSystem.enabled = true;

                _playerInput.enabled = false;
                _inputSystemUIInputModule.enabled = false;

                var actions = manager.GetActions();
                var asset = actions.asset;

#if UNITY_EDITOR
                InitializeEventsAndInputEditor(asset, actions);
#endif

                _inputSystemUIInputModule.enabled = true;

                _playerInput.uiInputModule = _inputSystemUIInputModule;
                _playerInput.actions = asset;
                _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
                _playerInput.neverAutoSwitchControlSchemes = false;
                _playerInput.enabled = true;
            }
        }

        private void InitializeGraphics(GameObject gameObject)
        {
            using (_PRF_Initialize3DGraphics.Auto())
            {
                gameObject.GetOrAddComponentInChild(ref _postProcessVolume, nameof(PostProcessVolume));

                if (_postProcessVolume.profile == null)
                {
                    var profile = AppalachiaObjectFactory.LoadExistingOrCreateNewAsset<PostProcessProfile>(
                        "LifetimeGlobalProfile",
                        ownerType: typeof(LifetimeComponentManager)
                    );

                    _postProcessVolume.profile = profile;
                }

                _postProcessVolume.isGlobal = true;
                _postProcessVolume.weight = 1.0f;

                gameObject.GetOrAddLifetimeComponentInChild(
                    ref _prefabRenderingManager,
                    nameof(PrefabRenderingManager)
                );
                gameObject.GetOrAddLifetimeComponentInChild(
                    ref _gpuInstancerPrefabManager,
                    nameof(GpuInstancerPrefabManager)
                );

                _prefabRenderingManager.enabled = false;
                _gpuInstancerPrefabManager.enabled = false;
            }
        }

        private void InitializeMeshes(GameObject gameObject)
        {
            using (_PRF_InitializeMeshes.Auto())
            {
                gameObject.GetOrAddLifetimeComponentInChild(
                    ref _meshObjectManager,
                    nameof(MeshObjectManager)
                );
            }
        }

        private void InitializeSystems(GameObject gameObject)
        {
            using (_PRF_InitializeSystems.Auto())
            {
                gameObject.GetOrAddComponentInChild(ref _clearCamera, nameof(_clearCamera));
                gameObject.GetOrAddLifetimeComponentInChild(ref _frameStart, nameof(FrameStart));
                gameObject.GetOrAddLifetimeComponentInChild(ref _frameEnd,   nameof(FrameEnd));

                gameObject.GetOrAddLifetimeComponentInChild(ref _audioListener,  nameof(AudioListener));
                gameObject.GetOrAddLifetimeComponentInChild(ref _databasManager, nameof(DatabaseManager));

                if (AppalachiaApplication.IsPlaying)
                {
                    _clearCamera.enabled = _lifetimeMetadata.clearCamera.enabled;
                    _clearCamera.backgroundColor = _lifetimeMetadata.clearCamera.color; //33322B
                }
                else
                {
                    _clearCamera.enabled = _lifetimeMetadata.clearCamera.enabledEditor;
                    _clearCamera.backgroundColor = _lifetimeMetadata.clearCamera.colorEditor; //33322B
                }

                _clearCamera.clearFlags = CameraClearFlags.SolidColor;
                _clearCamera.cullingMask = 0;

                gameObject.GetOrAddLifetimeComponentInChild(ref _cleanupManager, nameof(CleanupManager));
            }
        }

        private void InitializeUI(GameObject gameObject)
        {
            using (_PRF_InitializeUI.Auto())
            {
                _lifetimeMetadata.rootCanvas.PrepareAndConfigure(
                    ref _rootCanvas,
                    gameObject,
                    APPASTR.ObjectNames.Master_Canvas
                );

                _lifetimeMetadata.rootBackground.PrepareAndConfigure(
                    ref _rootBackground,
                    _rootCanvas.GameObject,
                    APPASTR.Background
                );

                _rootCanvas.GameObject.GetOrAddLifetimeComponentInChild(
                    ref _cursorManager,
                    nameof(CursorManager)
                );
            }
        }

        private void InitializeWorld(GameObject gameObject)
        {
            using (_PRF_InitializeTerrains.Auto())
            {
                gameObject.GetOrAddLifetimeComponentInChild(
                    ref _terrainMetadataManager,
                    nameof(TerrainMetadataManager)
                );
                gameObject.GetOrAddLifetimeComponentInChild(
                    ref _globalWindManager,
                    nameof(GlobalWindManager)
                );
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitializeMeshes =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeMeshes));

        private static readonly ProfilerMarker _PRF_InitializeTerrains =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeWorld));

        private static readonly ProfilerMarker _PRF_RunAsMainScene =
            new ProfilerMarker(_PRF_PFX + nameof(RunAsMainScene));

        private static readonly ProfilerMarker _PRF_RunAsSubScene =
            new ProfilerMarker(_PRF_PFX + nameof(RunAsSubScene));

        private static readonly ProfilerMarker
            _PRF_SetFlags = new ProfilerMarker(_PRF_PFX + nameof(SetFlags));

        private static readonly ProfilerMarker _PRF_Initialize3DGraphics =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeGraphics));

        private static readonly ProfilerMarker _PRF_InitializeEventsAndInput =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeEventsAndInput));

        private static readonly ProfilerMarker _PRF_InitializeSystems =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeSystems));

        private static readonly ProfilerMarker _PRF_InitializeUI =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeUI));

        #endregion
    }
}
