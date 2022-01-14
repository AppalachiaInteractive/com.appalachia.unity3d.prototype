using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RuntimeGraphs;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Services.Screenshots;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.ActivityBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.CommandPalette;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.CommandSuggestions;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.GameArea;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.MenuBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.Panel;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.SideBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Widgets.StatusBar;
using Appalachia.Prototype.KOC.Components.Fading;
using Appalachia.Prototype.KOC.Components.UI;
using Appalachia.Utility.Async;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01
{
    [CallStaticConstructorInEditor]
    public sealed class DeveloperInterfaceManager_V01 : DeveloperInterfaceManager<
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        static DeveloperInterfaceManager_V01()
        {
            RegisterService<DeveloperInterfaceScreenshotService>(
                i => _developerInterfaceScreenshotService = i
            );

            RegisterWidget<DeveloperInterfacePanelWidget>(i => _developerInterfacePanelWidget = i);
            RegisterWidget<DeveloperInterfaceActivityBarWidget>(
                i => _developerInterfaceActivityBarWidget = i
            );
            RegisterWidget<DeveloperInterfaceCommandPaletteWidget>(
                i => _developerInterfaceCommandPaletteWidget = i
            );
            RegisterWidget<DeveloperInterfaceMenuBarWidget>(i => _developerInterfaceMenuBarWidget = i);
            RegisterWidget<DeveloperInterfaceSideBarWidget>(i => _developerInterfaceSideBarWidget = i);
            RegisterWidget<DeveloperInterfaceStatusBarWidget>(i => _developerInterfaceStatusBarWidget = i);
            RegisterWidget<DeveloperInterfaceGameAreaWidget>(i => _developerInterfaceGameAreaWidget = i);
            RegisterWidget<DeveloperInterfaceCommandSuggestionsWidget>(
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

        [BoxGroup("Runtime Graph"), ReadOnly]
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
        public CanvasGroup developerConsoleCanvasGroup;

        [BoxGroup("Game View"), ReadOnly]
        [SerializeField, FoldoutGroup(APPASTR.Components)]
        private UIViewComponentSet gameView;

        #endregion

        public override AreaVersion Version => AreaVersion.V01;

        public bool IsVisible => _isVisible;

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

        public override void OnOpenCommandPalette(InputAction.CallbackContext context)
        {
            using (_PRF_OnOpenCommandPalette.Auto())
            {
                _developerInterfaceCommandPaletteWidget.Show();
                _developerInterfaceCommandSuggestionsWidget.Show();
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

        #region Profiling

        private static readonly ProfilerMarker _PRF_OnOpenCommandPalette =
            new ProfilerMarker(_PRF_PFX + nameof(OnOpenCommandPalette));

        private static readonly ProfilerMarker _PRF_OnToggleActivityBar =
            new ProfilerMarker(_PRF_PFX + nameof(OnToggleActivityBar));

        private static readonly ProfilerMarker _PRF_OnToggleDeveloperInterface =
            new ProfilerMarker(_PRF_PFX + nameof(OnToggleDeveloperInterface));

        private static readonly ProfilerMarker _PRF_OnToggleMenuBar =
            new ProfilerMarker(_PRF_PFX + nameof(OnToggleMenuBar));

        private static readonly ProfilerMarker _PRF_OnTogglePanel =
            new ProfilerMarker(_PRF_PFX + nameof(OnTogglePanel));

        private static readonly ProfilerMarker _PRF_OnToggleSideBar =
            new ProfilerMarker(_PRF_PFX + nameof(OnToggleSideBar));

        private static readonly ProfilerMarker _PRF_OnToggleStatusBar =
            new ProfilerMarker(_PRF_PFX + nameof(OnToggleStatusBar));

        private static readonly ProfilerMarker _PRF_ResetArea =
            new ProfilerMarker(_PRF_PFX + nameof(ResetArea));

        #endregion

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
