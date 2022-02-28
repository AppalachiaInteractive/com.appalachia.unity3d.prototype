using System;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Model
{
    [Flags]
    public enum SoftwareCursorStateFlags
    {
        /// <summary>
        ///     <para>Cursor behavior is unmodified.</para>
        /// </summary>
        None = 0,

        /// <summary>
        ///     <para>Lock cursor to the center of the game window.</para>
        /// </summary>
        Locked = 1 << 0,

        /// <summary>
        ///     <para>Confine cursor to the game window.</para>
        /// </summary>
        Confined = 1 << 1,

        /// <summary>
        ///     <para>Hide cursor from view.</para>
        /// </summary>
        Hidden = 1 << 2,
    }
}
