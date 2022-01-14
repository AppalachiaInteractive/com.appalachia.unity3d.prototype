using System;
using System.Collections.Generic;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Extensions
{
    public static class SignalEmitterExtensions
    {
        public static IEnumerable<SignalEmitter> Where(
            this IEnumerable<SignalEmitter> emitters,
            Predicate<SignalEmitter> predicate)
        {
            foreach (var emitter in emitters)
            {
                if (predicate(emitter))
                {
                    yield return emitter;
                }
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(SignalEmitterExtensions) + ".";

        #endregion
    }
}
