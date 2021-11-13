using System;
using Appalachia.Audio.Contextual.Context;
using Appalachia.Audio.Contextual.Context.Collections;

namespace Appalachia.Prototype.KOC.Character.Audio.Sounds
{
    [Serializable]
    public class HumanFoliageSounds : AudioContextCollection2<FoliageType_AudioContexts,
        MovementSpeed_AudioContexts, HumanFoliageSounds>
    {
    }
}
