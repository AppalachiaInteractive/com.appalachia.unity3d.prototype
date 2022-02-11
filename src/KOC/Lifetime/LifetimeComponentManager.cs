using System;
using System.Collections.Generic;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Behaviours;
using Appalachia.Prototype.KOC.Input;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Core;
using Appalachia.Utility.Async;
using Appalachia.Utility.Constants;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime
{
    [ExecuteAlways]
    [ExecutionOrder(ExecutionOrders.LifetimeComponentManager)]
    [Serializable, DoNotReorderFields]
    [CallStaticConstructorInEditor]
    public sealed partial class
        LifetimeComponentManager : GlobalSingletonAppalachiaBehaviour<LifetimeComponentManager>
    {
        #region Constants and Static Readonly

        private const string GROUP_BASE = GROUP_COMPONENTS + "/";
        private const string GROUP_COMPONENTS = "Components";
        private const string GROUP_GENERAL = GROUP_BASE + "General";
        private const string GROUP_METADATA = "Metadata";

        #endregion

        static LifetimeComponentManager()
        {
            RegisterDependency<LifetimeMetadata>(i => { _lifetimeMetadata = i; });

            _featureSet = new LifetimeFeatureSet();
        }

        #region Static Fields and Autoproperties

        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        [ShowInInspector]
        private static LifetimeMetadata _lifetimeMetadata;

        #endregion

        #region Fields and Autoproperties

        private Queue<Action> _nextFrameActions;

        [SerializeField] private AppalachiaRepository _repository;

        #endregion

        public ControlScheme CurrentControlScheme
        {
            get
            {
                switch (PlayerInput.currentControlScheme)
                {
                    case "Gamepad":
                        return ControlScheme.Gamepad;
                    case "KeyboardMouse":
                        return ControlScheme.KeyboardMouse;
                    default:
                        throw new NotSupportedException(PlayerInput.currentControlScheme);
                }
            }
        }

        public LifetimeMetadata lifetimeMetadata => _lifetimeMetadata;

        protected override bool DestroyObjectOfSubsequentInstances => true;

        protected override bool ReInitializeOnEnable => true;

        #region Event Functions

        protected override void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (ShouldSkipUpdate)
                {
                    return;
                }

                base.Update();

                if (!AppalachiaApplication.IsPlayingOrWillPlay)
                {
                    if (!gameObject.scene.isLoaded)
                    {
                        gameObject.MoveToLoadedScene();
                    }

                    return;
                }

                ExecuteDelayedActions();

                ExecuteInitializationActions();
            }
        }

        #endregion

        public void DoNextFrame(Action action)
        {
            using (_PRF_DoNextFrame.Auto())
            {
                _nextFrameActions ??= new Queue<Action>();

                _nextFrameActions.Enqueue(action);
            }
        }

        public void RunAsMainScene()
        {
            using (_PRF_RunAsMainScene.Auto())
            {
                SetFlags(gameObject, ApplicationManager.MAIN_SCENE_FLAGS);
            }
        }

        public void RunAsSubScene()
        {
            using (_PRF_RunAsSubScene.Auto())
            {
                SetFlags(gameObject, ApplicationManager.SUBSCENE_FLAGS);
            }
        }

        internal KOCInputActions GetActions()
        {
            using (_PRF_GetActions.Auto())
            {
                return InputActions;
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            await base.Initialize(initializer);

            Context.Log.Info(nameof(Initialize), gameObject);

            enabled = true;

            gameObject.name = nameof(LifetimeComponentManager);
            gameObject.SetActive(true);

            DontDestroyOnLoadSafe();
            ExecuteInitializationActions();

            try
            {
                await AppaTask.WaitUntil(() => DependenciesAreReady);

                using (_PRF_Initialize.Auto())
                {
                    ExecuteInitialization(InitializeSystems,        nameof(InitializeSystems));
                    ExecuteInitialization(InitializeWorld,          nameof(InitializeWorld));
                    ExecuteInitialization(InitializeMeshes,         nameof(InitializeMeshes));
                    ExecuteInitialization(InitializeEventsAndInput, nameof(InitializeEventsAndInput));
                    ExecuteInitialization(InitializeGraphics,       nameof(InitializeGraphics));
                    ExecuteInitialization(InitializeUI,             nameof(InitializeUI));
                    await ExecuteInitialization(InitializeFeatures, nameof(InitializeFeatures));
#if UNITY_EDITOR
                    ExecuteInitialization(InitializeEditorOnly, nameof(InitializeEditorOnly));
#endif

                    _systemsObject.transform.SetSiblingIndex(0);
                    _worldObject.transform.SetSiblingIndex(1);
                    _meshesObject.transform.SetSiblingIndex(2);
                    _eventsObject.transform.SetSiblingIndex(3);
                    _graphicsObject.transform.SetSiblingIndex(4);
                    _uiObject.transform.SetSiblingIndex(5);
                    _featuresObject.transform.SetSiblingIndex(6);
#if UNITY_EDITOR
                    _editorObject.transform.SetSiblingIndex(7);
#endif
                }
            }
            catch (Exception ex)
            {
                Context.Log.Error("Exception while initializing lifetime components.", this, ex);

                gameObject.SetActive(false);
                AppalachiaApplication.IsPlaying = false;

                throw;
            }

            if (RunningAsSubScene)
            {
                RunAsSubScene();
            }
            else
            {
                RunAsMainScene();
            }
        }

        private static void SetFlags(GameObject gameObject, HideFlags flags)
        {
            using (_PRF_SetFlags.Auto())
            {
                if (gameObject.hideFlags != flags)
                {
                    gameObject.hideFlags = flags;
                }
            }
        }

        private void ExecuteDelayedActions()
        {
            using (_PRF_ExecuteDelayedActions.Auto())
            {
                _nextFrameActions ??= new Queue<Action>();

                while (_nextFrameActions.Count > 0)
                {
                    try
                    {
                        _nextFrameActions.Dequeue()();
                    }
                    catch (Exception ex)
                    {
                        Context.Log.Error("Failed to process queued action.", this, ex);
                    }
                }
            }
        }

        private async AppaTask ExecuteInitialization(Func<AppaTask> execute, string methodName)
        {
            using (_PRF_ExecuteInitialization.Auto())
            {
                try
                {
                    await execute();
                }
                catch (Exception ex)
                {
                    Context.Log.Error(methodName.GenericMethodException(this), this, ex);
                    throw;
                }
            }
        }

        private void ExecuteInitialization(Action execute, string methodName)
        {
            using (_PRF_ExecuteInitialization.Auto())
            {
                try
                {
                    execute();
                }
                catch (Exception ex)
                {
                    Context.Log.Error(methodName.GenericMethodException(this), this, ex);
                    throw;
                }
            }
        }

        private void ExecuteInitializationActions()
        {
            using (_PRF_ExecuteInitializationActions.Auto())
            {
                try
                {
                    while (AppalachiaBase.InitializationFunctions.Count > 0)
                    {
                        var initializationFunction = AppalachiaBase.InitializationFunctions.Dequeue();
                        var initializationTask = initializationFunction();

                        initializationTask.Forget();
                    }
                }
                catch (Exception ex)
                {
                    Context.Log.Error($"Exception in {nameof(ExecuteInitializationActions)}.", this, ex);
                }
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_ExecuteInitialization =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteInitialization));

        private static readonly ProfilerMarker _PRF_SetFlags =
            new ProfilerMarker(_PRF_PFX + nameof(SetFlags));

        private static readonly ProfilerMarker _PRF_DoNextFrame =
            new ProfilerMarker(_PRF_PFX + nameof(DoNextFrame));

        private static readonly ProfilerMarker _PRF_ExecuteDelayedActions =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteDelayedActions));

        private static readonly ProfilerMarker _PRF_ExecuteInitializationActions =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteInitializationActions));

        private static readonly ProfilerMarker _PRF_GetActions =
            new ProfilerMarker(_PRF_PFX + nameof(GetActions));

        private static readonly ProfilerMarker _PRF_RunAsMainScene =
            new ProfilerMarker(_PRF_PFX + nameof(RunAsMainScene));

        private static readonly ProfilerMarker _PRF_RunAsSubScene =
            new ProfilerMarker(_PRF_PFX + nameof(RunAsSubScene));

        #endregion
    }
}
