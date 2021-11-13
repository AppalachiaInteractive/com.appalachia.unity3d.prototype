using System;
using Appalachia.Audio.Contextual.Context;
using Appalachia.Audio.Contextual.Context.Collections;

namespace Appalachia.Prototype.KOC.Character.Audio.Sounds
{
    [Serializable]
    public class FootstepSounds : AudioContextCollection3<Surface_AudioContexts, GroundCondition_AudioContexts
        , MovementSpeed_AudioContexts, FootstepSounds>
    {
    }
}
