using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Application.Services.Screenshot.Model;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Services.Screenshots;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.ActivityBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.CommandPalette;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.CommandSuggestions;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.GameArea;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.MenuBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.Panel;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.SideBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.StatusBar;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Unity.Profiling;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01
{
    [CallStaticConstructorInEditor]
    public sealed class DeveloperInterfaceManager_V01 : DeveloperInterfaceManager<
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        static DeveloperInterfaceManager_V01()
        {
            FunctionalitySet.RegisterService<DeveloperInterfaceScreenshotService>(
                _dependencyTracker,
                i => _developerInterfaceScreenshotService = i
            );

            FunctionalitySet.RegisterWidget<DeveloperInterfacePanelWidget>(
                _dependencyTracker,
                i => _developerInterfacePanelWidget = i
            );
            FunctionalitySet.RegisterWidget<DeveloperInterfaceActivityBarWidget>(
                _dependencyTracker,
                i => _developerInterfaceActivityBarWidget = i
            );
            FunctionalitySet.RegisterWidget<DeveloperInterfaceCommandPaletteWidget>(
                _dependencyTracker,
                i => _developerInterfaceCommandPaletteWidget = i
            );
            FunctionalitySet.RegisterWidget<DeveloperInterfaceMenuBarWidget>(
                _dependencyTracker,
                i => _developerInterfaceMenuBarWidget = i
            );
            FunctionalitySet.RegisterWidget<DeveloperInterfaceSideBarWidget>(
                _dependencyTracker,
                i => _developerInterfaceSideBarWidget = i
            );
            FunctionalitySet.RegisterWidget<DeveloperInterfaceStatusBarWidget>(
                _dependencyTracker,
                i => _developerInterfaceStatusBarWidget = i
            );
            FunctionalitySet.RegisterWidget<DeveloperInterfaceGameAreaWidget>(
                _dependencyTracker,
                i => _developerInterfaceGameAreaWidget = i
            );
            FunctionalitySet.RegisterWidget<DeveloperInterfaceCommandSuggestionsWidget>(
                _dependencyTracker,
                i => _developerInterfaceCommandSuggestionsWidget = i
            );
        }

        #region Static Fields and Autoproperties

        private static DeveloperInterfaceActivityBarWidget _developerInterfaceActivityBarWidget;
        private static DeveloperInterfaceCommandPaletteWidget _developerInterfaceCommandPaletteWidget;
        private static DeveloperInterfaceCommandSuggestionsWidget _developerInterfaceCommandSuggestionsWidget;
        private static DeveloperInterfaceGameAreaWidget _developerInterfaceGameAreaWidget;
        private static DeveloperInterfaceMenuBarWidget _developerInterfaceMenuBarWidget;

        private static DeveloperInterfacePanelWidget _developerInterfacePanelWidget;

        private static DeveloperInterfaceScreenshotService _developerInterfaceScreenshotService;
        private static DeveloperInterfaceSideBarWidget _developerInterfaceSideBarWidget;
        private static DeveloperInterfaceStatusBarWidget _developerInterfaceStatusBarWidget;

        #endregion

        #region Fields and Autoproperties

        private bool _isVisible;

        #endregion

        public override AreaVersion Version => AreaVersion.V01;

        public bool IsVisible => _isVisible;

        public override void OnOpenCommandPalette(InputAction.CallbackContext context)
        {
            using (_PRF_OnOpenCommandPalette.Auto())
            {
                _developerInterfaceCommandPaletteWidget.Show();
                _developerInterfaceCommandSuggestionsWidget.Show();
            }
        }

        public override void OnScreenshot(InputAction.CallbackContext context)
        {
            using (_PRF_OnScreenshot.Auto())
            {
                _developerInterfaceScreenshotService.RequestScreenshot(OnScreenshotCompleted);
            }
        }

        public override void OnToggleActivityBar(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleActivityBar.Auto())
            {
                _developerInterfaceActivityBarWidget.ToggleVisibility();
            }
        }

        public override void OnToggleDeveloperInterface(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleDeveloperInterface.Auto())
            {
                if (IsVisible)
                {
                    HideAreaInterface();
                }
                else
                {
                    ShowAreaInterface();
                }
            }
        }

        public override void OnToggleMenuBar(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleMenuBar.Auto())
            {
                _developerInterfaceMenuBarWidget.ToggleVisibility();
            }
        }

        public override void OnTogglePanel(InputAction.CallbackContext context)
        {
            using (_PRF_OnTogglePanel.Auto())
            {
                _developerInterfacePanelWidget.ToggleVisibility();
            }
        }

        public override void OnToggleSideBar(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleSideBar.Auto())
            {
                _developerInterfaceSideBarWidget.ToggleVisibility();
            }
        }

        public override void OnToggleStatusBar(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleStatusBar.Auto())
            {
                _developerInterfaceStatusBarWidget.ToggleVisibility();
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                _isVisible = !areaMetadata.startHidden;

                _developerInterfaceMenuBarWidget.UpdateSize();
                _developerInterfaceStatusBarWidget.UpdateSize();
                _developerInterfaceActivityBarWidget.UpdateSize();
                _developerInterfaceCommandPaletteWidget.UpdateSize();
                _developerInterfaceSideBarWidget.UpdateSize();
                _developerInterfacePanelWidget.UpdateSize();

                /*
                 initializer.Do(
                    this,
                    nameof(RuntimeGraphManager),
                    runtimeGraphManager == null,
                    () =>
                    {
                        
                            runtimeGraphManager = FindObjectOfType<RuntimeGraphManager>(true);
                            runtimeGraphManager.enabled = true;
                            runtimeGraphManager.gameObject.SetActive(true);
                        
                    }
                );
    
                initializer.Do(
                    this,
                    nameof(DebugLogManager),
                    debugLogManager == null,
                    () =>
                    {
                       
                            debugLogManager = FindObjectOfType<DebugLogManager>(true);
                            debugLogManager.enabled = true;
                            debugLogManager.gameObject.SetActive(true);
                        
                    }
                );
    
                    runtimeGraphManagerCanvasFadeManager = runtimeGraphManager.GetComponent<CanvasFadeManager>();
                    runtimeGraphManagerCanvasGroup = runtimeGraphManager.GetComponent<CanvasGroup>();
                    developerConsoleCanvasFadeManager = debugLogManager.GetComponent<CanvasFadeManager>();
                    developerConsoleCanvasGroup = debugLogManager.GetComponent<CanvasGroup>();
                
                */
            }
        }

        protected override void ResetArea()
        {
            using (_PRF_ResetArea.Auto())
            {
                Context.Log.Info(nameof(ResetArea), this);
            }
        }

        protected override async AppaTask SetFeaturesToInitialState()
        {
            await AppaTask.CompletedTask;
        }

        protected override async AppaTask SetServicesToInitialState()
        {
            await AppaTask.CompletedTask;
        }

        protected override async AppaTask SetWidgetsToInitialState()
        {
            _developerInterfaceGameAreaWidget.Show();

            _developerInterfaceCommandPaletteWidget.Hide();
            _developerInterfaceCommandSuggestionsWidget.Hide();
            _developerInterfacePanelWidget.Hide();
            _developerInterfaceSideBarWidget.Hide();

            _developerInterfaceActivityBarWidget.Show();
            _developerInterfaceStatusBarWidget.Show();
            _developerInterfaceMenuBarWidget.Show();

            await AppaTask.CompletedTask;
        }

        private void OnScreenshotCompleted(ScreenshotCompletedArgs args)
        {
            using (_PRF_OnScreenshotCompleted.Auto())
            {
                Context.Log.Info(
                    $"Screen shot completed.  Saved at {args.screenshotFilePath.FormatForLogging()}."
                );
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_OnScreenshotCompleted =
            new ProfilerMarker(_PRF_PFX + nameof(OnScreenshotCompleted));

        #endregion

        /*[BoxGroup("Runtime Graph"), ReadOnly]
        public RuntimeGraphManager runtimeGraphManager;

        [BoxGroup("Runtime Graph"), ReadOnly]
        public CanvasFadeManager runtimeGraphManagerCanvasFadeManager;

        [BoxGroup("Runtime Graph"), ReadOnly]
        public CanvasGroup runtimeGraphManagerCanvasGroup;

        [FormerlySerializedAs("developerConsoleManager")]
        [BoxGroup("Developer Console"), ReadOnly]
        public DebugLogManager debugLogManager;

        [BoxGroup("Developer Console"), ReadOnly]
        public CanvasFadeManager developerConsoleCanvasFadeManager;

        [BoxGroup("Developer Console"), ReadOnly]
        public CanvasGroup developerConsoleCanvasGroup;*/

        /*
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
        
        public override void OnToggleRuntimeGraph(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleRuntimeGraph.Auto())
            {
                if (!context.performed)
                {
                    return;
                }

                Context.Log.Info(nameof(OnToggleRuntimeGraph), this);

                RuntimeGraphManager.instance.ToggleActive();
            }
        }

        public override void OnToggleRuntimeGraphMode(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleRuntimeGraphMode.Auto())
            {
                if (!context.performed)
                {
                    return;
                }

                Context.Log.Info(nameof(OnToggleRuntimeGraphMode), this);

                RuntimeGraphManager.instance.CycleThroughPresets();
            }
        }
        */
    }
}
