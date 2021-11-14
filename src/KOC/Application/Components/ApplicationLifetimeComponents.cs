using System;
using Appalachia.Core.Behaviours;
using Appalachia.Core.Extensions;
using Appalachia.Utility.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Appalachia.Prototype.KOC.Application.Components
{
    [ExecuteAlways]
    public class ApplicationLifetimeComponents : SingletonAppalachiaBehaviour<ApplicationLifetimeComponents>
    {
        [NonSerialized] private bool _initialized;

        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private InputSystemUIInputModule _inputSystemUIInputModule;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private AudioListener _audioListener;

        #region Event Functions

        private void Update()
        {
            if (!_initialized)
            {
                Initialize();
            }
        }

        private void OnDisable()
        {
            _initialized = false;
        }

        #endregion

        protected override bool DestroyObjectOfSubsequentInstances => true;

        private void Initialize()
        {
            _initialized = true;

            var asset = LifetimeComponentsAsset.instance;

            gameObject.name = nameof(ApplicationLifetimeComponents);

            gameObject.GetOrCreateComponent(ref _eventSystem);
            gameObject.GetOrCreateComponent(ref _inputSystemUIInputModule);
            gameObject.GetOrCreateComponent(ref _playerInput);
            gameObject.GetOrCreateComponent(ref _audioListener);

            _inputSystemUIInputModule.actionsAsset = asset.inputActions;

            _playerInput.actions = asset.inputActions;
            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            _playerInput.neverAutoSwitchControlSchemes = false;
            _playerInput.uiInputModule = _inputSystemUIInputModule;

            _inputSystemUIInputModule.point = asset.point;
            _inputSystemUIInputModule.leftClick = asset.leftClick;
            _inputSystemUIInputModule.middleClick = asset.middleClick;
            _inputSystemUIInputModule.rightClick = asset.rightClick;
            _inputSystemUIInputModule.scrollWheel = asset.scrollWheel;
            _inputSystemUIInputModule.move = asset.move;
            _inputSystemUIInputModule.submit = asset.submit;
            _inputSystemUIInputModule.cancel = asset.cancel;
            _inputSystemUIInputModule.trackedDevicePosition =asset.trackedDevicePosition;
            _inputSystemUIInputModule.trackedDeviceOrientation =asset.trackedDeviceOrientation;
        }
    }
}
