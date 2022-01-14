using UnityEngine;

namespace Appalachia.KOC.Character
{
    public static class BOTDPlayerInputMappingExtensions
    {
        public static BOTDPlayerInput GetControllerInput(this IBOTDPlayerInputMapping mapping)
        {
            return new()
            {
                move = new Vector2(Input.GetAxis(mapping.moveX), Input.GetAxis(mapping.moveY)),
                look = new Vector2(Input.GetAxis(mapping.lookY), Input.GetAxis(mapping.lookX)),
                run = Input.GetAxis(mapping.run),
                jump = Input.GetButtonDown(mapping.jump)
            };
        }

        public static BOTDPlayerInput GetKeyboardInput(this IBOTDPlayerInputMapping mapping)
        {
            return new()
            {
                move = new Vector2(Input.GetAxis(mapping.moveX), Input.GetAxis(mapping.moveY)),
                look = new Vector2(Input.GetAxis(mapping.lookY), Input.GetAxis(mapping.lookX)),
                run = Input.GetButton(mapping.run) ? 1f : 0f,
                jump = Input.GetButtonDown(mapping.jump)
            };
        }
    }
}
