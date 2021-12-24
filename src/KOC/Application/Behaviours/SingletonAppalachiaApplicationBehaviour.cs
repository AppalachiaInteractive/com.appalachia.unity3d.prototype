using System;
using Appalachia.Core.Attributes;
using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Application.Components;
using Appalachia.Prototype.KOC.Application.Input;
using Sirenix.OdinInspector;
using Unity.Profiling;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Behaviours
{
    // [SmartLabelChildren]
    [Serializable, ExecuteAlways]
    [CallStaticConstructorInEditor]
    public abstract class SingletonAppalachiaApplicationBehaviour<T> : SingletonAppalachiaBehaviour<T>
        where T : SingletonAppalachiaApplicationBehaviour<T>
    {
        static SingletonAppalachiaApplicationBehaviour()
        {
            RegisterDependency<LifetimeComponentManager>(i => _lifetimeComponentManager = i);
        }

        #region Static Fields and Autoproperties

        private static LifetimeComponentManager _lifetimeComponentManager;

        #endregion

        #region Fields and Autoproperties

        [FoldoutGroup("Events & Input")]
        private KOCInputActions _inputActions;

        #endregion

        protected static LifetimeComponents LifetimeComponents => _lifetimeComponentManager.Components;

        protected KOCInputActions InputActions
        {
            get
            {
                if (_inputActions == null)
                {
                    _inputActions = new KOCInputActions();
                }

                return _inputActions;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(SingletonAppalachiaApplicationBehaviour<T>) + ".";

        private static readonly ProfilerMarker _PRF_Initialize =
            new ProfilerMarker(_PRF_PFX + nameof(Initialize));

        #endregion
    }
}
