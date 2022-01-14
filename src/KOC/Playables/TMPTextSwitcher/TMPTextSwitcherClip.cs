using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Playables.TMPTextSwitcher
{
    [Serializable]
    public class TMPTextSwitcherClip : PlayableAsset, ITimelineClipAsset
    {
        #region Fields and Autoproperties

        public TMPTextSwitcherBehaviour template = new TMPTextSwitcherBehaviour();

        #endregion

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TMPTextSwitcherBehaviour>.Create(graph, template);
            return playable;
        }

        #region ITimelineClipAsset Members

        public ClipCaps clipCaps => ClipCaps.Blending;

        #endregion
    }
}
