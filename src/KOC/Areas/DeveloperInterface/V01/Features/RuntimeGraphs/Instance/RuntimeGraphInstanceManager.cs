using System.Collections.Generic;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Settings;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.UI;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Util;
using Appalachia.Utility.Async;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs.Instance
{
    public abstract class RuntimeGraphInstanceManager<TGraph, TManager, TMonitor, TText, TSettings> :
        RuntimeGraphInstanceBase<TGraph, TManager, TMonitor, TText, TSettings, TManager>,
        IMovable,
        IModifiableState
        where TGraph : RuntimeGraphInstance<TGraph, TManager, TMonitor, TText, TSettings>
        where TManager : RuntimeGraphInstanceManager<TGraph, TManager, TMonitor, TText, TSettings>
        where TMonitor : RuntimeGraphInstanceMonitor<TGraph, TManager, TMonitor, TText, TSettings>
        where TText : RuntimeGraphInstanceText<TGraph, TManager, TMonitor, TText, TSettings>
        where TSettings : IRuntimeGraphSettings
    {
        static RuntimeGraphInstanceManager()
        {
            RegisterDependency<TGraph>(i => graph = i);
            RegisterDependency<TMonitor>(i => monitor = i);
            RegisterDependency<TText>(i => text = i);
        }

        #region Static Fields and Autoproperties

        protected static TGraph graph;
        protected static TMonitor monitor;
        protected static TText text;

        #endregion

        #region Fields and Autoproperties

        [SerializeField] protected List<GameObject> m_nonBasicTextGameObjects = new();
        protected List<GameObject> childrenGameObjects = new();
        public ModuleState previousModuleState = ModuleState.FULL;
        public ModuleState currentModuleState = ModuleState.FULL;

        [FormerlySerializedAs("m_backgroundImages")]
        [SerializeField]
        protected List<Image> backgroundImages = new();

        #endregion

        protected abstract int BasicBackgroundImageIndex { get; }
        protected abstract int FullBackgroundImageIndex { get; }
        protected abstract int TextBackgroundImageIndex { get; }

        public override void InitializeParameters()
        {
            using (_PRF_InitializeParameters.Auto())
            {
                foreach (var image in backgroundImages)
                {
                    image.color = allSettings.general.backgroundColor;
                }

                graph.InitializeParameters();
                monitor.InitializeParameters();
                text.InitializeParameters();

                SetState(currentModuleState, true);
            }
        }

        public override void UpdateParameters()
        {
            using (_PRF_UpdateParameters.Auto())
            {
                foreach (var image in backgroundImages)
                {
                    image.color = allSettings.general.backgroundColor;
                }

                graph.UpdateParameters();
                monitor.UpdateParameters();
                text.UpdateParameters();

                SetState(currentModuleState, true);
            }
        }

        public void RestorePreviousState()
        {
            using (_PRF_RestorePreviousState.Auto())
            {
                SetState(previousModuleState);
            }
        }

        protected virtual void UpdateModuleToStateBackground()
        {
            using (_PRF_UpdateModuleToStateBackground.Auto())
            {
                gameObject.SetActive(true);
                SetGraphActive(false);

                childrenGameObjects.SetAllActive(false);
                backgroundImages.SetAllActive(false);
            }
        }

        protected virtual void UpdateModuleToStateBasic()
        {
            using (_PRF_UpdateModuleToStateBasic.Auto())
            {
                gameObject.SetActive(true);
                childrenGameObjects.SetAllActive(true);
                m_nonBasicTextGameObjects.SetAllActive(false);
                SetGraphActive(false);

                if (allSettings.general.background)
                {
                    backgroundImages.SetOneActive(BasicBackgroundImageIndex);
                }
                else
                {
                    backgroundImages.SetAllActive(false);
                }
            }
        }

        protected virtual void UpdateModuleToStateFull()
        {
            using (_PRF_UpdateModuleToStateFull.Auto())
            {
                gameObject.SetActive(true);
                childrenGameObjects.SetAllActive(true);
                SetGraphActive(true);

                if (allSettings.general.background)
                {
                    backgroundImages.SetOneActive(FullBackgroundImageIndex);
                }
                else
                {
                    backgroundImages.SetAllActive(false);
                }
            }
        }

        protected virtual void UpdateModuleToStateOff()
        {
            using (_PRF_UpdateModuleToStateOff.Auto())
            {
                gameObject.SetActive(false);
            }
        }

        protected virtual void UpdateModuleToStateText()
        {
            using (_PRF_UpdateModuleToStateText.Auto())
            {
                gameObject.SetActive(true);
                childrenGameObjects.SetAllActive(true);
                SetGraphActive(false);

                if (allSettings.general.background)
                {
                    backgroundImages.SetOneActive(TextBackgroundImageIndex);
                }
                else
                {
                    backgroundImages.SetAllActive(false);
                }
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }

            childrenGameObjects ??= new List<GameObject>();

            childrenGameObjects.Clear();

            foreach (Transform child in transform)
            {
                if (child.parent == transform)
                {
                    childrenGameObjects.Add(child.gameObject);
                }
            }
        }

        protected void SetGraphActive(bool active)
        {
            using (_PRF_SetGraphActive.Auto())
            {
                graph.enabled = active;
                graph.graphParent.SetActive(active);
            }
        }

        #region IModifiableState Members

        public void SetState(ModuleState state, bool silentUpdate = false)
        {
            using (_PRF_SetState.Auto())
            {
                if (!silentUpdate)
                {
                    previousModuleState = currentModuleState;
                }

                currentModuleState = state;

                switch (state)
                {
                    case ModuleState.FULL:
                        UpdateModuleToStateFull();

                        break;

                    case ModuleState.TEXT:
                        UpdateModuleToStateText();

                        break;

                    case ModuleState.BASIC:
                        UpdateModuleToStateBasic();

                        break;

                    case ModuleState.BACKGROUND:
                        UpdateModuleToStateBackground();
                        break;

                    case ModuleState.OFF:
                        UpdateModuleToStateOff();
                        break;
                }
            }
        }

        #endregion

        #region IMovable Members

        public void SetPosition(ModulePosition newModulePosition)
        {
            using (_PRF_SetPosition.Auto())
            {
                if (rectTransform == null)
                {
                    rectTransform = GetComponent<RectTransform>();
                }

                var xSideOffset = Mathf.Abs(rectTransform.anchoredPosition.x);
                var ySideOffset = Mathf.Abs(rectTransform.anchoredPosition.y);

                switch (newModulePosition)
                {
                    case ModulePosition.TOP_LEFT:

                        rectTransform.anchorMax = Vector2.up;
                        rectTransform.anchorMin = Vector2.up;
                        rectTransform.anchoredPosition = new Vector2(xSideOffset, -ySideOffset);

                        break;

                    case ModulePosition.TOP_RIGHT:

                        rectTransform.anchorMax = Vector2.one;
                        rectTransform.anchorMin = Vector2.one;
                        rectTransform.anchoredPosition = new Vector2(-xSideOffset, -ySideOffset);

                        break;

                    case ModulePosition.BOTTOM_LEFT:

                        rectTransform.anchorMax = Vector2.zero;
                        rectTransform.anchorMin = Vector2.zero;
                        rectTransform.anchoredPosition = new Vector2(xSideOffset, ySideOffset);

                        break;

                    case ModulePosition.BOTTOM_RIGHT:

                        rectTransform.anchorMax = Vector2.right;
                        rectTransform.anchorMin = Vector2.right;
                        rectTransform.anchoredPosition = new Vector2(-xSideOffset, ySideOffset);

                        break;

                    case ModulePosition.FREE:
                        break;
                }
            }
        }

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitializeParameters =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeParameters));

        private static readonly ProfilerMarker _PRF_RestorePreviousState =
            new ProfilerMarker(_PRF_PFX + nameof(RestorePreviousState));

        private static readonly ProfilerMarker _PRF_SetGraphActive =
            new ProfilerMarker(_PRF_PFX + nameof(SetGraphActive));

        private static readonly ProfilerMarker _PRF_SetPosition =
            new ProfilerMarker(_PRF_PFX + nameof(SetPosition));

        private static readonly ProfilerMarker
            _PRF_SetState = new ProfilerMarker(_PRF_PFX + nameof(SetState));

        private static readonly ProfilerMarker _PRF_UpdateModuleToStateBackground =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateModuleToStateBackground));

        private static readonly ProfilerMarker _PRF_UpdateModuleToStateBasic =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateModuleToStateBasic));

        private static readonly ProfilerMarker _PRF_UpdateModuleToStateFull =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateModuleToStateFull));

        private static readonly ProfilerMarker _PRF_UpdateModuleToStateOff =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateModuleToStateOff));

        private static readonly ProfilerMarker _PRF_UpdateModuleToStateText =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateModuleToStateText));

        private static readonly ProfilerMarker _PRF_UpdateParameters =
            new ProfilerMarker(_PRF_PFX + nameof(UpdateParameters));

        #endregion
    }
}
