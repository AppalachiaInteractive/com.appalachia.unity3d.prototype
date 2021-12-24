using System;
using Appalachia.Core.Collections;
using Appalachia.Utility.Colors;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors.Collections
{
    [Serializable]
    public class ComplexCursorInstanceLookup : AppaLookup<ComplexCursors, ComplexCursorInstance,
        ComplexCursorsList, CursorInstanceList>
    {
        protected override Color GetDisplayColor(ComplexCursors key, ComplexCursorInstance value)
        {
            return Colors.WhiteSmokeGray96;
        }

        protected override string GetDisplaySubtitle(ComplexCursors key, ComplexCursorInstance value)
        {
            if (value == null)
            {
                return "MISSING";
            }

            return value.name;
        }

        protected override string GetDisplayTitle(ComplexCursors key, ComplexCursorInstance value)
        {
            return key.ToString();
        }
    }
}
