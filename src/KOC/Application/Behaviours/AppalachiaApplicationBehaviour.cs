using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Behaviours;
using Appalachia.Prototype.KOC.Application.Components;
using Appalachia.Prototype.KOC.Application.Input;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Behaviours
{
    [SmartLabelChildren]
    [ExecuteAlways]
    public class AppalachiaApplicationBehaviour : AppalachiaBehaviour
    {
        protected static ApplicationLifetimeComponents LifetimeComponents => ApplicationLifetimeComponents.instance;

        protected static KOCInputActions InputActions => ApplicationLifetimeComponents.instance.InputActions;

        protected override bool InitializeAlways => true;
    }
}
