using UnityEngine;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Playables.Markers
{
    [CustomStyle("Annotation")]
    public class Annotation : Marker
    {
        #region Fields and Autoproperties

        [TextArea] public string annotation;

        #endregion
    }
}
