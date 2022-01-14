using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Extensions
{
    public static class SignalTrackExtensions
    {
        public static SignalEmitter CreateSignalEmitter(this SignalTrack track, float time)
        {
            using (_PRF_CreateSignalEmitter.Auto())
            {
                var result = track.CreateMarker(typeof(SignalEmitter), time) as SignalEmitter;
                track.MarkAsModified();

                return result;
            }
        }

        public static SignalEmitter GetEndSignalEmitter(this SignalTrack track, SignalAsset asset)
        {
            using (_PRF_GetEndSignalEmitter.Auto())
            {
                var expected = track.GetSignalEmitter(
                    se => (se.asset == asset) && (Math.Abs(se.time - track.parent.duration) < float.Epsilon)
                );

                if (expected != null)
                {
                    return expected;
                }

                var allEmitters = track.GetSignalEmitters().Where(se => se.asset == asset).ToArray();

                if (allEmitters.Length == 0)
                {
                    return null;
                }

                var last = allEmitters[^1];

                return last.time == 0f ? null : last;
            }
        }

        public static SignalEmitter GetSignalEmitter(
            this SignalTrack track,
            Predicate<SignalEmitter> predicate)
        {
            using (_PRF_GetStartSignalEmitter.Auto())
            {
                return track.GetSignalEmitters().FirstOrDefault(se => predicate(se));
            }
        }

        public static IEnumerable<SignalEmitter> GetSignalEmitters(this SignalTrack track)
        {
            foreach (var marker in track.GetMarkers())
            {
                if (marker is SignalEmitter se)
                {
                    yield return se;
                }
            }
        }

        public static SignalEmitter GetStartSignalEmitter(this SignalTrack track, SignalAsset asset)
        {
            using (_PRF_GetStartSignalEmitter.Auto())
            {
                return track.GetSignalEmitter(se => (se.asset == asset) && (se.time == 0f));
            }
        }

        public static IEnumerable<T> GetTracks<T>(this TimelineAsset asset)
            where T : TrackAsset
        {
            foreach (var track in asset.GetAllTracks())
            {
                if (track is T ct)
                {
                    yield return ct;
                }
            }
        }

        public static void SetStartAndEndEmitters(
            this SignalTrack track,
            SignalAsset startAsset,
            SignalAsset endAsset,
            bool retroactive = false,
            bool emitOnce = false)
        {
            using (_PRF_SetStartAndEndEmitters.Auto())
            {
                var startSignalEmitter = track.GetStartSignalEmitter(startAsset);
                var endSignalEmitter = track.GetEndSignalEmitter(endAsset);

                if (startSignalEmitter == null)
                {
                    startSignalEmitter = track.CreateSignalEmitter(0f);
                }
                else
                {
                    startSignalEmitter.time = 0f;
                }

                var frameRate = track.timelineAsset.editorSettings.frameRate;
                var minEndTime = 1.0 / frameRate;
                var endTime = Math.Max(minEndTime, track.parent.duration);

                if (endSignalEmitter == null)
                {
                    endSignalEmitter = track.CreateSignalEmitter((float)endTime);
                }
                else
                {
                    endSignalEmitter.time = (float)endTime;
                }

                startSignalEmitter.asset = startAsset;
                startSignalEmitter.retroactive = retroactive;
                startSignalEmitter.emitOnce = emitOnce;
                startSignalEmitter.name = startAsset.name;

                endSignalEmitter.asset = endAsset;
                endSignalEmitter.retroactive = retroactive;
                endSignalEmitter.emitOnce = emitOnce;
                endSignalEmitter.name = endAsset.name;

                track.MarkAsModified();
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(SignalTrackExtensions) + ".";

        private static readonly ProfilerMarker _PRF_CreateSignalEmitter =
            new ProfilerMarker(_PRF_PFX + nameof(CreateSignalEmitter));

        private static readonly ProfilerMarker _PRF_SetStartAndEndEmitters =
            new ProfilerMarker(_PRF_PFX + nameof(SetStartAndEndEmitters));

        private static readonly ProfilerMarker _PRF_GetStartSignalEmitter =
            new ProfilerMarker(_PRF_PFX + nameof(GetStartSignalEmitter));

        private static readonly ProfilerMarker _PRF_GetEndSignalEmitter =
            new ProfilerMarker(_PRF_PFX + nameof(GetEndSignalEmitter));

        private static readonly ProfilerMarker _PRF_GetSignalEmitters =
            new ProfilerMarker(_PRF_PFX + nameof(GetSignalEmitters));

        #endregion
    }
}
