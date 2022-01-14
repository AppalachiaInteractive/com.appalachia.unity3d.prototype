using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Playables.Markers
{
    [DisplayName("Skip/EnabledMarker")]
    [CustomStyle("SkipEnabledMarker")]
    public class SkipEnabledMarker : Marker
    {
        #region Fields and Autoproperties

        [SerializeField] public bool active;

        #endregion

        #region Event Functions

        private void Reset()
        {
            active = true;
        }

        #endregion
    }
}
