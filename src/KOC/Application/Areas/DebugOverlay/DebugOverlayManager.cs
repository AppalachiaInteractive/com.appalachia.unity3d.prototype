using System;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Prototype.KOC.Application.Components.Fading;
using Appalachia.Prototype.KOC.Application.Input;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Appalachia.Prototype.KOC.Application.Areas.DebugOverlay
{
    public abstract class DebugOverlayManager<T, TM> : AreaManager<T, TM>,
                                                       IDebugOverlayManager,
                                                       KOCInputActions.IDebugActions
        where T : DebugOverlayManager<T, TM>
        where TM : DebugOverlayMetadata<T, TM>
    {
        protected virtual void AfterToggleDebugOverlays(InputAction.CallbackContext context)
        {
        }

        protected static void ExecuteCanvasComponentFade(
            CanvasFadeManager canvasFadeManager,
            CanvasGroup canvasGroup)
        {
            using (_PRF_ExecuteCanvasComponentFade.Auto())
            {
                if (canvasFadeManager.IsFading)
                {
                    return;
                }

                var overlayIsShowing = canvasGroup.alpha > .99f;

                if (overlayIsShowing)
                {
                    canvasFadeManager.FadeOut();
                }

                else
                {
                    canvasFadeManager.FadeIn();
                }
            }
        }

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();
            }
        }

        protected override void OnActivation()
        {
            using (_PRF_OnActivation.Auto())
            {
                Context.Log.Info(nameof(OnActivation), this);
            }
        }

        protected override void OnDeactivation()
        {
            using (_PRF_OnDeactivation.Auto())
            {
                Context.Log.Info(nameof(OnDeactivation), this);
            }
        }

        #region IDebugActions Members

        public void OnToggleDebugOverlays(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleDebugOverlays.Auto())
            {
                if (!context.performed)
                {
                    return;
                }

                var canvasFadeManager = view.canvasFadeManager;
                var canvasGroup = view.canvasGroup;

                ExecuteCanvasComponentFade(canvasFadeManager, canvasGroup);
                AfterToggleDebugOverlays(context);
            }
        }

        public void OnScreenshot(InputAction.CallbackContext context)
        {
            using (_PRF_OnScreenshot.Auto())
            {
                if (!context.performed)
                {
                    return;
                }

                Context.Log.Info(nameof(OnScreenshot), this);

                var now = DateTime.Now;
                var filename = ZString.Format(
                    "{0}-{1}{2:D2}{3:D2}{4:D2}{5:D2}{6:D2}.png",
                    SceneManager.GetSceneAt(0).name,
                    now.Year,
                    now.Month,
                    now.Day,
                    now.Hour,
                    now.Minute,
                    now.Second
                );
                var filePath = AppaPath.Combine("Screenshots", filename);

                AppaDirectory.CreateDirectoryStructureForFilePath(filePath);

                Context.Log.Info(ZString.Format("Captured Screenshot to : {0}", filePath));
                ScreenCapture.CaptureScreenshot(filePath);
            }
        }

        public abstract void OnToggleDebugLog(InputAction.CallbackContext context);

        public abstract void OnToggleGraphy(InputAction.CallbackContext context);

        public abstract void OnToggleGraphyMode(InputAction.CallbackContext context);

        #endregion

        #region IDebugOverlayManager Members

        public override ApplicationArea Area => ApplicationArea.DebugOverlay;
        public override ApplicationArea ParentArea => ApplicationArea.None;

        #endregion

        #region Profiling

        private const string _PRF_PFX = nameof(DebugOverlayManager<T, TM>) + ".";

        private static readonly ProfilerMarker _PRF_ExecuteCanvasComponentFade =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteCanvasComponentFade));

        private static readonly ProfilerMarker _PRF_OnToggleDebugOverlays =
            new ProfilerMarker(_PRF_PFX + nameof(OnToggleDebugOverlays));

        private static readonly ProfilerMarker _PRF_OnActivation =
            new ProfilerMarker(_PRF_PFX + nameof(OnActivation));

        private static readonly ProfilerMarker _PRF_OnDeactivation =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeactivation));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_OnScreenshot =
            new ProfilerMarker(_PRF_PFX + nameof(OnScreenshot));

        #endregion
    }
}
