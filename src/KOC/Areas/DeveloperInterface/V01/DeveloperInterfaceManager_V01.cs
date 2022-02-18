using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.ActivityBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.CommandPalette;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperInfo;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.GameArea;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.MenuBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Panel;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.RectVisualizer;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.Screenshot;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.SideBar;
using Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.StatusBar;
using Appalachia.Utility.Async;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01
{
    [CallStaticConstructorInEditor]
    public sealed partial class DeveloperInterfaceManager_V01 : DeveloperInterfaceManager<
        DeveloperInterfaceManager_V01, DeveloperInterfaceMetadata_V01>
    {
        static DeveloperInterfaceManager_V01()
        {
            FeatureSet.RegisterFeature<ActivityBarFeature>(_dependencyTracker, i => _activityBar = i);
            FeatureSet.RegisterFeature<CommandPaletteFeature>(_dependencyTracker, i => _commandPalette = i);
            FeatureSet.RegisterFeature<GameAreaFeature>(_dependencyTracker, i => _gameArea = i);
            FeatureSet.RegisterFeature<MenuBarFeature>(_dependencyTracker, i => _menuBar = i);
            FeatureSet.RegisterFeature<PanelFeature>(_dependencyTracker, i => _panel = i);
            FeatureSet.RegisterFeature<ScreenshotFeature>(_dependencyTracker, i => _screenshot = i);
            FeatureSet.RegisterFeature<SideBarFeature>(_dependencyTracker, i => _sideBar = i);
            FeatureSet.RegisterFeature<StatusBarFeature>(_dependencyTracker, i => _statusBar = i);
            FeatureSet.RegisterFeature<DeveloperInfoFeature>(_dependencyTracker, i => _developerInfo = i);
            FeatureSet.RegisterFeature<RectVisualizerFeature>(_dependencyTracker, i => _rectVisualizer = i);
        }

        #region Static Fields and Autoproperties

        private static ActivityBarFeature _activityBar;
        private static CommandPaletteFeature _commandPalette;
        private static DeveloperInfoFeature _developerInfo;
        private static GameAreaFeature _gameArea;
        private static MenuBarFeature _menuBar;
        private static PanelFeature _panel;

        private static RectVisualizerFeature _rectVisualizer;
        private static ScreenshotFeature _screenshot;
        private static SideBarFeature _sideBar;
        private static StatusBarFeature _statusBar;

        #endregion

        #region Fields and Autoproperties

        private bool _isVisible;

        #endregion

        /// <inheritdoc />
        public override AreaVersion Version => AreaVersion.V01;

        public bool IsVisible => _isVisible;

        /// <inheritdoc />
        public override void OnOpenCommandPalette(InputAction.CallbackContext context)
        {
            using (_PRF_OnOpenCommandPalette.Auto())
            {
                _commandPalette.ShowFeature().Forget();
            }
        }

        /// <inheritdoc />
        public override void OnScreenshot(InputAction.CallbackContext context)
        {
            using (_PRF_OnScreenshot.Auto())
            {
                _screenshot.RequestScreenshot();
            }
        }

        /// <inheritdoc />
        public override void OnToggleActivityBar(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleActivityBar.Auto())
            {
                _activityBar.ToggleVisibility().Forget();
            }
        }

        /// <inheritdoc />
        public override void OnToggleDeveloperInterface(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleDeveloperInterface.Auto())
            {
                if (IsVisible)
                {
                    HideAreaInterface().Forget();
                }
                else
                {
                    ShowAreaInterface().Forget();
                }
            }
        }

        /// <inheritdoc />
        public override void OnToggleMenuBar(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleMenuBar.Auto())
            {
                _menuBar.ToggleVisibility().Forget();
            }
        }

        /// <inheritdoc />
        public override void OnTogglePanel(InputAction.CallbackContext context)
        {
            using (_PRF_OnTogglePanel.Auto())
            {
                _panel.ToggleVisibility().Forget();
            }
        }

        /// <inheritdoc />
        public override void OnToggleSideBar(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleSideBar.Auto())
            {
                _sideBar.ToggleVisibility().Forget();
            }
        }

        /// <inheritdoc />
        public override void OnToggleStatusBar(InputAction.CallbackContext context)
        {
            using (_PRF_OnToggleStatusBar.Auto())
            {
                _statusBar.ToggleVisibility().Forget();
            }
        }

        /// <inheritdoc />
        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            using (_PRF_Initialize.Auto())
            {
                _isVisible = !areaMetadata.startHidden;
            }
        }

        /// <inheritdoc />
        protected override void ResetArea()
        {
            using (_PRF_ResetArea.Auto())
            {
                Context.Log.Info(nameof(ResetArea), this);
            }
        }

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
         /// <inheritdoc />
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
        
        /// <inheritdoc />
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

        /// <inheritdoc />
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
