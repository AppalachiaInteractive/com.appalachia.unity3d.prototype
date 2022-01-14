namespace Appalachia.Prototype.KOC.Areas.DeveloperInterface.V01.Features.DebugConditions.Model
{
    public enum PacketActivationType
    {
        /// <summary>
        ///     The packet is always eligible for execution.
        /// </summary>
        None = 0,

        /// <summary>
        ///     Prevents the packet from executing before the game has run for X seconds.
        /// </summary>
        EnableAfterSeconds = 1 << 0,

        /// <summary>
        ///     Prevents the packet from executing after the game has run for X seconds.
        /// </summary>
        DisableAfterSeconds = 1 << 1
    }
}
