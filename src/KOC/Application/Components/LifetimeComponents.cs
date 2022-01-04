using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.CI.Integration.Core;
using Appalachia.Core.Attributes;
using Appalachia.Core.Execution.Hooks;
using Appalachia.Core.Objects.Root;
using Appalachia.Jobs.MeshData;
using Appalachia.Prototype.KOC.Application.Components.Cursors;
using Appalachia.Prototype.KOC.Application.Components.Fading;
using Appalachia.Prototype.KOC.Application.Extensions;
using Appalachia.Prototype.KOC.Application.Input;
using Appalachia.Prototype.KOC.Data;
using Appalachia.Rendering.Prefabs.Rendering;
using Appalachia.Simulation.Wind;
using Appalachia.Spatial.Terrains;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Doozy.Engine.Extensions;
using Doozy.Engine.UI;
using Doozy.Engine.UI.Base;
using GPUInstancer;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;
using UnityEngine.UI;
#if UNITY_EDITOR
#endif

namespace Appalachia.Prototype.KOC.Application.Components
{
    [Serializable, DoNotReorderFields]
    [CallStaticConstructorInEditor]
    public sealed partial class LifetimeComponents : AppalachiaBase<LifetimeComponents>
    {
        static LifetimeComponents()
        {
            RegisterDependency<LifetimeMetadata>(i => { _lifetimeMetadata = i; });
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
        private Canvas _canvas;

        [FormerlySerializedAs("_masterCanvas")]
        [FoldoutGroup("UI")]
        [SerializeField]
        private RectTransform _canvasRectTransform;

        [FormerlySerializedAs("_masterCanvas")]
        [FoldoutGroup("UI")]
        [SerializeField]
        private UICanvas _doozyMasterCanvas;

        [FoldoutGroup("UI")]
        [SerializeField]
        private Canvas _cursorCanvas;

        [FoldoutGroup("UI")]
        [SerializeField]
        private CanvasFadeManager _screenFader;

        [FoldoutGroup("UI")]
        [SerializeField]
        private Image _fullScreenBlackImage;

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
        public Canvas Canvas => _canvas;
        public CanvasFadeManager ScreenFader => _screenFader;
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

        public TerrainMetadataManager TerrainMetadataManager => _terrainMetadataManager;
        public UICanvas DoozyMasterCanvas => _doozyMasterCanvas;

        public async AppaTask Initialize(LifetimeComponentManager manager)
        {
            using (_PRF_Initialize.Auto())
            {
                var gameObject = manager.gameObject;

                Context.Log.Info(nameof(Initialize), gameObject);

                _gameObject = gameObject;

                try
                {
                    Initialize3DGraphics(gameObject);
                }
                catch (Exception ex)
                {
                    Context.Log.Error($"Exception in {nameof(Initialize3DGraphics)}.", this, ex);
                    throw;
                }

                try
                {
                    InitializeSystems(gameObject);
                }
                catch (Exception ex)
                {
                    Context.Log.Error($"Exception in {nameof(InitializeSystems)}.", this, ex);
                    throw;
                }

                try
                {
                    InitializeUI(gameObject);
                }
                catch (Exception ex)
                {
                    Context.Log.Error($"Exception in {nameof(InitializeUI)}.", this, ex);
                    throw;
                }

                try
                {
                    InitializeEventsAndInput(gameObject, manager);
                }
                catch (Exception ex)
                {
                    Context.Log.Error($"Exception in {nameof(InitializeEventsAndInput)}.", this, ex);
                    throw;
                }

                try
                {
                    InitializeMeshes(gameObject);
                }
                catch (Exception ex)
                {
                    Context.Log.Error($"Exception in {nameof(InitializeMeshes)}.", this, ex);
                    throw;
                }

                try
                {
                    InitializeWorld(gameObject);
                }
                catch (Exception ex)
                {
                    Context.Log.Error($"Exception in {nameof(InitializeWorld)}.", this, ex);
                    throw;
                }

                try
                {
                    InitializeEditorOnly(gameObject);
                }
                catch (Exception ex)
                {
                    Context.Log.Error($"Exception in {nameof(InitializeEditorOnly)}.", this, ex);
                    throw;
                }

                await AppaTask.CompletedTask;
            }
        }

        public void RunAsMainScene(GameObject gameObject)
        {
            SetFlags(gameObject, ApplicationManager.MAIN_SCENE_FLAGS);
        }

        public void RunAsSubScene(GameObject gameObject)
        {
            SetFlags(gameObject, ApplicationManager.SUBSCENE_FLAGS);
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

        private void Initialize3DGraphics(GameObject gameObject)
        {
            using (_PRF_Initialize3DGraphics.Auto())
            {
                gameObject.GetOrCreateComponentInChild(ref _postProcessVolume, nameof(PostProcessVolume));

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

                gameObject.GetOrCreateLifetimeComponentInChild(
                    ref _prefabRenderingManager,
                    nameof(PrefabRenderingManager)
                );
                gameObject.GetOrCreateLifetimeComponentInChild(
                    ref _gpuInstancerPrefabManager,
                    nameof(GpuInstancerPrefabManager)
                );

                _prefabRenderingManager.enabled = false;
                _gpuInstancerPrefabManager.enabled = false;
            }
        }

        private void InitializeEventsAndInput(GameObject gameObject, LifetimeComponentManager manager)
        {
            using (_PRF_InitializeEventsAndInput.Auto())
            {
                gameObject.GetOrCreateLifetimeComponentInChild(ref _eventSystem, nameof(EventSystem));

                _eventSystem.gameObject.GetOrCreateComponent(ref _inputSystemUIInputModule);
                _eventSystem.gameObject.GetOrCreateComponent(ref _playerInput);

                _eventSystem.enabled = false;
                _eventSystem.enabled = true;

                _playerInput.enabled = false;
                _inputSystemUIInputModule.enabled = false;

                var actions = new KOCInputActions();
                var asset = actions.asset;

#if UNITY_EDITOR
                if (!AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    var inputActions = manager.GetActions();

                    _inputSystemUIInputModule.UnassignActions();
                    _inputSystemUIInputModule.actionsAsset = asset;
                    _inputSystemUIInputModule.point =
                        asset.FindAction(inputActions.GenericMenu.Point.id).GetReference();
                    _inputSystemUIInputModule.leftClick = asset
                                                         .FindAction(inputActions.GenericMenu.Click.id)
                                                         .GetReference();
                    _inputSystemUIInputModule.move = asset
                                                    .FindAction(inputActions.GenericMenu.Navigate.id)
                                                    .GetReference();
                    _inputSystemUIInputModule.submit = asset
                                                      .FindAction(inputActions.GenericMenu.Submit.id)
                                                      .GetReference();
                    _inputSystemUIInputModule.cancel = asset
                                                      .FindAction(inputActions.GenericMenu.Cancel.id)
                                                      .GetReference();
                }
#endif

                _inputSystemUIInputModule.enabled = true;

                _playerInput.uiInputModule = _inputSystemUIInputModule;
                _playerInput.actions = asset;
                _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
                _playerInput.neverAutoSwitchControlSchemes = false;
                _playerInput.enabled = true;
            }
        }

        private void InitializeMeshes(GameObject gameObject)
        {
            using (_PRF_InitializeMeshes.Auto())
            {
                gameObject.GetOrCreateLifetimeComponentInChild(
                    ref _meshObjectManager,
                    nameof(MeshObjectManager)
                );
            }
        }

        private void InitializeSystems(GameObject gameObject)
        {
            using (_PRF_InitializeSystems.Auto())
            {
                gameObject.GetOrCreateComponentInChild(ref _clearCamera, nameof(_clearCamera));
                gameObject.GetOrCreateLifetimeComponentInChild(ref _frameStart, nameof(FrameStart));
                gameObject.GetOrCreateLifetimeComponentInChild(ref _frameEnd,   nameof(FrameEnd));

                gameObject.GetOrCreateLifetimeComponentInChild(ref _audioListener,  nameof(AudioListener));
                gameObject.GetOrCreateLifetimeComponentInChild(ref _databasManager, nameof(DatabaseManager));

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
            }
        }

        private void InitializeUI(GameObject gameObject)
        {
            using (_PRF_InitializeUI.Auto())
            {
                gameObject.GetOrCreateComponentInChild(ref _doozyMasterCanvas, NamesDatabase.MASTER_CANVAS);

                _doozyMasterCanvas.DontDestroyCanvasOnLoad = false;

                _doozyMasterCanvas.gameObject.GetOrCreateComponent(ref _canvas);

                _canvas.renderMode = RenderMode.ScreenSpaceOverlay;

                _canvasRectTransform = _canvas.gameObject.GetComponent<RectTransform>();

                _doozyMasterCanvas.gameObject.GetOrCreateComponentInChild(
                    ref _screenFader,
                    "Master Canvas Screen Fader"
                );

                _screenFader.gameObject.GetOrCreateComponentInChild(
                    ref _fullScreenBlackImage,
                    "FULL_SCREEN_BLACK"
                );

                _fullScreenBlackImage.color = Color.black;

                _canvasRectTransform.FullScreen(true);
                _screenFader.rectTransform.FullScreen(true);
                _fullScreenBlackImage.rectTransform.FullScreen(true);

                _doozyMasterCanvas.gameObject.GetOrCreateLifetimeComponentInChild(
                    ref _cursorManager,
                    nameof(CursorManager)
                );

                _cursorManager.gameObject.GetOrCreateComponent(ref _cursorCanvas);
                _cursorManager.rectTransform.FullScreen(true);
            }
        }

        private void InitializeWorld(GameObject gameObject)
        {
            using (_PRF_InitializeTerrains.Auto())
            {
                gameObject.GetOrCreateLifetimeComponentInChild(
                    ref _terrainMetadataManager,
                    nameof(TerrainMetadataManager)
                );
                gameObject.GetOrCreateLifetimeComponentInChild(
                    ref _globalWindManager,
                    nameof(GlobalWindManager)
                );
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(LifetimeComponents) + ".";

        private static readonly ProfilerMarker _PRF_InitializeTerrains =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeWorld));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_InitializeMeshes =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeMeshes));

        private static readonly ProfilerMarker
            _PRF_SetFlags = new ProfilerMarker(_PRF_PFX + nameof(SetFlags));

        private static readonly ProfilerMarker _PRF_Initialize3DGraphics =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize3DGraphics));

        private static readonly ProfilerMarker _PRF_InitializeEventsAndInput =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeEventsAndInput));

        private static readonly ProfilerMarker _PRF_InitializeSystems =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeSystems));

        private static readonly ProfilerMarker _PRF_InitializeUI =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeUI));

        #endregion
    }
}
