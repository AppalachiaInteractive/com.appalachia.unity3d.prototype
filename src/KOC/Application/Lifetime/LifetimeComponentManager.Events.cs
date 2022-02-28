using Appalachia.Prototype.KOC.Extensions;
using Appalachia.Utility.Events;
using Appalachia.Utility.Events.Extensions;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Appalachia.Prototype.KOC.Application.Lifetime
{
    public partial class LifetimeComponentManager
    {
        #region Constants and Static Readonly

        private const string GROUP_EVENTSANDINPUT = GROUP_BASE + PARENT_NAME_EVENTSANDINPUT;

        private const string PARENT_NAME_EVENTSANDINPUT = "Events And Input";

        #endregion

        #region Fields and Autoproperties

        public AppaEvent<EventSystem>.Data EventSystemReady;
        public AppaEvent<InputSystemUIInputModule>.Data InputSystemUIInputModuleReady;
        public AppaEvent<PlayerInput>.Data PlayerInputReady;

        [FoldoutGroup(GROUP_EVENTSANDINPUT)]
        [SerializeField]
        private EventSystem _eventSystem;

        [FoldoutGroup(GROUP_EVENTSANDINPUT)]
        [SerializeField]
        private InputSystemUIInputModule _inputSystemUIInputModule;

        [FoldoutGroup(GROUP_EVENTSANDINPUT)]
        [SerializeField]
        private PlayerInput _playerInput;

        [FoldoutGroup(GROUP_EVENTSANDINPUT), SerializeField]
        private GameObject _eventsObject;

        #endregion

        public EventSystem EventSystem => _eventSystem;

        public GameObject EventsObject => _eventsObject;
        public InputSystemUIInputModule InputSystemUIInputModule => _inputSystemUIInputModule;
        public PlayerInput PlayerInput => _playerInput;

        private void InitializeEventsAndInput()
        {
            using (_PRF_InitializeEventsAndInput.Auto())
            {
                gameObject.GetOrAddChild(ref _eventsObject, PARENT_NAME_EVENTSANDINPUT, false);

                _eventsObject.GetOrAddLifetimeComponentInChild(ref _eventSystem, nameof(EventSystem));

                _eventSystem.gameObject.GetOrAddComponent(ref _inputSystemUIInputModule);
                _eventSystem.gameObject.GetOrAddComponent(ref _playerInput);

                _eventSystem.enabled = false;
                _playerInput.enabled = false;
                _inputSystemUIInputModule.enabled = false;

                var actions = GetActions();
                var asset = actions.asset;

/*
#if UNITY_EDITOR
                InitializeEventsAndInputEditor(asset, actions);
#endif
*/

                _playerInput.uiInputModule = _inputSystemUIInputModule;
                _playerInput.actions = asset;
                _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
                _playerInput.neverAutoSwitchControlSchemes = false;

                _eventSystem.enabled = true;
                _inputSystemUIInputModule.enabled = true;
                _playerInput.enabled = true;

                _inputSystemUIInputModule.UnassignActions();
                _inputSystemUIInputModule.actionsAsset = asset;

                _inputSystemUIInputModule.point =
                    asset.FindAction(actions.GenericMenu.Point.id).GetReference();
                _inputSystemUIInputModule.leftClick =
                    asset.FindAction(actions.GenericMenu.Select.id).GetReference();
                _inputSystemUIInputModule.rightClick =
                    asset.FindAction(actions.GenericMenu.Menu.id).GetReference();
                _inputSystemUIInputModule.middleClick =
                    asset.FindAction(actions.GenericMenu.AlternateSelect.id).GetReference();
                _inputSystemUIInputModule.scrollWheel =
                    asset.FindAction(actions.GenericMenu.Scroll.id).GetReference();
                _inputSystemUIInputModule.move =
                    asset.FindAction(actions.GenericMenu.Navigate.id).GetReference();
                _inputSystemUIInputModule.submit =
                    asset.FindAction(actions.GenericMenu.Select.id).GetReference();
                _inputSystemUIInputModule.cancel =
                    asset.FindAction(actions.GenericMenu.Cancel.id).GetReference();

                EventSystemReady.RaiseEvent(_eventSystem);
                InputSystemUIInputModuleReady.RaiseEvent(_inputSystemUIInputModule);
                PlayerInputReady.RaiseEvent(_playerInput);
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitializeEventsAndInput =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeEventsAndInput));

        #endregion
    }
}
