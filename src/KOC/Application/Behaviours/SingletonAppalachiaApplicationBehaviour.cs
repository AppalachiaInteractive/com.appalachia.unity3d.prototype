using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Behaviours;
using Appalachia.Prototype.KOC.Application.Components;
using Appalachia.Prototype.KOC.Application.Input;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Behaviours
{
    [SmartLabelChildren]
    [Serializable, ExecuteAlways]
    [InspectorIcon(Icons.Squirrel.Bone)]
    public abstract class SingletonAppalachiaApplicationBehaviour<T> : SingletonAppalachiaBehaviour<T>
        where T : SingletonAppalachiaApplicationBehaviour<T>
    {
        protected static ApplicationLifetimeComponents LifetimeComponents => ApplicationLifetimeComponents.instance;

        protected static KOCInputActions InputActions => ApplicationLifetimeComponents.instance.InputActions;

        protected override bool InitializeAlways => true;
    }
}
