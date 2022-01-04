using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Initialization;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Behaviours;
using Appalachia.Prototype.KOC.Application.Components.Controls;
using Appalachia.Prototype.KOC.Application.Input;
using Appalachia.Utility.Async;
using Appalachia.Utility.Execution;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components
{
    [ExecuteAlways]
    [ExecutionOrder(ExecutionOrders.LifetimeComponentManager)]
    public class LifetimeComponentManager : GlobalSingletonAppalachiaBehaviour<LifetimeComponentManager>
    {
        #region Fields and Autoproperties

        [SerializeField]
        [HideLabel, InlineProperty, Title("Lifetime Components")]
        private LifetimeComponents _components;

        private Queue<Action> _nextFrameActions;

        #endregion

        public ControlScheme CurrentControlScheme
        {
            get
            {
                switch (Components.PlayerInput.currentControlScheme)
                {
                    case "Gamepad":
                        return ControlScheme.Gamepad;
                    case "KeyboardMouse":
                        return ControlScheme.KeyboardMouse;
                    default:
                        throw new NotSupportedException(Components.PlayerInput.currentControlScheme);
                }
            }
        }

        public LifetimeComponents Components
        {
            get
            {
                if (_components == null)
                {
                    _components = new LifetimeComponents(this);
                }

                return _components;
            }
        }

        protected override bool DestroyObjectOfSubsequentInstances => true;

        #region Event Functions

        protected override void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (!DependenciesAreReady || !FullyInitialized)
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

        internal KOCInputActions GetActions()
        {
            using (_PRF_GetActions.Auto())
            {
                return InputActions;
            }
        }

        protected override async AppaTask Initialize(Initializer initializer)
        {
            using (_PRF_Initialize.Auto())
            {
                await base.Initialize(initializer);

                Context.Log.Info(nameof(Initialize), this);

                enabled = true;

                var obj = gameObject;

                obj.name = nameof(LifetimeComponentManager);
                obj.SetActive(true);

                DontDestroyOnLoadSafe();

                var components = Components;

                ExecuteInitializationActions();
                
                try
                {
                    await components.Initialize(this);
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
                    Components.RunAsSubScene(obj);
                }
                else
                {
                    Components.RunAsMainScene(obj);
                }
            }
        }

        protected override async AppaTask WhenDisabled()
        {
            using (_PRF_OnDisable.Auto())
            {
                await base.WhenDisabled();
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

        private const string _PRF_PFX = nameof(LifetimeComponentManager) + ".";

        private static readonly ProfilerMarker _PRF_ExecuteDelayedActions =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteDelayedActions));

        private static readonly ProfilerMarker _PRF_ExecuteInitializationActions =
            new ProfilerMarker(_PRF_PFX + nameof(ExecuteInitializationActions));

        private static readonly ProfilerMarker _PRF_DoNextFrame =
            new ProfilerMarker(_PRF_PFX + nameof(DoNextFrame));

        private static readonly ProfilerMarker _PRF_WhenDestroyed =
            new ProfilerMarker(_PRF_PFX + nameof(WhenDestroyed));

        private static readonly ProfilerMarker _PRF_WhenEnabled =
            new ProfilerMarker(_PRF_PFX + nameof(WhenEnabled));

        private static readonly ProfilerMarker _PRF_WhenDisabled =
            new ProfilerMarker(_PRF_PFX + nameof(WhenDisabled));

        private static readonly ProfilerMarker _PRF_GetActions =
            new ProfilerMarker(_PRF_PFX + nameof(GetActions));

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        private static readonly ProfilerMarker _PRF_Update = new ProfilerMarker(_PRF_PFX + nameof(Update));

        private static readonly ProfilerMarker
            _PRF_OnEnable = new ProfilerMarker(_PRF_PFX + nameof(OnEnable));

        private static readonly ProfilerMarker _PRF_OnDisable =
            new ProfilerMarker(_PRF_PFX + nameof(OnDisable));

        #endregion
    }
}
