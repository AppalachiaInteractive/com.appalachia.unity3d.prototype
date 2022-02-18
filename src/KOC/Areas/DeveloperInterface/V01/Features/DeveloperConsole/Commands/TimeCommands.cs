using Appalachia.Utility.Timing;
using UnityEngine;
using UnityEngine.Scripting;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperConsole.Commands
{
    [DeveloperConsoleMethodProvider]
    public class TimeCommands
    {
        [DeveloperConsoleMethod("time.scale", "Returns the current CoreClock.Instance.TimeScale value")]
        [Preserve]
        public static float GetTimeScale()
        {
            return CoreClock.Instance.TimeScale;
        }

        [DeveloperConsoleMethod("time.scale", "Sets the CoreClock.Instance.TimeScale value")]
        [Preserve]
        public static void SetTimeScale(float value)
        {
            CoreClock.Instance.TimeScale = Mathf.Max(value, 0f);
        }
    }
}
