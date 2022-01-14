using System;
using Appalachia.CI.Constants;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Components.Graphs;
using Appalachia.Prototype.KOC.Components.UI;
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

namespace Appalachia.Prototype.KOC.Areas
{
    [SmartLabelChildren]
    [InspectorIcon(Brand.AreaMetadata.Icon)]
    [AssetLabel(Brand.AreaManager.Label)]
    public abstract class AreaMetadata<TManager, TMetadata> : SingletonAppalachiaObject<TMetadata>,
                                                              IAreaMetadata
        where TManager : AreaManager<TManager, TMetadata>
        where TMetadata : AreaMetadata<TManager, TMetadata>
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
            await base.Initialize(initializer);
            using (_PRF_Initialize.Auto())
            {
                AreaRegistry.RegisterMetadata(this);

                if (AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    return;
                }

                initializer.Reset(this, "2021-11-20a");
            }

            initializer.Do(
                this,
                APPASTR.Cursor,
                cursor.ShouldForceReinitialize,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        cursor.Initialize(Area);
                    }
                }
            );

            initializer.Do(
                this,
                APPASTR.Input,
                input.ShouldForceReinitialize,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        input.Initialize(Area);
                    }
                }
            );

            initializer.Do(
                this,
                APPASTR.Canvas,
                canvas.ShouldForceReinitialize,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        canvas.Initialize(Area);
                    }
                }
            );

            initializer.Do(
                this,
                APPASTR.View,
                view.ShouldForceReinitialize,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        view.Initialize(Area);
                    }
                }
            );

            initializer.Do(
                this,
                APPASTR.Graphic_Raycaster,
                graphicRaycaster.ShouldForceReinitialize,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        graphicRaycaster.Initialize(Area);
                    }
                }
            );

            initializer.Do(
                this,
                APPASTR.Doozy_Canvas,
                doozyCanvas.ShouldForceReinitialize,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        doozyCanvas.Initialize(Area);
                    }
                }
            );

            initializer.Do(
                this,
                APPASTR.Doozy_Graph,
                doozyGraph.ShouldForceReinitialize,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        doozyGraph.Initialize(Area);
                    }
                }
            );

            initializer.Do(
                this,
                APPASTR.Doozy_View,
                doozyView.ShouldForceReinitialize,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        doozyView.Initialize(Area);
                    }
                }
            );

            initializer.Do(
                this,
                APPASTR.Templates,
                templates.ShouldForceReinitialize,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        templates.Initialize(Area);
                    }
                }
            );

            initializer.Do(
                this,
                APPASTR.Default_References,
                defaultReferences.ShouldForceReinitialize,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        defaultReferences.Initialize(Area);
                    }
                }
            );

            initializer.Do(
                this,
                APPASTR.Scene_Behaviour,
                sceneBehaviour.ShouldForceReinitialize,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        sceneBehaviour.Initialize(Area);
                    }
                }
            );

            initializer.Do(
                this,
                APPASTR.Audio,
                audio.ShouldForceReinitialize,
                () =>
                {
                    using (_PRF_Initialize.Auto())
                    {
                        audio.Initialize(Area);
                    }
                }
            );
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
                if (doozyCanvas.doNotUseDoozyCanvas)
                {
                    target.enabled = false;
                }

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
                if (doozyView.doNotUseDoozyView)
                {
                    target.enabled = false;
                }

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
        public AreaMetadataConfigurations.AreaTemplatesConfiguration Templates => templates;

        public AreaMetadataConfigurations.AreaDefaultReferencesConfiguration DefaultReferences =>
            defaultReferences;

        public AreaMetadataConfigurations.AreaSceneBehaviourConfiguration SceneBehaviour => sceneBehaviour;
        public AreaMetadataConfigurations.AreaAudioConfiguration Audio => audio;

        public abstract ApplicationArea Area { get; }

        public void Apply(TemplateComponentSet target, GameObject manager, GameObject canvas, GameObject view)
        {
            switch (templates.parent)
            {
                case AreaMetadataConfigurations.AreaTemplatesConfiguration.Parent.AreaManager:
                    target.GameObject.transform.SetParent(manager.transform);
                    break;
                case AreaMetadataConfigurations.AreaTemplatesConfiguration.Parent.Canvas:
                    target.GameObject.transform.SetParent(canvas.transform);
                    break;
                case AreaMetadataConfigurations.AreaTemplatesConfiguration.Parent.View:
                    target.GameObject.transform.SetParent(view.transform);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Apply(target.rect);
            Apply(target.canvas);
            Apply(target.canvasGroup);

            target.image.enabled = templates.templateEnabled;
            target.image.sprite = templates.selectedTemplate;
            target.GameObject.MarkAsModified();
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

        public void Apply(UICanvasComponentSet target)
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

        private static readonly ProfilerMarker _PRF_Apply = new ProfilerMarker(_PRF_PFX + nameof(Apply));

        private static readonly ProfilerMarker _PRF_GetManager =
            new ProfilerMarker(_PRF_PFX + nameof(GetManager));

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
