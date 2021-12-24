using Appalachia.Core.Objects.Root;
using Appalachia.Prototype.KOC.Debugging.DebugConsole.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

namespace Appalachia.Prototype.KOC.Debugging.DebugConsole.Commands
{
    public class TimeCommands : AppalachiaSimpleBase
    {
        [ConsoleMethod("time.scale", "Returns the current Time.timeScale value")]
        [Preserve]
        public static float GetTimeScale()
        {
            return Time.timeScale;
        }

        [ConsoleMethod("time.scale", "Sets the Time.timeScale value")]
        [Preserve]
        public static void SetTimeScale(float value)
        {
            Time.timeScale = Mathf.Max(value, 0f);
        }
    }
}
