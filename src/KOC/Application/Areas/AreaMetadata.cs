using System.Collections.Generic;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Extensions;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Prototype.KOC.Application.Components.Cursors;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Graphs;
using Appalachia.Prototype.KOC.Application.Input;
using Appalachia.Prototype.KOC.Application.Scriptables;
using Appalachia.Utility.Extensions;
using Doozy.Engine.Extensions;
using Doozy.Engine.Nody;
using Doozy.Engine.Nody.Models;
using Doozy.Engine.UI;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Areas
{
    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [SmartLabelChildren]
    public abstract class AreaMetadata<T, TM> : SingletonAppalachiaApplicationObject<TM>, IAreaMetadata
        where T : AreaManager<T, TM>
        where TM : AreaMetadata<T, TM>
    {
        #region Constants and Static Readonly

        private const string FOLDOUT_GROUP = FOLDOUT_GROUP_ + "/";

        private const string FOLDOUT_GROUP_ = APPASTR.Common;

        #endregion

        #region Fields and Autoproperties

        [FormerlySerializedAs("hideCursor")]
        [FoldoutGroup(FOLDOUT_GROUP_)]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Cursor)]
        [ToggleLeft]
        [SerializeField]
        private bool _hideCursor;

        [FormerlySerializedAs("updateCursorLockState")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Cursor)]
        [ToggleLeft]
        [SerializeField]
        private bool _updateCursorLockState;

        [FormerlySerializedAs("cursorLockState")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Cursor)]
        [EnableIf(nameof(_updateCursorLockState))]
        [SerializeField]
        private CursorLockMode _cursorLockState;

        [FormerlySerializedAs("hasCustomCursor")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Cursor)]
        [DisableIf(nameof(_hideCursor))]
        [ToggleLeft]
        [SerializeField]
        private bool _hasCustomCursor;

        [FormerlySerializedAs("cursor")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Cursor)]
        [DisableIf(nameof(_hideCursor))]
        [EnableIf(nameof(_hasCustomCursor))]
        [SerializeField]
        private Cursors _cursor;

        [FormerlySerializedAs("_onEnable")]
        [FormerlySerializedAs("onEnable")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Input)]
        [SerializeField]
        private KOCInputActions.MapEnableState _onEnableMapState;

        [FormerlySerializedAs("_onDisable")]
        [FormerlySerializedAs("onDisable")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Input)]
        [SerializeField]
        private KOCInputActions.MapEnableState _onDisableMapState;

        [FormerlySerializedAs("blocksRaycasts")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Canvas_Group)]
        [ToggleLeft]
        [SerializeField]
        private bool _blocksRaycasts;

        [FormerlySerializedAs("interactable")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Canvas_Group)]
        [ToggleLeft]
        [SerializeField]
        private bool _interactable;

        [FormerlySerializedAs("ignoreParentGroups")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Canvas_Group)]
        [ToggleLeft]
        [SerializeField]
        private bool _ignoreParentGroups;

        [FormerlySerializedAs("uiScaleMode")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Canvas_Scaling)]
        [SerializeField]
        private CanvasScaler.ScaleMode _uiScaleMode;

        [FormerlySerializedAs("screenMatchMode")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Canvas_Scaling)]
        [SerializeField]
        private CanvasScaler.ScreenMatchMode _screenMatchMode;

        [FormerlySerializedAs("match")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Canvas_Scaling)]
        [Range(0f, 1f)]
        [SerializeField]
        private float _match;

        [FormerlySerializedAs("referenceResolution")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Canvas_Scaling)]
        [SerializeField]
        private Vector2 _referenceResolution;

        [FormerlySerializedAs("referencePixelsPerUnit")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Canvas_Scaling)]
        [Range(1f, 200f)]
        [SerializeField]
        private float _referencePixelsPerUnit;

        [FormerlySerializedAs("renderMode")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Canvas)]
        [SerializeField]
        private RenderMode _renderMode;

        [FormerlySerializedAs("additionalShaderChannels")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Canvas)]
        [SerializeField]
        private AdditionalCanvasShaderChannels _additionalShaderChannels;

        [FormerlySerializedAs("pixelPerfect")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Canvas)]
        [ToggleLeft]
        [SerializeField]
        private bool _pixelPerfect;

        [FormerlySerializedAs("sortingOrder")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Canvas)]
        [SerializeField]
        private int _sortingOrder;

        [FormerlySerializedAs("fullscreen")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.View)]
        [ToggleLeft]
        [SerializeField]
        private bool _fullscreen;

        [FormerlySerializedAs("resetScaleToOne")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.View), EnableIf(nameof(_fullscreen))]
        [ToggleLeft]
        [SerializeField]
        private bool _resetScaleToOne;

        [FormerlySerializedAs("blockingObjects")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Graphic_Raycaster)]
        [SerializeField]
        private GraphicRaycaster.BlockingObjects _blockingObjects;

        [FormerlySerializedAs("ignoreReversedGraphics")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Graphic_Raycaster)]
        [SerializeField]
        private bool _ignoreReversedGraphics;

        [FormerlySerializedAs("blockingMask")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Graphic_Raycaster)]
        [SerializeField]
        private LayerMask _blockingMask;

        [FormerlySerializedAs("graph")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Doozy_Graph)]
        [SerializeField]
        private Graph _graph;

        [FormerlySerializedAs("canvasName")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Doozy_Canvas)]
        [SerializeField]
        private string _canvasName;

        [FormerlySerializedAs("graphControllerName")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Doozy_Graph)]
        [SerializeField]
        private string _graphControllerName;

        [FormerlySerializedAs("viewCategory")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Doozy_View)]
        [SerializeField]
        private string _viewCategory;

        [FormerlySerializedAs("viewName")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Doozy_View)]
        [SerializeField]
        private string _viewName;

        [FormerlySerializedAs("doesDrawMenu")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Menu)]
        [ToggleLeft]
        [SerializeField]
        private bool _doesDrawMenu;

        [FormerlySerializedAs("templateEnabled")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Templates)]
        [ToggleLeft]
        [SerializeField]
        private bool _templateEnabled;

        [FormerlySerializedAs("templateAlpha")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Templates)]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float _templateAlpha;

        [FormerlySerializedAs("selectedTemplate")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Templates)]
        [ValueDropdown(nameof(_templates))]
        [SerializeField]
        private Sprite _selectedTemplate;

        [FormerlySerializedAs("templates")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Templates)]
        [SerializeField]
        private List<Sprite> _templates;

        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Default_References)]
        [SerializeField]
        private AreaMetadataDefaultReferences _defaultReferences;

        [FormerlySerializedAs("loadBehaviour")]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Scene_Behaviour)]
        [SerializeField]
        private LoadBehaviour _loadBehaviour;

        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Scene_Behaviour)]
        [ToggleLeft]
        [SerializeField]
        private bool _setEntrySceneActive;

        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Audio)]
        [SerializeField]
        internal AudioMixerGroup _mixerGroup;

        #endregion

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();
                AreaRegistry.RegisterMetadata(this);
                initializer.Reset(this, "2021-11-20a");

                var missingGraph = _graph == null;
                var missingTestGraph = (_graph != null) && _graph.IsSubGraph && (testGraph == null);

                initializer.Initialize(
                    this,
                    APPASTR.Doozy_Graph,
                    missingGraph || missingTestGraph,
                    () =>
                    {
                        var graphName = Area.ToString();
#if UNITY_EDITOR

                        if (missingGraph)
                        {
                            _graph = GenerateNodyGraph(graphName);
                        }

                        if (missingTestGraph)
                        {
                            testGraph = GenerateNodyGraph(graphName + "_TestWrapper");
                        }
#endif
                        _graphControllerName = $"Graph Controller - {graphName}";
                    }
                );

                initializer.Initialize(this, APPASTR.Doozy_Canvas, () => { _canvasName = Area.ToString(); });

                initializer.Initialize(
                    this,
                    APPASTR.Doozy_View,
                    () =>
                    {
                        var manager = GetManager();

                        if (manager != null)
                        {
                            _viewCategory = (manager.HasParent ? manager.ParentArea : Area).ToString();
                        }
                        else
                        {
                            _viewCategory = Area.ToString();
                        }

                        _viewName = Area.ToString();
                    }
                );

                initializer.Initialize(
                    this,
                    APPASTR.View,
                    () =>
                    {
                        _fullscreen = true;
                        _resetScaleToOne = true;
                    }
                );

                initializer.Initialize(
                    this,
                    APPASTR.Canvas_Group,
                    () =>
                    {
                        _interactable = true;
                        _blocksRaycasts = true;
                        _ignoreParentGroups = false;
                    }
                );

                initializer.Initialize(
                    this,
                    APPASTR.Canvas_Scaling,
                    () =>
                    {
                        _uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                        _referenceResolution = new Vector2(1920f, 1080f);
                        _screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                        _match = 1f;
                        _referencePixelsPerUnit = 100f;
                    }
                );

                initializer.Initialize(
                    this,
                    APPASTR.Canvas,
                    () =>
                    {
                        _renderMode = RenderMode.ScreenSpaceOverlay;
                        _pixelPerfect = false;
                        _sortingOrder = 0;
                        _additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 |
                                                    AdditionalCanvasShaderChannels.Normal |
                                                    AdditionalCanvasShaderChannels.Tangent;
                    }
                );

                initializer.Initialize(
                    this,
                    APPASTR.Graphic_Raycaster,
                    () =>
                    {
                        _blockingMask = 0;
                        _blockingObjects = GraphicRaycaster.BlockingObjects.None;
                        _ignoreReversedGraphics = false;
                    }
                );

                initializer.Initialize(this, nameof(_templateAlpha), () => { _templateAlpha = 1.0f; });

                initializer.Initialize(
                    this,
                    nameof(_defaultReferences),
                    _defaultReferences == null,
                    () => { _defaultReferences = AreaMetadataDefaultReferences.instance; }
                );
            }
        }

        protected void Apply(RectTransform target)
        {
            using (_PRF_Apply.Auto())
            {
                if (_fullscreen)
                {
                    target.FullScreen(_resetScaleToOne);
                    target.MarkAsModified();
                }
            }
        }

        protected void Apply(Canvas target)
        {
            using (_PRF_Apply.Auto())
            {
                target.renderMode = _renderMode;
                target.pixelPerfect = _pixelPerfect;
                target.sortingOrder = _sortingOrder;
                target.additionalShaderChannels = _additionalShaderChannels;
                target.MarkAsModified();
            }
        }

        protected void Apply(CanvasScaler target)
        {
            using (_PRF_Apply.Auto())
            {
                target.uiScaleMode = _uiScaleMode;
                target.referenceResolution = _referenceResolution;
                target.screenMatchMode = _screenMatchMode;
                target.matchWidthOrHeight = _match;
                target.referencePixelsPerUnit = _referencePixelsPerUnit;
                target.MarkAsModified();
            }
        }

        protected void Apply(CanvasGroup target)
        {
            using (_PRF_Apply.Auto())
            {
                target.interactable = _interactable;
                target.blocksRaycasts = _blocksRaycasts;
                target.ignoreParentGroups = _ignoreParentGroups;
                target.MarkAsModified();
            }
        }

        protected void Apply(GraphicRaycaster target)
        {
            using (_PRF_Apply.Auto())
            {
                target.blockingMask = _blockingMask;
                target.blockingObjects = _blockingObjects;
                target.ignoreReversedGraphics = _ignoreReversedGraphics;
                target.MarkAsModified();
            }
        }

        protected void Apply(UICanvas target)
        {
            using (_PRF_Apply.Auto())
            {
                target.CanvasName = _canvasName;
                target.CustomCanvasName = false;
                target.DontDestroyCanvasOnLoad = true;
                this.MarkAsModified();
            }
        }

        protected void Apply(UIView target)
        {
            using (_PRF_Apply.Auto())
            {
                target.ViewCategory = _viewCategory;
                target.ViewName = _viewName;
                target.ShowBehavior.PresetCategory = "Fade";
                target.HideBehavior.PresetCategory = "Fade";
                target.LoopBehavior.PresetCategory = "Fade";
                target.ShowBehavior.PresetName = "InNormal";
                target.HideBehavior.PresetName = "OutNormal";
                target.LoopBehavior.PresetName = "Normal";
                this.MarkAsModified();
            }
        }

        protected void Apply(GraphController target)
        {
            using (_PRF_Apply.Auto())
            {
                var manager = GetManager();

                if (manager.HasParent)
                {
                    _graph.IsSubGraph = true;

                    NodyGraphGenerator.GenerateTestGraph(target, this);
                }
                else
                {
                    _graph.IsSubGraph = false;
                    target.SetGraph(_graph);
                }

                target.ControllerName = _graphControllerName;
                target.MarkAsModified();
            }
        }

        private static Graph GenerateNodyGraph(string graphName)
        {
            using (_PRF_GenerateNodyGraph.Auto())
            {
                var saveLocationFolder =
                    AssetDatabaseManager.GetSaveDirectoryForOwnedAsset<ApplicationManager, Graph>(graphName);

                var saveLocation = AppaPath.Combine(saveLocationFolder, graphName) + ".asset";

                var tempGraph = CreateInstance<Graph>();
                var absolutePath = saveLocation.ToAbsolutePath();

                if (!AppaFile.Exists(absolutePath))
                {
                    AssetDatabaseManager.CreateAsset(tempGraph, saveLocation);
                }

                return AssetDatabaseManager.ImportAndLoadAssetAtPath<Graph>(saveLocation);
            }
        }

        private IAreaManager GetManager()
        {
            using (_PRF_GetManager.Auto())
            {
                return AreaRegistry.GetManager(Area);
            }
        }

        #region IAreaMetadata Members

        public bool HideCursor => _hideCursor;
        public bool UpdateCursorLockState => _updateCursorLockState;
        public CursorLockMode CursorLockState => _cursorLockState;
        public bool HasCustomCursor => _hasCustomCursor;
        public Cursors Cursor => _cursor;
        public KOCInputActions.MapEnableState OnEnableMapState => _onEnableMapState;
        public KOCInputActions.MapEnableState OnDisableMapState => _onDisableMapState;
        public bool BlocksRaycasts => _blocksRaycasts;
        public bool Interactable => _interactable;
        public bool IgnoreParentGroups => _ignoreParentGroups;
        public bool SetEntrySceneActive => _setEntrySceneActive;
        public CanvasScaler.ScaleMode UiScaleMode => _uiScaleMode;
        public CanvasScaler.ScreenMatchMode ScreenMatchMode => _screenMatchMode;
        public float Match => _match;
        public Vector2 ReferenceResolution => _referenceResolution;
        public float ReferencePixelsPerUnit => _referencePixelsPerUnit;
        public RenderMode RenderMode => _renderMode;
        public AdditionalCanvasShaderChannels AdditionalShaderChannels => _additionalShaderChannels;
        public bool PixelPerfect => _pixelPerfect;
        public int SortingOrder => _sortingOrder;
        public bool Fullscreen => _fullscreen;
        public bool ResetScaleToOne => _resetScaleToOne;
        public GraphicRaycaster.BlockingObjects BlockingObjects => _blockingObjects;
        public bool IgnoreReversedGraphics => _ignoreReversedGraphics;
        public LayerMask BlockingMask => _blockingMask;
        public Graph Graph => _graph;
        public string CanvasName => _canvasName;
        public string GraphControllerName => _graphControllerName;
        public string ViewCategory => _viewCategory;
        public string ViewName => _viewName;
        public bool DoesDrawMenu => _doesDrawMenu;
        public bool TemplateEnabled => _templateEnabled;
        public float TemplateAlpha => _templateAlpha;
        public Sprite SelectedTemplate => _selectedTemplate;
        public List<Sprite> Templates => _templates;
        public AreaMetadataDefaultReferences DefaultReferences => _defaultReferences;
        public LoadBehaviour LoadBehaviour => _loadBehaviour;

        public AudioMixerGroup MixerGroup => _mixerGroup;

        public abstract ApplicationArea Area { get; }

        public void Apply(UITemplateComponentSet target)
        {
            Initialize();

            Apply(target.rect);
            Apply(target.canvas);
            Apply(target.canvasGroup);

            target.image.enabled = _templateEnabled;
            target.image.sprite = _selectedTemplate;
            target.gameObject.MarkAsModified();
        }

        public void Apply(UIViewComponentSet target)
        {
            Initialize();

            Apply(target.rect);
            Apply(target.canvas);
            Apply(target.canvasGroup);
            Apply(target.graphicRaycaster);
            Apply(target.uiView);
            target.gameObject.MarkAsModified();
        }

        public void Apply(UICanvasAreaComponentSet target)
        {
            Initialize();

            Apply(target.rect);
            Apply(target.canvas);
            Apply(target.canvasGroup);
            Apply(target.canvasScaler);
            Apply(target.graphicRaycaster);
            Apply(target.uiCanvas);
            Apply(target.graphController);
            target.gameObject.MarkAsModified();
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(AreaMetadata<T, TM>) + ".";

        private static readonly ProfilerMarker _PRF_GenerateNodyGraph =
            new ProfilerMarker(_PRF_PFX + nameof(GenerateNodyGraph));

        private static readonly ProfilerMarker _PRF_GetManager =
            new ProfilerMarker(_PRF_PFX + nameof(GetManager));

        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));
        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion

#if UNITY_EDITOR
        private bool _showTestGraph => (_graph != null) && _graph.IsSubGraph;

        [ShowIf(nameof(_showTestGraph))]
        [FoldoutGroup(FOLDOUT_GROUP + APPASTR.Doozy_Graph)]
        public Graph testGraph;

#endif
    }
}
