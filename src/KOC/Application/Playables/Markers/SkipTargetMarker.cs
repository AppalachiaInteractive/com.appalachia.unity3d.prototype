using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Application.Playables.Markers
{
    [DisplayName("Skip/TargetMarker")]
    [CustomStyle("SkipTargetMarker")]
    public class SkipTargetMarker : Marker
    {
        [SerializeField] public bool active;

        private void Reset()
        {
            active = true;
        }
    }
}