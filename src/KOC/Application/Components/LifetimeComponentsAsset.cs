using Appalachia.Core.Scriptables;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Application.Components
{
    public class LifetimeComponentsAsset : SingletonAppalachiaObject<LifetimeComponentsAsset>
    {
        public InputActionAsset inputActions;
        
        public InputActionReference point;
        public InputActionReference leftClick;
        public InputActionReference middleClick;
        public InputActionReference rightClick;
        public InputActionReference scrollWheel;
        public InputActionReference move;
        public InputActionReference submit;
        public InputActionReference cancel;
        public InputActionReference trackedDevicePosition;
        public InputActionReference trackedDeviceOrientation;
    }
}
