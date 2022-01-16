using UnityEngine;

namespace Appalachia.Prototype.KOC.Areas.Common.Widgets.Models
{
    public class WidgetDimensions
    {
        #region Fields and Autoproperties

        public Vector2 anchorMax;
        public Vector2 anchorMin;

        #endregion

        public float AnchorHeight => anchorMax.y - anchorMin.y;
        public float AnchorWidth => anchorMax.x - anchorMin.x;
    }
}
