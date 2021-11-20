using Appalachia.Core.Scriptables;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Application.Components
{
    public class LifetimeComponentsAsset : SingletonAppalachiaObject<LifetimeComponentsAsset>
    {
        #region Fields and Autoproperties

        public InputActionAsset inputActions;
        public InputActionReference cancel;
        public InputActionReference leftClick;
        public InputActionReference middleClick;
        public InputActionReference move;

        public InputActionReference point;
        public InputActionReference rightClick;
        public InputActionReference scrollWheel;
        public InputActionReference submit;
        public InputActionReference trackedDeviceOrientation;
        public InputActionReference trackedDevicePosition;

        #endregion
    }
}
