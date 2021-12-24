using System;
using System.Collections.Generic;
using Appalachia.Prototype.KOC.Application.Components.Cursors;
using Appalachia.Prototype.KOC.Application.Input;
using Appalachia.Utility.Strings;
using Doozy.Engine.Nody.Models;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
#if UNITY_EDITOR
using Appalachia.Prototype.KOC.Application.Graphs;
#endif

namespace Appalachia.Prototype.KOC.Application.Areas
{
    [Serializable]
    public struct AreaMetadataConfigurations
    {
        #region Nested type: AreaAudioConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaAudioConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [SerializeField] public AudioMixerGroup mixerGroup;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
            }

            #endregion
        }

        #endregion

        #region Nested type: AreaCanvasConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaCanvasConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [SerializeField] public AdditionalCanvasShaderChannels additionalShaderChannels;

            [ToggleLeft]
            [SerializeField]
            public bool pixelPerfect;

            [SerializeField] public int sortingOrder;

            [SerializeField] public RenderMode renderMode;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
                renderMode = RenderMode.ScreenSpaceOverlay;
                pixelPerfect = false;
                sortingOrder = 0;
                additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 |
                                           AdditionalCanvasShaderChannels.Normal |
                                           AdditionalCanvasShaderChannels.Tangent;
            }

            #endregion
        }

        #endregion

        #region Nested type: AreaCanvasGroupConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaCanvasGroupConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [ToggleLeft]
            [SerializeField]
            public bool blocksRaycasts;

            [ToggleLeft]
            [SerializeField]
            public bool ignoreParentGroups;

            [ToggleLeft]
            [SerializeField]
            public bool interactable;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
                interactable = true;
                blocksRaycasts = true;
                ignoreParentGroups = false;
            }

            #endregion
        }

        #endregion

        #region Nested type: AreaCanvasScalingConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaCanvasScalingConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [Range(0f, 1f)]
            [SerializeField]
            public float match;

            [Range(1f, 200f)]
            [SerializeField]
            public float referencePixelsPerUnit;

            [SerializeField] public CanvasScaler.ScaleMode uiScaleMode;

            [SerializeField] public CanvasScaler.ScreenMatchMode screenMatchMode;

            [SerializeField] public Vector2 referenceResolution;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
                uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                referenceResolution = new Vector2(1920f, 1080f);
                screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                match = 1f;
                referencePixelsPerUnit = 100f;
            }

            #endregion
        }

        #endregion

        #region Nested type: AreaCursorConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaCursorConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [DisableIf(nameof(hideCursor))]
            [ToggleLeft]
            [SerializeField]
            public bool hasCustomCursor;

            [DisableIf(nameof(hideCursor))]
            [ShowIf(nameof(hasCustomCursor))]
            [ToggleLeft]
            [SerializeField]
            public bool hasSimpleCursor;

            [ShowIf(nameof(hasCustomCursor))]
            [ToggleLeft]
            [SerializeField]
            public bool hideCursor;

            [ShowIf(nameof(hasCustomCursor))]
            [ToggleLeft]
            [SerializeField]
            public bool lockCursor;

            [ShowIf(nameof(hasCustomCursor))]
            [EnableIf(nameof(showComplexCursor))]
            [SerializeField]
            public ComplexCursors complexCursor;

            [ShowIf(nameof(hasCustomCursor))]
            [SerializeField]
            public CursorState initialState;

            [ShowIf(nameof(hasCustomCursor))]
            [EnableIf(nameof(showSimpleCursor))]
            [SerializeField]
            public SimpleCursors simpleCursor;

            #endregion

            private bool showComplexCursor => hasCustomCursor && !hasSimpleCursor && !hideCursor;

            private bool showSimpleCursor => hasCustomCursor && hasSimpleCursor && !hideCursor;

            public void Apply(CursorManager cursorManager)
            {
                if (hideCursor)
                {
                    cursorManager.SetCursorVisible(false);
                }

                if (hasCustomCursor)
                {
                    if (hasSimpleCursor)
                    {
                        cursorManager.SetCursorType(simpleCursor);
                    }
                    else
                    {
                        cursorManager.SetCursorType(complexCursor);
                    }
                }

                if (lockCursor)
                {
                    cursorManager.SetCursorLocked(lockCursor);
                }

                cursorManager.SetCursorState(initialState);
            }

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
                initialState = CursorState.Normal;
            }

            #endregion
        }

        #endregion

        #region Nested type: AreaDefaultReferencesConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaDefaultReferencesConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [SerializeField] public AreaMetadataDefaultReferences defaultReferences;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
                defaultReferences = AreaMetadataDefaultReferences.instance;
            }

            #endregion
        }

        #endregion

        #region Nested type: AreaDoozyCanvasConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaDoozyCanvasConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [SerializeField] public string canvasName;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
                canvasName = area.ToString();
            }

            #endregion
        }

        #endregion

        #region Nested type: AreaDoozyGraphConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaDoozyGraphConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [SerializeField] public Graph graph;

            [ShowIf(nameof(_showTestGraph))]
            [SerializeField]
            public Graph testGraph;

            [SerializeField] public string graphControllerName;

            #endregion

            public bool MissingGraph => graph == null;
            public bool MissingTestGraph => (graph != null) && graph.IsSubGraph && (testGraph == null);

            private bool _showTestGraph => (graph != null) && graph.IsSubGraph;

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => MissingGraph || MissingTestGraph;

            public void Initialize(ApplicationArea area)
            {
                var graphName = area.ToString();
#if UNITY_EDITOR

                if (MissingGraph)
                {
                    graph = NodyGraphGenerator.GenerateNodyGraph(graphName, false);
                }

                if (MissingTestGraph)
                {
                    testGraph = NodyGraphGenerator.GenerateNodyGraph(graphName + "_TestWrapper", false);

                    graph.IsSubGraph = true;
                    graph.CheckAndCreateAnyMissingSystemNodes();
                }
#endif
                graphControllerName = ZString.Format("Graph Controller - {0}", graphName);
            }

            #endregion
        }

        #endregion

        #region Nested type: AreaDoozyViewConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaDoozyViewConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [SerializeField] public string viewCategory;

            [SerializeField] public string viewName;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
                var manager = AreaRegistry.GetManager(area);

                if (manager != null)
                {
                    viewCategory = (manager.HasParent ? manager.ParentArea : area).ToString();
                }
                else
                {
                    viewCategory = area.ToString();
                }

                viewName = area.ToString();
            }

            #endregion
        }

        #endregion

        #region Nested type: AreaGraphicRaycasterConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaGraphicRaycasterConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [SerializeField] public GraphicRaycaster.BlockingObjects blockingObjects;

            [SerializeField] public bool ignoreReversedGraphics;

            [SerializeField] public LayerMask blockingMask;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
                blockingMask = 0;
                blockingObjects = GraphicRaycaster.BlockingObjects.None;
                ignoreReversedGraphics = false;
            }

            #endregion
        }

        #endregion

        #region Nested type: AreaInputConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaInputConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [Title("On Disable")]
            [SerializeField]
            public KOCInputActions.MapEnableState onDisableMapState;

            [Title("On Enable")]
            [SerializeField]
            public KOCInputActions.MapEnableState onEnableMapState;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
            }

            #endregion
        }

        #endregion

        #region Nested type: AreaMenuConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaMenuConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [ToggleLeft]
            [SerializeField]
            public bool doesDrawMenu;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
            }

            #endregion
        }

        #endregion

        #region Nested type: AreaSceneBehaviourConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaSceneBehaviourConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [ToggleLeft]
            [SerializeField]
            public bool setEntrySceneActive;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
            }

            #endregion
        }

        #endregion

        #region Nested type: AreaTemplatesConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaTemplatesConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [ToggleLeft]
            [SerializeField]
            public bool templateEnabled;

            [Range(0.0f, 1.0f)]
            [SerializeField]
            public float templateAlpha;

            [SerializeField] public List<Sprite> templates;

            [ValueDropdown(nameof(templates))]
            [SerializeField]
            public Sprite selectedTemplate;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
                templateAlpha = 1.0f;
            }

            #endregion
        }

        #endregion

        #region Nested type: AreaViewConfiguration

        [Serializable, HideReferenceObjectPicker, InlineProperty, HideLabel]
        public struct AreaViewConfiguration : IAreaConfiguration
        {
            #region Fields and Autoproperties

            [ToggleLeft]
            [SerializeField]
            public bool fullscreen;

            [ToggleLeft]
            [SerializeField]
            public bool resetScaleToOne;

            #endregion

            #region IAreaConfiguration Members

            public bool ShouldForceReinitialize => false;

            public void Initialize(ApplicationArea area)
            {
                fullscreen = true;
                resetScaleToOne = true;
            }

            #endregion
        }

        #endregion

        #region Nested type: IAreaConfiguration

        public interface IAreaConfiguration
        {
            bool ShouldForceReinitialize { get; }

            void Initialize(ApplicationArea area);
        }

        #endregion
    }
}
