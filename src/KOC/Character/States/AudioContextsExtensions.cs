using System;
using Appalachia.Audio.Contextual.Context;

namespace Appalachia.Prototype.KOC.Character.States
{
    public static class AudioContextsExtensions
    {
        public static RespirationSpeed_AudioContexts ToAudio(this RespirationSpeed value)
        {
            switch (value)
            {
                case RespirationSpeed.Normal:
                    return RespirationSpeed_AudioContexts.Normal;
                case RespirationSpeed.Slow:
                    return RespirationSpeed_AudioContexts.Slow;
                case RespirationSpeed.Fast:
                    return RespirationSpeed_AudioContexts.Fast;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public static RespirationStyle_AudioContexts ToAudio(this RespirationStyle value)
        {
            switch (value)
            {
                case RespirationStyle.Nose:
                    return RespirationStyle_AudioContexts.Nose;
                case RespirationStyle.Mouth:
                    return RespirationStyle_AudioContexts.Mouth;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}
