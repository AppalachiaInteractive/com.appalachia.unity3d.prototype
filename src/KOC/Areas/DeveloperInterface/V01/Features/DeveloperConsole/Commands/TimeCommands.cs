using Appalachia.Core.Objects.Root;
using UnityEngine;
using UnityEngine.Scripting;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DeveloperConsole.Commands
{
    public class TimeCommands : AppalachiaSimpleBase
    {
        [DeveloperConsole("time.scale", "Returns the current Time.timeScale value")]
        [Preserve]
        public static float GetTimeScale()
        {
            return Time.timeScale;
        }

        [DeveloperConsole("time.scale", "Sets the Time.timeScale value")]
        [Preserve]
        public static void SetTimeScale(float value)
        {
            Time.timeScale = Mathf.Max(value, 0f);
        }
    }
}
