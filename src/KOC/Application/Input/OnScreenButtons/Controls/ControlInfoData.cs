using System;
using Appalachia.CI.Integration.Attributes;
using Appalachia.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Appalachia.Prototype.KOC.Application.Input.OnScreenButtons.Controls
{
    [Serializable, DoNotReorderFields]
    public struct ControlInfoData
    {
        public ControlInfoData(InputControl control)
        {
            name = control.name;
            displayName = control.displayName;
            shortDisplayName = control.shortDisplayName;
            path = control.path;
        }

        #region Fields and Autoproperties

        [ReadOnly, SerializeField]
        public string name;

        [ReadOnly, SerializeField]
        public string displayName;

        [ReadOnly, SerializeField]
        public string shortDisplayName;

        [ReadOnly, SerializeField]
        public string path;

        #endregion

        public override string ToString()
        {
            return
                $"{{ name: \"{name}\", displayName: \"{displayName}\", shortDisplayName: \"{shortDisplayName}\", path: \"{path}\" }}\"";
        }

        public string GetDisplayText(InputAction action, OnScreenButtonTextStyle style)
        {
            while (true)
            {
                switch (style)
                {
                    case OnScreenButtonTextStyle.ActionName:
                        return action.name;

                    case OnScreenButtonTextStyle.ShortDisplayName:

                        if (shortDisplayName.IsNullOrWhiteSpace())
                        {
                            style = OnScreenButtonTextStyle.DisplayName;
                            continue;
                        }

                        return shortDisplayName;

                    case OnScreenButtonTextStyle.DisplayName:

                        if (displayName.IsNullOrWhiteSpace())
                        {
                            style = OnScreenButtonTextStyle.Name;
                            continue;
                        }

                        return displayName;
                    case OnScreenButtonTextStyle.Name:
                        return name;
                    case OnScreenButtonTextStyle.None:
                        return string.Empty;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(style), style, null);
                }
            }
        }
    }
}
