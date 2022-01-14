using System;

namespace Appalachia.Prototype.KOC.Features.Character.States
{
    [Flags]
    public enum HumanHands : byte
    {
        Neither = 0,
        Right = 1,
        Left = 2,
        Both = 3
    }
}
