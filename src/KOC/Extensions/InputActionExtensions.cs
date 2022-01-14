using Appalachia.CI.Integration.Assets;
using Appalachia.Utility.Strings;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Extensions
{
    public static class InputActionExtensions
    {
#if UNITY_EDITOR
        public static InputActionReference[] InputActionReferences =>
            AssetDatabaseManager.FindAssets<InputActionReference>().ToArray();
#endif

#if UNITY_EDITOR
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
#endif

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

            return ZString.Format("{0}_{1}", mapName, actionName);
        }

        public static string ToReferenceAssetName(this InputAction action)
        {
            var mapName = action.actionMap.name;
            var actionName = action.name;

            return ZString.Format("{0}/{1}", mapName, actionName);
        }
    }
}
