using System;
using Appalachia.Core.Behaviours;
using Appalachia.Prototype.KOC.Data;
using Appalachia.Utility.Extensions;
using Doozy.Engine.UI;
using Doozy.Engine.UI.Base;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Appalachia.Prototype.KOC.Application.Components
{
    [ExecuteAlways]
    public class ApplicationLifetimeComponents : SingletonAppalachiaBehaviour<ApplicationLifetimeComponents>
    {
        #region Fields and Autoproperties

        public LifetimeComponentsAsset asset;
        [SerializeField] public UICanvas masterCanvas;
        [SerializeField] private AudioListener _audioListener;
        [NonSerialized] private bool _initialized;
        [SerializeField] private DatabaseManager _databasManager;
        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private InputSystemUIInputModule _inputSystemUIInputModule;
        [SerializeField] private PlayerInput _playerInput;

        #endregion

        protected override bool DestroyObjectOfSubsequentInstances => true;

        #region Event Functions

        private void Update()
        {
            using (_PRF_Update.Auto())
            {
                if (!_initialized)
                {
                    Initialize();
                }
            }
        }

        protected override void OnEnable()
        {
            using (_PRF_OnEnable.Auto())
            {
                base.OnEnable();

                Initialize();
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

        public override void Initialize()
        {
            using (_PRF_Initialize.Auto())
            {
                base.Initialize();

                _initialized = true;

                asset = LifetimeComponentsAsset.instance;

                gameObject.name = nameof(ApplicationLifetimeComponents);

                gameObject.GetOrCreateComponent(ref _eventSystem);
                gameObject.GetOrCreateComponent(ref _inputSystemUIInputModule);
                gameObject.GetOrCreateComponent(ref _playerInput);
                gameObject.GetOrCreateComponent(ref _audioListener);
                gameObject.GetOrCreateComponent(ref _databasManager);

                _playerInput.actions = asset.inputActions;
                _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
                _playerInput.neverAutoSwitchControlSchemes = false;
                _playerInput.uiInputModule = _inputSystemUIInputModule;

                AssignDefaultActions();

                gameObject.GetOrCreateComponentInChild(ref masterCanvas, NamesDatabase.MASTER_CANVAS);
            }
        }

        public void AssignDefaultActions()
        {
            using (_PRF_AssignDefaultActions.Auto())
            {
                _inputSystemUIInputModule.actionsAsset = asset.inputActions;

                asset.point.action.Enable();
                asset.leftClick.action.Enable();
                asset.middleClick.action.Enable();
                asset.rightClick.action.Enable();
                asset.scrollWheel.action.Enable();
                asset.move.action.Enable();
                asset.submit.action.Enable();
                asset.cancel.action.Enable();
                asset.trackedDevicePosition.action.Enable();
                asset.trackedDeviceOrientation.action.Enable();

                _inputSystemUIInputModule.point = null;
                _inputSystemUIInputModule.leftClick = null;
                _inputSystemUIInputModule.middleClick = null;
                _inputSystemUIInputModule.rightClick = null;
                _inputSystemUIInputModule.scrollWheel = null;
                _inputSystemUIInputModule.move = null;
                _inputSystemUIInputModule.submit = null;
                _inputSystemUIInputModule.cancel = null;
                _inputSystemUIInputModule.trackedDevicePosition = null;
                _inputSystemUIInputModule.trackedDeviceOrientation = null;

                _inputSystemUIInputModule.point = asset.point;
                _inputSystemUIInputModule.leftClick = asset.leftClick;
                _inputSystemUIInputModule.middleClick = asset.middleClick;
                _inputSystemUIInputModule.rightClick = asset.rightClick;
                _inputSystemUIInputModule.scrollWheel = asset.scrollWheel;
                _inputSystemUIInputModule.move = asset.move;
                _inputSystemUIInputModule.submit = asset.submit;
                _inputSystemUIInputModule.cancel = asset.cancel;
                _inputSystemUIInputModule.trackedDevicePosition = asset.trackedDevicePosition;
                _inputSystemUIInputModule.trackedDeviceOrientation = asset.trackedDeviceOrientation;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(ApplicationLifetimeComponents) + ".";

        private static readonly ProfilerMarker _PRF_AssignDefaultActions =
            new ProfilerMarker(_PRF_PFX + nameof(AssignDefaultActions));

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
