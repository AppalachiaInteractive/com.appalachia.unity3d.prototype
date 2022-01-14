using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.Cursors
{
    [Serializable]
    public struct CursorStateData
    {
        #region Fields and Autoproperties

        [HorizontalGroup("1"), PropertyOrder(10)]
        [ToggleLeft]
        public bool locked;

        [HorizontalGroup("1"), PropertyOrder(9)]
        [ToggleLeft]
        public bool visible;

        [HorizontalGroup("2"), PropertyOrder(20)]
        public Color color;

        [HorizontalGroup("2"), PropertyOrder(19)]
        public CursorState state;

        #endregion
    }
}
