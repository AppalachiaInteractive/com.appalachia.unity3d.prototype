using System;
using Appalachia.Core.Behaviours;
using Appalachia.Core.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace Appalachia.Prototype.KOCPrototype.Application.Components
{
    [ExecuteAlways]
    public class ApplicationLifetimeComponents : SingletonAppalachiaBehaviour<ApplicationLifetimeComponents>
    {
        [NonSerialized] private bool _initialized;

        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private InputSystemUIInputModule _inputSystemUIInputModule;

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

        private void Initialize()
        {
            _initialized = true;

            gameObject.name = nameof(ApplicationLifetimeComponents);

            gameObject.GetOrCreateComponent(ref _eventSystem);
            gameObject.GetOrCreateComponent(ref _inputSystemUIInputModule);

            _inputSystemUIInputModule.actionsAsset = LifetimeComponentsAsset.instance.inputActions;
        }
    }
}
