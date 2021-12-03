using System;
using Appalachia.CI.Integration.FileSystem;
using Appalachia.Editing.Debugging.Graphy;
using Appalachia.Editing.Debugging.IngameDebugConsole;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Appalachia.Prototype.KOC.Application.Areas.DebugOverlay
{
    public class DebugOverlayManager : AreaManager<DebugOverlayManager, DebugOverlayMetadata>
    {
        #region Fields and Autoproperties

        [FoldoutGroup("Editor Only")]
        [BoxGroup("Editor Only/Graphy")]
        [ReadOnly]
        public GameObject graphyInstance;

        [BoxGroup("Editor Only/Debug Log")]
        [ReadOnly]
        public GameObject inGameConsoleInstance;

        #endregion

        public override ApplicationArea Area => ApplicationArea.DebugOverlay;
        public override ApplicationArea ParentArea => ApplicationArea.None;
        

        protected override void OnActivation()
        {
            using (_PRF_Activate.Auto())
            {
                AppaLog.Context.Area.Info(nameof(OnActivation));

                Initialize();
                EnableInput();
            }
        }

        protected override void OnDeactivation()
        {
            using (_PRF_Deactivate.Auto())
            {
                AppaLog.Context.Area.Info(nameof(OnDeactivation));
            }
        }

        protected override void ResetArea()
        {
            using (_PRF_ResetArea.Auto())
            {
                AppaLog.Context.Area.Info(nameof(ResetArea));

                graphyInstance.DestroySafely();
                inGameConsoleInstance.DestroySafely();
            }
        }
        
        private void EnableInput()
        {
            using (_PRF_EnableInput.Auto())
            {
                AppaLog.Context.Area.Info(nameof(EnableInput));

                metadata.graphyToggleActive.action.performed += _ =>
                {
                    GraphyManager.instance.ToggleActive();
                };
                metadata.graphyToggleModes.action.performed += _ => { GraphyManager.instance.ToggleModes(); };
                metadata.debugLogToggle.action.performed += _ => { DebugLogManager.instance.Toggle(); };
                metadata.captureScreenshot.action.performed += _ =>
                {
                    var now = DateTime.Now;
                    var filename =
                        $"{SceneManager.GetSceneAt(1).name}-{now.Year}{now.Month:D2}{now.Day:D2}{now.Hour:D2}{now.Minute:D2}{now.Second:D2}.png";
                    var filePath = AppaPath.Combine("Screenshots", filename);

                    AppaDirectory.CreateDirectoryStructureForFilePath(filePath);

                    AppaLog.Info($"Captured Screenshot to : {filePath}");
                    ScreenCapture.CaptureScreenshot(filePath);
                };
            }
        }

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();
                
                AppaLog.Context.Area.Info(nameof(Initialize));

                if (graphyInstance == null)
                {
                    metadata.graphyPrefab.GetReference(
                        canvas.canvasGroup.transform,
                        go => graphyInstance = go
                    );
                }

                if (inGameConsoleInstance == null)
                {
                    metadata.inGameConsolePrefab.GetReference(
                        canvas.canvasGroup.transform,
                        go => inGameConsoleInstance = go
                    );
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(DebugOverlayManager) + ".";

        private static readonly ProfilerMarker
            _PRF_Activate = new ProfilerMarker(_PRF_PFX + nameof(OnActivation));

        private static readonly ProfilerMarker _PRF_Deactivate =
            new ProfilerMarker(_PRF_PFX + nameof(OnDeactivation));

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        private static readonly ProfilerMarker _PRF_EnableInput =
            new ProfilerMarker(_PRF_PFX + nameof(EnableInput));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
