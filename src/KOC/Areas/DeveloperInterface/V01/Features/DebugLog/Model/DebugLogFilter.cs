using System;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugLog.Model
{
    [Flags]
    public enum DebugLogFilter
    {
        None = 0,
        Info = 1,
        Warning = 2,
        Error = 4,
        All = 7
    }
}
