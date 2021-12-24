using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors
{
    [Serializable]
    public struct CursorStateData
    {
        [HorizontalGroup("2"), PropertyOrder(19)]
        public CursorState state;

        [HorizontalGroup("2"), PropertyOrder(20)]
        public Color color;

        [HorizontalGroup("1"), PropertyOrder(9)]
        [ToggleLeft]
        public bool visible;

        [HorizontalGroup("1"), PropertyOrder(10)]
        [ToggleLeft]
        public bool locked;
    }
}
