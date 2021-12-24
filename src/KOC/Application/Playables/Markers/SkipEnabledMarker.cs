using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Application.Playables.Markers
{
    [DisplayName("Skip/EnabledMarker")]
    [CustomStyle("SkipEnabledMarker")]
    public class SkipEnabledMarker : Marker
    {
        [SerializeField] public bool active;

        private void Reset()
        {
            active = true;
        }
    }
}
