using Appalachia.CI.Integration.Assets;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Application.Extensions
{
    public static class InputActionExtensions
    {
        public static InputActionReference[] InputActionReferences =>
            AssetDatabaseManager.FindAssets<InputActionReference>().ToArray();

        public static InputActionReference GetReference(this InputAction action)
        {
            foreach (var reference in InputActionReferences)
            {
                if (reference.action.id == action.id)
                {
                    return reference;
                }
            }

            return null;
        }

        public static string ToFormattedName(this InputActionReference action)
        {
            return action.action.ToFormattedName();
        }

        public static string ToFormattedName(this InputAction action)
        {
            if ((action == null) || (action.actionMap == null))
            {
                return null;
            }

            var mapName = action.actionMap.name.Replace(" ", string.Empty).Trim();
            var actionName = action.name.Replace(" ", string.Empty).Trim();

            return $"{mapName}_{actionName}";
        }

        public static string ToReferenceAssetName(this InputAction action)
        {
            var mapName = action.actionMap.name;
            var actionName = action.name;

            return $"{mapName}/{actionName}";
        }
    }
}
