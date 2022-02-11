using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Appalachia.Prototype.KOC.Lifetime
{
    public partial class LifetimeComponentManager
    {
        #region Constants and Static Readonly

        private const string GROUP_EVENTSANDINPUT = GROUP_BASE + PARENT_NAME_EVENTSANDINPUT;

        private const string PARENT_NAME_EVENTSANDINPUT = "Events And Input";

        #endregion

        #region Fields and Autoproperties

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
                _eventSystem.enabled = true;

                _playerInput.enabled = false;
                _inputSystemUIInputModule.enabled = false;

                var actions = GetActions();
                var asset = actions.asset;

#if UNITY_EDITOR
                InitializeEventsAndInputEditor(asset, actions);
#endif

                _inputSystemUIInputModule.enabled = true;

                _playerInput.uiInputModule = _inputSystemUIInputModule;
                _playerInput.actions = asset;
                _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
                _playerInput.neverAutoSwitchControlSchemes = false;
                _playerInput.enabled = true;
            }
        }

        #region Profiling

        private static readonly ProfilerMarker _PRF_InitializeEventsAndInput =
            new ProfilerMarker(_PRF_PFX + nameof(InitializeEventsAndInput));

        #endregion
    }
}
