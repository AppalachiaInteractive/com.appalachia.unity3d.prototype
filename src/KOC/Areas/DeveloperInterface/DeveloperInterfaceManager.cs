using System;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Prototype.KOC.Input;
using Appalachia.Utility.Strings;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface
{
    public abstract class DeveloperInterfaceManager<TManager, TMetadata> : AreaManager<TManager, TMetadata>,
        IDeveloperInterfaceManager,
        KOCInputActions.IDeveloperInterfaceActions
        where TManager : DeveloperInterfaceManager<TManager, TMetadata>
        where TMetadata : DeveloperInterfaceMetadata<TManager, TMetadata>
    {
        public abstract void OnOpenCommandPalette(InputAction.CallbackContext context);

        public void OnToggleDebugOverlays(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleDebugOverlays.Auto())
            {
                if (!context.performed)
                {
                    return;
                }

                ToggleAreaInterface();
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

        #region IDeveloperInterfaceActions Members

        public abstract void OnToggleDeveloperInterface(InputAction.CallbackContext context);
        public abstract void OnToggleMenuBar(InputAction.CallbackContext context);
        public abstract void OnToggleStatusBar(InputAction.CallbackContext context);
        public abstract void OnToggleActivityBar(InputAction.CallbackContext context);
        public abstract void OnToggleSideBar(InputAction.CallbackContext context);
        public abstract void OnTogglePanel(InputAction.CallbackContext context);

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

        #endregion

        #region IDeveloperInterfaceManager Members

        public override ApplicationArea Area => ApplicationArea.DeveloperInterface;
        public override ApplicationArea ParentArea => ApplicationArea.None;

        #endregion

        #region Profiling

        private static readonly ProfilerMarker _PRF_OnActivation =
            new ProfilerMarker(_PRF_PFX + nameof(OnActivation));

        private static readonly ProfilerMarker _PRF_OnDeactivation =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeactivation));

        private static readonly ProfilerMarker _PRF_OnScreenshot =
            new ProfilerMarker(_PRF_PFX + nameof(OnScreenshot));

        private static readonly ProfilerMarker _PRF_OnToggleDebugOverlays =
            new ProfilerMarker(_PRF_PFX + nameof(OnToggleDebugOverlays));

        #endregion
    }
}
