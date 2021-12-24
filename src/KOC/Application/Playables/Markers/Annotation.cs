using UnityEngine;
using UnityEngine.Timeline;

namespace Appalachia.Prototype.KOC.Application.Playables.Markers
{
    [CustomStyle("Annotation")]
    public class Annotation : Marker
    {
        [TextArea] public string annotation;
    }
}
