using System.Collections.Generic;
using System.Linq;
using Appalachia.Utility.Extensions;
using Unity.Profiling;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Application.Extensions
{
    public static class TimelineExtensions
    {
        public static IEnumerable<TrackAsset> GetAllTracks(this TimelineAsset asset)
        {
            foreach (var track in asset.GetRootTracks())
            {
                yield return track;

                foreach (var childTrack in track.GetChildTracks())
                {
                    yield return childTrack;
                }
            }
        }

        public static MarkerTrack GetOrCreateMarkerTrack(this TimelineAsset asset)
        {
            using (_PRF_GetOrCreateMarkerTrack.Auto())
            {
                if (asset.markerTrack == null)
                {
                    asset.CreateMarkerTrack();
                    asset.MarkAsModified();
                }

                return asset.markerTrack;
            }
        }

        public static SignalTrack GetOrCreateSignalTrack(this TimelineAsset asset)
        {
            using (_PRF_GetOrCreateSignalTrack.Auto())
            {
                var signalTracks = asset.GetTracks<SignalTrack>();

                var signalTrack = signalTracks.FirstOrDefault();

                if (signalTrack == null)
                {
                    signalTrack = asset.CreateTrack<SignalTrack>();
                    asset.MarkAsModified();
                }

                return signalTrack;
            }
        }

        #region Profiling

        private const string _PRF_PFX = nameof(TimelineExtensions) + ".";

        private static readonly ProfilerMarker _PRF_GetOrCreateSignalTrack =
            new ProfilerMarker(_PRF_PFX + nameof(GetOrCreateSignalTrack));

        private static readonly ProfilerMarker _PRF_GetOrCreateMarkerTrack =
            new ProfilerMarker(_PRF_PFX + nameof(GetOrCreateMarkerTrack));

        #endregion
    }
}
