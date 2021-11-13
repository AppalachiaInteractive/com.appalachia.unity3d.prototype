using System;
using Appalachia.Audio.Contextual.Context;
using Appalachia.Audio.Contextual.Context.Collections;

namespace Appalachia.Prototype.KOC.Character.Audio.Sounds
{
    [Serializable]
    public class HumanBreathingSounds : AudioContextCollection3<Health_AudioContexts,
        RespirationSpeed_AudioContexts, RespirationStyle_AudioContexts, HumanBreathingSounds>
    {
    }
}
