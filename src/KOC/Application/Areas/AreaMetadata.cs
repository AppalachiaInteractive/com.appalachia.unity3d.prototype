using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Assets;
using Appalachia.CI.Integration.Extensions;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Core.Scriptables;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Graphs;
using Appalachia.Utility.Extensions;
using Doozy.Engine.Extensions;
using Doozy.Engine.Nody;
using Doozy.Engine.Nody.Models;
using Doozy.Engine.UI;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Areas
{
    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    public abstract class AreaMetadata<T, TM> : SingletonAppalachiaObject<TM>
        where T : AreaManager<T, TM>
        where TM : AreaMetadata<T, TM>
    {
        #region Fields and Autoproperties

        [FoldoutGroup(APPASTR.Canvas)]
        public AdditionalCanvasShaderChannels additionalShaderChannels;

        [FoldoutGroup(APPASTR.Graphic_Raycaster)]
        public GraphicRaycaster.BlockingObjects blockingObjects;

        [FoldoutGroup(APPASTR.Canvas_Group)]
        public bool blocksRaycasts;

        [FoldoutGroup(APPASTR.View)]
        public bool fullscreen;

        [FoldoutGroup(APPASTR.Canvas_Group)]
        public bool ignoreParentGroups;

        [FoldoutGroup(APPASTR.Graphic_Raycaster)]
        public bool ignoreReversedGraphics;

        [FoldoutGroup(APPASTR.Canvas_Group)]
        public bool interactable;

        [FoldoutGroup(APPASTR.Canvas)]
        public bool pixelPerfect;

        [FoldoutGroup(APPASTR.View), EnableIf(nameof(fullscreen))]
        public bool resetScaleToOne;

        [FoldoutGroup(APPASTR.Canvas_Scaling)]
        [Range(0f, 1f)]
        public float match;

        [FoldoutGroup(APPASTR.Canvas_Scaling)]
        [Range(1f, 200f)]
        public float referencePixelsPerUnit;

        [FoldoutGroup(APPASTR.Doozy_Graph)]
        public Graph graph;

        [FoldoutGroup(APPASTR.Canvas)]
        public int sortingOrder;

        [FoldoutGroup(APPASTR.Graphic_Raycaster)]
        public LayerMask blockingMask;

        [FoldoutGroup(APPASTR.Canvas)]
        public RenderMode renderMode;

        [FoldoutGroup(APPASTR.Canvas_Scaling)]
        public CanvasScaler.ScaleMode uiScaleMode;

        [FoldoutGroup(APPASTR.Canvas_Scaling)]
        public CanvasScaler.ScreenMatchMode screenMatchMode;

        [FoldoutGroup(APPASTR.Doozy_Canvas)]
        public string canvasName;

        [FoldoutGroup(APPASTR.Doozy_Graph)]
        public string graphControllerName;

        [FoldoutGroup(APPASTR.Doozy_View)]
        public string viewCategory;

        [FoldoutGroup(APPASTR.Doozy_View)]
        public string viewName;

        [FoldoutGroup(APPASTR.Canvas_Scaling)]
        public Vector2 referenceResolution;

        private string _defaultCanvasName;
        private string _defaultCategoryName;
        private string _defaultGraphName;

        private string _defaultViewName;

        #endregion

        public abstract ApplicationArea Area { get; }

        protected string DefaultCanvasName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_defaultCanvasName))
                {
                    return _defaultCanvasName;
                }

                _defaultCanvasName = Area.ToString();

                return _defaultCanvasName;
            }
        }

        protected string DefaultCategoryName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_defaultCategoryName))
                {
                    return _defaultCategoryName;
                }

                var manager = GetManager();

                _defaultCategoryName = (manager.HasParent ? manager.ParentArea : manager.Area).ToString();

                return _defaultCategoryName;
            }
        }

        protected string DefaultGraphName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_defaultGraphName))
                {
                    return _defaultGraphName;
                }

                _defaultGraphName = Area.ToString();

                return _defaultGraphName;
            }
        }

        protected string DefaultViewName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_defaultViewName))
                {
                    return _defaultViewName;
                }

                _defaultViewName = Area.ToString();

                return _defaultViewName;
            }
        }

        #region Event Functions

        private void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                Initialize();
            }
        }

        #endregion

        public void Apply(UIViewComponentSet target)
        {
            Initialize();

            Apply(target.rect);
            Apply(target.canvas);
            Apply(target.canvasGroup);
            Apply(target.graphicRaycaster);
            Apply(target.uiView);
            target.gameObject.SetDirty();
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
            target.gameObject.SetDirty();
        }

        protected void Apply(RectTransform target)
        {
            using (_PRF_Apply.Auto())
            {
                if (fullscreen)
                {
                    target.FullScreen(resetScaleToOne);
                    target.SetDirty();
                }
            }
        }

        protected void Apply(Canvas target)
        {
            using (_PRF_Apply.Auto())
            {
                target.renderMode = renderMode;
                target.pixelPerfect = pixelPerfect;
                target.sortingOrder = sortingOrder;
                target.additionalShaderChannels = additionalShaderChannels;
                target.SetDirty();
            }
        }

        protected void Apply(CanvasScaler target)
        {
            using (_PRF_Apply.Auto())
            {
                target.uiScaleMode = uiScaleMode;
                target.referenceResolution = referenceResolution;
                target.screenMatchMode = screenMatchMode;
                target.matchWidthOrHeight = match;
                target.referencePixelsPerUnit = referencePixelsPerUnit;
                target.SetDirty();
            }
        }

        protected void Apply(CanvasGroup target)
        {
            using (_PRF_Apply.Auto())
            {
                target.interactable = interactable;
                target.blocksRaycasts = blocksRaycasts;
                target.ignoreParentGroups = ignoreParentGroups;
                target.SetDirty();
            }
        }

        protected void Apply(GraphicRaycaster target)
        {
            using (_PRF_Apply.Auto())
            {
                target.blockingMask = blockingMask;
                target.blockingObjects = blockingObjects;
                target.ignoreReversedGraphics = ignoreReversedGraphics;
                target.SetDirty();
            }
        }

        protected void Apply(UICanvas target)
        {
            target.CanvasName = canvasName;
            target.CustomCanvasName = false;
            target.DontDestroyCanvasOnLoad = true;
        }

        protected void Apply(UIView target)
        {
            target.ViewCategory = viewCategory;
            target.ViewName = viewName;
        }

        protected void Apply(GraphController target)
        {
            var manager = GetManager();

            if (manager.HasParent)
            {
                graph.IsSubGraph = true;

                NodyGraphGenerator.GenerateTestGraph(target, this);
            }
            else
            {
                graph.IsSubGraph = false;
                target.SetGraph(graph);
            }

            target.ControllerName = graphControllerName;
            target.SetDirty();
        }

        public override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                initializationData.Reset(this, "2021-11-20a");

                var missingGraph = graph == null;
                var missingTestGraph = (graph != null) && graph.IsSubGraph && (testGraph == null);

                initializationData.Initialize(
                    this,
                    APPASTR.Doozy_Graph,
                    missingGraph || missingTestGraph,
                    () =>
                    {
#if UNITY_EDITOR
                        if (missingGraph)
                        {
                            graph = GenerateNodyGraph(DefaultGraphName);
                        }

                        if (missingTestGraph)
                        {
                            testGraph = GenerateNodyGraph(DefaultGraphName + "_TestWrapper");
                        }
#endif
                        graphControllerName = $"Graph Controller - {DefaultGraphName}";
                    }
                );

                initializationData.Initialize(
                    this,
                    APPASTR.Doozy_Canvas,
                    () => { canvasName = DefaultCanvasName; }
                );

                initializationData.Initialize(
                    this,
                    APPASTR.Doozy_View,
                    () =>
                    {
                        viewCategory = DefaultCategoryName;
                        viewName = DefaultViewName;
                    }
                );

                initializationData.Initialize(
                    this,
                    APPASTR.View,
                    () =>
                    {
                        fullscreen = true;
                        resetScaleToOne = true;
                    }
                );

                initializationData.Initialize(
                    this,
                    APPASTR.Canvas_Group,
                    () =>
                    {
                        interactable = true;
                        blocksRaycasts = true;
                        ignoreParentGroups = false;
                    }
                );

                initializationData.Initialize(
                    this,
                    APPASTR.Canvas_Scaling,
                    () =>
                    {
                        uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                        referenceResolution = new Vector2(1920f, 1080f);
                        screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                        match = 1f;
                        referencePixelsPerUnit = 100f;
                    }
                );

                initializationData.Initialize(
                    this,
                    APPASTR.Canvas,
                    () =>
                    {
                        renderMode = RenderMode.ScreenSpaceOverlay;
                        pixelPerfect = false;
                        sortingOrder = 0;
                        additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 |
                                                   AdditionalCanvasShaderChannels.Normal |
                                                   AdditionalCanvasShaderChannels.Tangent;
                    }
                );

                initializationData.Initialize(
                    this,
                    APPASTR.Graphic_Raycaster,
                    () =>
                    {
                        blockingMask = 0;
                        blockingObjects = GraphicRaycaster.BlockingObjects.None;
                        ignoreReversedGraphics = false;
                    }
                );
            }
        }

        private static Graph GenerateNodyGraph(string graphName)
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

        private IAreaManager GetManager()
        {
            return AreaManagerRegistry.GetManager(Area);
        }

        #region Profiling

        private const string _PRF_PFX = nameof(AreaMetadata<T, TM>) + ".";
        private static readonly ProfilerMarker _PRF_Awake = new ProfilerMarker(_PRF_PFX + nameof(Awake));
        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion

#if UNITY_EDITOR
        private bool _showTestGraph => (graph != null) && graph.IsSubGraph;

        [ShowIf(nameof(_showTestGraph))]
        [FoldoutGroup(APPASTR.Doozy_Graph)]
        public Graph testGraph;

#endif
    }
}
