using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Components.UI;
using Appalachia.Prototype.KOC.Application.Graphs;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Doozy.Engine.Extensions;
using Doozy.Engine.Nody;
using Doozy.Engine.UI;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Application.Areas
{
    [SmartLabelChildren]
    [InspectorIcon(Brand.AreaMetadata.Icon)]
    public abstract class AreaMetadata<T, TM> : SingletonAppalachiaObject<TM>, IAreaMetadata
        where T : AreaManager<T, TM>
        where TM : AreaMetadata<T, TM>
    {
        #region Constants and Static Readonly

        private const string FOLDOUT_GROUP = FOLDOUT_GROUP_ + "/";

        private const string FOLDOUT_GROUP_ = APPASTR.Common;

        #endregion

        #region Fields and Autoproperties

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Cursor, Expanded = false)]
        public AreaMetadataConfigurations.AreaCursorConfiguration cursor;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Input, Expanded = false)]
        public AreaMetadataConfigurations.AreaInputConfiguration input;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Canvas, Expanded = false)]
        public AreaMetadataConfigurations.AreaCanvasConfiguration canvas;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Canvas_Group, Expanded = false)]
        public AreaMetadataConfigurations.AreaCanvasGroupConfiguration canvasGroup;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Canvas_Scaling, Expanded = false)]
        public AreaMetadataConfigurations.AreaCanvasScalingConfiguration canvasScaling;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.View, Expanded = false)]
        public AreaMetadataConfigurations.AreaViewConfiguration view;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Graphic_Raycaster, Expanded = false)]
        public AreaMetadataConfigurations.AreaGraphicRaycasterConfiguration graphicRaycaster;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Doozy_Canvas, Expanded = false)]
        public AreaMetadataConfigurations.AreaDoozyCanvasConfiguration doozyCanvas;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Doozy_Graph, Expanded = false)]
        public AreaMetadataConfigurations.AreaDoozyGraphConfiguration doozyGraph;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Doozy_View, Expanded = false)]
        public AreaMetadataConfigurations.AreaDoozyViewConfiguration doozyView;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Menu, Expanded = false)]
        public AreaMetadataConfigurations.AreaMenuConfiguration menu;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Templates, Expanded = false)]
        public AreaMetadataConfigurations.AreaTemplatesConfiguration templates;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Default_References, Expanded = false)]
        public AreaMetadataConfigurations.AreaDefaultReferencesConfiguration defaultReferences;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Scene_Behaviour, Expanded = false)]
        public AreaMetadataConfigurations.AreaSceneBehaviourConfiguration sceneBehaviour;

        [SerializeField, FoldoutGroup(FOLDOUT_GROUP + APPASTR.Audio, Expanded = false)]
        public AreaMetadataConfigurations.AreaAudioConfiguration audio;

        #endregion

        [FoldoutGroup(FOLDOUT_GROUP_), PropertyOrder(-100)]
        [ShowInInspector, ReadOnly]
        public abstract AreaVersion Version { get; }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);
                AreaRegistry.RegisterMetadata(this);

                if (AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    return;
                }

                initializer.Reset(this, "2021-11-20a");

                await initializer.Do(
                    this,
                    APPASTR.Cursor,
                    cursor.ShouldForceReinitialize,
                    () => cursor.Initialize(Area)
                );

                await initializer.Do(
                    this,
                    APPASTR.Input,
                    input.ShouldForceReinitialize,
                    () => input.Initialize(Area)
                );

                await initializer.Do(
                    this,
                    APPASTR.Canvas,
                    canvas.ShouldForceReinitialize,
                    () => canvas.Initialize(Area)
                );

                await initializer.Do(
                    this,
                    APPASTR.View,
                    view.ShouldForceReinitialize,
                    () => view.Initialize(Area)
                );

                await initializer.Do(
                    this,
                    APPASTR.Graphic_Raycaster,
                    graphicRaycaster.ShouldForceReinitialize,
                    () => graphicRaycaster.Initialize(Area)
                );

                await initializer.Do(
                    this,
                    APPASTR.Doozy_Canvas,
                    doozyCanvas.ShouldForceReinitialize,
                    () => doozyCanvas.Initialize(Area)
                );

                await initializer.Do(
                    this,
                    APPASTR.Doozy_Graph,
                    doozyGraph.ShouldForceReinitialize,
                    () => doozyGraph.Initialize(Area)
                );

                await initializer.Do(
                    this,
                    APPASTR.Doozy_View,
                    doozyView.ShouldForceReinitialize,
                    () => doozyView.Initialize(Area)
                );

                await initializer.Do(
                    this,
                    APPASTR.Menu,
                    menu.ShouldForceReinitialize,
                    () => menu.Initialize(Area)
                );

                await initializer.Do(
                    this,
                    APPASTR.Templates,
                    templates.ShouldForceReinitialize,
                    () => templates.Initialize(Area)
                );

                await initializer.Do(
                    this,
                    APPASTR.Default_References,
                    defaultReferences.ShouldForceReinitialize,
                    () => defaultReferences.Initialize(Area)
                );

                await initializer.Do(
                    this,
                    APPASTR.Scene_Behaviour,
                    sceneBehaviour.ShouldForceReinitialize,
                    () => sceneBehaviour.Initialize(Area)
                );

                await initializer.Do(
                    this,
                    APPASTR.Audio,
                    audio.ShouldForceReinitialize,
                    () => audio.Initialize(Area)
                );
            }
        }

        protected void Apply(RectTransform target)
        {
            using (_PRF_Apply.Auto())
            {
                if (view.fullscreen)
                {
                    target.FullScreen(view.resetScaleToOne);
                    target.MarkAsModified();
                }
            }
        }

        protected void Apply(Canvas target)
        {
            using (_PRF_Apply.Auto())
            {
                target.renderMode = canvas.renderMode;
                target.pixelPerfect = canvas.pixelPerfect;
                target.sortingOrder = canvas.sortingOrder;
                target.additionalShaderChannels = canvas.additionalShaderChannels;
                target.MarkAsModified();
            }
        }

        protected void Apply(CanvasScaler target)
        {
            using (_PRF_Apply.Auto())
            {
                target.uiScaleMode = canvasScaling.uiScaleMode;
                target.referenceResolution = canvasScaling.referenceResolution;
                target.screenMatchMode = canvasScaling.screenMatchMode;
                target.matchWidthOrHeight = canvasScaling.match;
                target.referencePixelsPerUnit = canvasScaling.referencePixelsPerUnit;
                target.MarkAsModified();
            }
        }

        protected void Apply(CanvasGroup target)
        {
            using (_PRF_Apply.Auto())
            {
                target.interactable = canvasGroup.interactable;
                target.blocksRaycasts = canvasGroup.blocksRaycasts;
                target.ignoreParentGroups = canvasGroup.ignoreParentGroups;
                target.MarkAsModified();
            }
        }

        protected void Apply(GraphicRaycaster target)
        {
            using (_PRF_Apply.Auto())
            {
                target.blockingMask = graphicRaycaster.blockingMask;
                target.blockingObjects = graphicRaycaster.blockingObjects;
                target.ignoreReversedGraphics = graphicRaycaster.ignoreReversedGraphics;
                target.MarkAsModified();
            }
        }

        protected void Apply(UICanvas target)
        {
            using (_PRF_Apply.Auto())
            {
                target.CanvasName = doozyCanvas.canvasName;
                target.CustomCanvasName = false;
                target.DontDestroyCanvasOnLoad = false;
                MarkAsModified();
            }
        }

        protected void Apply(UIView target)
        {
            using (_PRF_Apply.Auto())
            {
                target.ViewCategory = doozyView.viewCategory;
                target.ViewName = doozyView.viewName;
                target.ShowBehavior.PresetCategory = "Fade";
                target.HideBehavior.PresetCategory = "Fade";
                target.LoopBehavior.PresetCategory = "Fade";
                target.ShowBehavior.PresetName = "InNormal";
                target.HideBehavior.PresetName = "OutNormal";
                target.LoopBehavior.PresetName = "Normal";
                MarkAsModified();
            }
        }

        protected void Apply(GraphController target)
        {
            using (_PRF_Apply.Auto())
            {
                var manager = GetManager();

                if (manager.HasParent)
                {
                    doozyGraph.graph.IsSubGraph = true;
#if UNITY_EDITOR
                    target.GenerateTestGraph(this);
#endif
                    target.DontDestroyControllerOnLoad = false;
                }
                else
                {
                    doozyGraph.graph.IsSubGraph = false;
                    target.SetGraph(doozyGraph.graph);
                }

#if UNITY_EDITOR
                doozyGraph.graph.CheckAndCreateAnyMissingSystemNodes();
#endif

                target.ControllerName = doozyGraph.graphControllerName;
                target.MarkAsModified();
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

        public AreaMetadataConfigurations.AreaCursorConfiguration Cursor => cursor;
        public AreaMetadataConfigurations.AreaInputConfiguration Input => input;
        public AreaMetadataConfigurations.AreaCanvasConfiguration Canvas => canvas;
        public AreaMetadataConfigurations.AreaViewConfiguration View => view;

        public AreaMetadataConfigurations.AreaGraphicRaycasterConfiguration GraphicRaycaster =>
            graphicRaycaster;

        public AreaMetadataConfigurations.AreaDoozyCanvasConfiguration DoozyCanvas => doozyCanvas;
        public AreaMetadataConfigurations.AreaDoozyGraphConfiguration DoozyGraph => doozyGraph;
        public AreaMetadataConfigurations.AreaDoozyViewConfiguration DoozyView => doozyView;
        public AreaMetadataConfigurations.AreaMenuConfiguration Menu => menu;
        public AreaMetadataConfigurations.AreaTemplatesConfiguration Templates => templates;

        public AreaMetadataConfigurations.AreaDefaultReferencesConfiguration DefaultReferences =>
            defaultReferences;

        public AreaMetadataConfigurations.AreaSceneBehaviourConfiguration SceneBehaviour => sceneBehaviour;
        public AreaMetadataConfigurations.AreaAudioConfiguration Audio => audio;

        public abstract ApplicationArea Area { get; }

        public void Apply(UITemplateComponentSet target)
        {
            Apply(target.rect);
            Apply(target.canvas);
            Apply(target.canvasGroup);

            target.image.enabled = templates.templateEnabled;
            target.image.sprite = templates.selectedTemplate;
            target.gameObject.MarkAsModified();
        }

        public void Apply(UIViewComponentSet target)
        {
            Apply(target.rect);
            Apply(target.canvas);
            Apply(target.canvasGroup);
            Apply(target.graphicRaycaster);
            Apply(target.uiView);
            target.GameObject.MarkAsModified();
        }

        public void Apply(UICanvasAreaComponentSet target)
        {
            Apply(target.rect);
            Apply(target.canvas);
            Apply(target.canvasGroup);
            Apply(target.canvasScaler);
            Apply(target.graphicRaycaster);
            Apply(target.uiCanvas);
            Apply(target.graphController);
            target.GameObject.MarkAsModified();
        }

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(AreaMetadata<T, TM>) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_GetManager =
            new ProfilerMarker(_PRF_PFX + nameof(GetManager));

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        #endregion

#if UNITY_EDITOR

        protected override string GetTitle()
        {
            return Brand.AreaMetadata.Text;
        }

        protected override string GetFallbackTitle()
        {
            return Brand.AreaMetadata.Fallback;
        }

        protected override string GetTitleColor()
        {
            return Brand.AreaMetadata.Color;
        }

        protected override string GetBackgroundColor()
        {
            return Brand.AreaMetadata.Banner;
        }
#endif
    }
}
