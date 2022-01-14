using Appalachia.Core.Objects.Root;
using UnityEngine;
using UnityEngine.Scripting;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperConsole.Commands
{
    public class PlayerPrefsCommands : AppalachiaSimpleBase
    {
        [DeveloperConsole("prefs.clear", "Deletes all PlayerPrefs fields")]
        [Preserve]
        public static void PlayerPrefsClear()
        {
            PlayerPrefs.DeleteAll();
        }

        [DeveloperConsole("prefs.delete", "Deletes a PlayerPrefs field")]
        [Preserve]
        public static void PlayerPrefsDelete(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        [DeveloperConsole("prefs.float", "Returns the value of a Float PlayerPrefs field")]
        [Preserve]
        public static string PlayerPrefsGetFloat(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return "Key Not Found";
            }

            return PlayerPrefs.GetFloat(key).ToString();
        }

        [DeveloperConsole("prefs.int", "Returns the value of an Integer PlayerPrefs field")]
        [Preserve]
        public static string PlayerPrefsGetInt(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return "Key Not Found";
            }

            return PlayerPrefs.GetInt(key).ToString();
        }

        [DeveloperConsole("prefs.string", "Returns the value of a String PlayerPrefs field")]
        [Preserve]
        public static string PlayerPrefsGetString(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return "Key Not Found";
            }

            return PlayerPrefs.GetString(key);
        }

        [DeveloperConsole("prefs.float", "Sets the value of a Float PlayerPrefs field")]
        [Preserve]
        public static void PlayerPrefsSetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        [DeveloperConsole("prefs.int", "Sets the value of an Integer PlayerPrefs field")]
        [Preserve]
        public static void PlayerPrefsSetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        [DeveloperConsole("prefs.string", "Sets the value of a String PlayerPrefs field")]
        [Preserve]
        public static void PlayerPrefsSetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }
    }
}
