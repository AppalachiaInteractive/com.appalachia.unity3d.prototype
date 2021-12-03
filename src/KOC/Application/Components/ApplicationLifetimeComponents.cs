using System;
using System.Collections.Generic;
using Appalachia.Core.Attributes;
using Appalachia.Core.Behaviours;
using Appalachia.Prototype.KOC.Application.Components.Controls;
using Appalachia.Prototype.KOC.Application.Components.Cursors;
using Appalachia.Prototype.KOC.Application.Extensions;
using Appalachia.Prototype.KOC.Application.Input;
using Appalachia.Prototype.KOC.Data;
using Appalachia.Utility.Extensions;
using Appalachia.Utility.Logging;
using Doozy.Engine.UI;
using Doozy.Engine.UI.Base;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Serialization;

namespace Appalachia.Prototype.KOC.Application.Components
{
    [ExecuteAlways]
    [ExecutionOrder(ExecutionOrders.ApplicationLifetimeComponents)]
    public class ApplicationLifetimeComponents : SingletonAppalachiaBehaviour<ApplicationLifetimeComponents>
    {
        #region Fields and Autoproperties

        [FormerlySerializedAs("asset")]
        [InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        public LifetimeMetadata metadata;

        [SerializeField] public UICanvas masterCanvas;
        [SerializeField] private AudioListener _audioListener;
        [NonSerialized] private bool _initialized;
        [SerializeField] private DatabaseManager _databasManager;
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private InputSystemUIInputModule _inputSystemUIInputModule;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private CursorManager _cursorManager;
        private KOCInputActions _inputActions;

        private Queue<Action> _nextFrameActions;

        #endregion

        public AudioListener AudioListener => _audioListener;

        public ControlScheme CurrentControlScheme
        {
            get
            {
                switch (_playerInput.currentControlScheme)
                {
                    case "Gamepad":
                        return ControlScheme.Gamepad;
                    case "KeyboardMouse":
                        return ControlScheme.KeyboardMouse;
                    default:
                        throw new NotSupportedException(_playerInput.currentControlScheme);
                }
            }
        }

        public CursorManager CursorManager => _cursorManager;
        public DatabaseManager DatabaseManager => _databasManager;
        public EventSystem EventSystem => _eventSystem;
        public InputSystemUIInputModule InputSystemUIInputModule => _inputSystemUIInputModule;

        public KOCInputActions InputActions => _inputActions;
        public PlayerInput PlayerInput => _playerInput;

        public UICanvas UICanvas => masterCanvas;

        protected override bool DestroyObjectOfSubsequentInstances => true;

        protected override bool InitializeAlways => true;

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (!_initialized)
                {
                    Initialize();
                }

                _nextFrameActions ??= new Queue<Action>();

                while (_nextFrameActions.Count > 0)
                {
                    try
                    {
                        _nextFrameActions.Dequeue()();
                    }
                    catch (Exception ex)
                    {
                        AppaLog.Exception("Failed to process queued action.", ex);
                    }
                }
            }
        }

        protected override void OnDisable()
        {
            using (_PRF_OnDisable.Auto())
            {
                base.OnDisable();

                _initialized = false;
            }
        }

        #endregion

        protected override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                _initialized = true;

                _inputActions = new KOCInputActions();

                metadata = LifetimeMetadata.instance;

                gameObject.name = nameof(ApplicationLifetimeComponents);

                DoSafe(() => gameObject.CreateOrGetComponent(ref _eventSystem));
                DoSafe(() => gameObject.CreateOrGetComponent(ref _inputSystemUIInputModule));
                DoSafe(() => gameObject.CreateOrGetComponent(ref _playerInput));
                DoSafe(() => gameObject.CreateOrGetComponent(ref _audioListener));
                DoSafe(
                    () => gameObject.CreateOrGetComponentInChild(ref _databasManager, nameof(DatabaseManager))
                );
                DoSafe(
                    () => gameObject.CreateOrGetComponentInChild(ref _cursorManager, nameof(CursorManager))
                );

                DoSafe(
                    () =>
                    {
                        _playerInput.enabled = false;
                        _inputSystemUIInputModule.enabled = false;

                        var asset = LifetimeMetadata.instance.inputActionAsset;

                        _inputSystemUIInputModule.UnassignActions();
                        _inputSystemUIInputModule.actionsAsset = asset;
                        _inputSystemUIInputModule.point = asset.FindAction(InputActions.GenericMenu.Point.id)
                                                               .GetReference();
                        _inputSystemUIInputModule.leftClick = asset
                                                             .FindAction(InputActions.GenericMenu.Click.id)
                                                             .GetReference();
                        _inputSystemUIInputModule.move = asset
                                                        .FindAction(InputActions.GenericMenu.Navigate.id)
                                                        .GetReference();
                        _inputSystemUIInputModule.submit = asset
                                                          .FindAction(InputActions.GenericMenu.Submit.id)
                                                          .GetReference();
                        _inputSystemUIInputModule.cancel = asset
                                                          .FindAction(InputActions.GenericMenu.Cancel.id)
                                                          .GetReference();

                        _inputSystemUIInputModule.enabled = true;

                        _playerInput.uiInputModule = _inputSystemUIInputModule;
                        _playerInput.actions = asset;
                        _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
                        _playerInput.neverAutoSwitchControlSchemes = false;
                        _playerInput.enabled = true;
                    }
                );

                DoSafe(
                    () => gameObject.CreateOrGetComponentInChild(
                        ref masterCanvas,
                        NamesDatabase.MASTER_CANVAS
                    )
                );

                DoSafe(() => _cursorManager.InitializeExternal());
                
                DontDestroyOnLoadSafe(gameObject);
            }
        }

        public void DoNextFrame(Action action)
        {
            _nextFrameActions ??= new Queue<Action>();

            _nextFrameActions.Enqueue(action);
        }

        private void DoSafe(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                AppaLog.Exception(ex);
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ApplicationLifetimeComponents) + ".";

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
