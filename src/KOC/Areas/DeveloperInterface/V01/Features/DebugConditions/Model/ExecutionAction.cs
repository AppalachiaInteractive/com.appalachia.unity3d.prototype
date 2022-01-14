using System;

namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugConditions.Model
{
    [Flags]
    public enum ExecutionAction
    {
        None = 0,

        /// <summary>
        ///     Takes a screenshot of the game window.
        /// </summary>
        Screenshot = 1 << 0,

        /// <summary>
        ///     If true and the debugger is attached, pauses the debugger.
        /// </summary>
        DebuggerBreak = 1 << 1,

        /// <summary>
        ///     If true, it pauses the editor
        /// </summary>
        PauseEditor = 1 << 2,

        /// <summary>
        ///     Sets the time scale to a specified value.
        /// </summary>
        ModifyTimeScale = 1 << 3,

        /// <summary>
        ///     Log a message.
        /// </summary>
        LogMessage = 1 << 4,

        /// <summary>
        ///     Opens the developer interface..
        /// </summary>
        OpenDeveloperInterface = 1 << 5,
    }
}
