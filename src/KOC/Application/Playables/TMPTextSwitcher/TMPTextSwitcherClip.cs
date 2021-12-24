using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Application.Playables.TMPTextSwitcher
{
    [Serializable]
    public class TMPTextSwitcherClip : PlayableAsset, ITimelineClipAsset
    {
        public TMPTextSwitcherBehaviour template = new TMPTextSwitcherBehaviour();

        public ClipCaps clipCaps => ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TMPTextSwitcherBehaviour>.Create(graph, template);
            return playable;
        }
    }
}
