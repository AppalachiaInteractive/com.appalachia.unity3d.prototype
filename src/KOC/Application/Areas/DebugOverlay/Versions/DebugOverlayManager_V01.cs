using Appalachia.Prototype.KOC.Application.Components.Fading;
using Appalachia.Prototype.KOC.Debugging.DebugConsole;
using Appalachia.Prototype.KOC.Debugging.RuntimeGraphs;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Application.Areas.DebugOverlay.Versions
{
    public class
        DebugOverlayManager_V01 : DebugOverlayManager<DebugOverlayManager_V01, DebugOverlayMetadata_V01>
    {
        #region Fields and Autoproperties

        [BoxGroup("Runtime Graph")]
        [ReadOnly]
        public GraphyManager runtimeGraphManager;

        [BoxGroup("Runtime Graph")]
        [ReadOnly]
        public CanvasFadeManager runtimeGraphManagerCanvasFadeManager;

        [BoxGroup("Runtime Graph")]
        [ReadOnly]
        public CanvasGroup runtimeGraphManagerCanvasGroup;

        [BoxGroup("Developer Console")]
        [ReadOnly]
        public DebugLogManager developerConsoleManager;

        [BoxGroup("Developer Console")]
        [ReadOnly]
        public CanvasFadeManager developerConsoleCanvasFadeManager;

        [BoxGroup("Developer Console")]
        [ReadOnly]
        public CanvasGroup developerConsoleCanvasGroup;

        #endregion

        public override AreaVersion Version => AreaVersion.V01;

        public override void OnToggleDebugLog(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleDebugLog.Auto())
            {
                if (!context.performed)
                {
                    return;
                }

                Context.Log.Info(nameof(OnToggleDebugLog), this);

                DebugLogManager.instance.Toggle();
            }
        }

        public override void OnToggleGraphy(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleGraphy.Auto())
            {
                if (!context.performed)
                {
                    return;
                }

                Context.Log.Info(nameof(OnToggleGraphy), this);

                GraphyManager.instance.ToggleActive();
            }
        }

        public override void OnToggleGraphyMode(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleGraphyMode.Auto())
            {
                if (!context.performed)
                {
                    return;
                }

                Context.Log.Info(nameof(OnToggleGraphyMode), this);

                GraphyManager.instance.CycleThroughPresets();
            }
        }

        protected override void AfterToggleDebugOverlays(InputAction.CallbackContext context)
        {
            using (_PRF_AfterToggleDebugOverlays.Auto())
            {
                ExecuteCanvasComponentFade(developerConsoleCanvasFadeManager, developerConsoleCanvasGroup);
                ExecuteCanvasComponentFade(
                    runtimeGraphManagerCanvasFadeManager,
                    runtimeGraphManagerCanvasGroup
                );
            }
        }

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                await initializer.Do(
                    this,
                    nameof(GraphyManager),
                    runtimeGraphManager == null,
                    () =>
                    {
                        runtimeGraphManager = FindObjectOfType<GraphyManager>(true);
                        runtimeGraphManager.enabled = true;
                        runtimeGraphManager.gameObject.SetActive(true);
                    }
                );

                await initializer.Do(
                    this,
                    nameof(DebugLogManager),
                    developerConsoleManager == null,
                    () =>
                    {
                        developerConsoleManager = FindObjectOfType<DebugLogManager>(true);
                        developerConsoleManager.enabled = true;
                        developerConsoleManager.gameObject.SetActive(true);
                    }
                );

                runtimeGraphManagerCanvasFadeManager = runtimeGraphManager.GetComponent<CanvasFadeManager>();
                runtimeGraphManagerCanvasGroup = runtimeGraphManager.GetComponent<CanvasGroup>();
                developerConsoleCanvasFadeManager = developerConsoleManager.GetComponent<CanvasFadeManager>();
                developerConsoleCanvasGroup = developerConsoleManager.GetComponent<CanvasGroup>();
            }
        }

        protected override void ResetArea()
        {
            using (_PRF_ResetArea.Auto())
            {
                Context.Log.Info(nameof(ResetArea), this);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(DebugOverlayManager_V01) + ".";

        private static readonly ProfilerMarker _PRF_AfterToggleDebugOverlays =
            new ProfilerMarker(_PRF_PFX + nameof(AfterToggleDebugOverlays));

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_OnToggleDebugLog =
            new ProfilerMarker(_PRF_PFX + nameof(OnToggleDebugLog));

        private static readonly ProfilerMarker _PRF_OnToggleGraphy =
            new ProfilerMarker(_PRF_PFX + nameof(OnToggleGraphy));

        private static readonly ProfilerMarker _PRF_OnToggleGraphyMode =
            new ProfilerMarker(_PRF_PFX + nameof(OnToggleGraphyMode));

        #endregion
    }
}
