using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Components.Cursors.Metadata;
using Appalachia.Utility.Colors;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.Cursors.Collections
{
    [Serializable]
    public class ComplexCursorMetadataLookup : AppaLookup<ComplexCursors, ComplexCursorMetadata,
        ComplexCursorsList, ComplexCursorMetadataList>
    {
        protected override Color GetDisplayColor(ComplexCursors key, ComplexCursorMetadata value)
        {
            return Colors.WhiteSmokeGray96;
        }

        protected override string GetDisplaySubtitle(ComplexCursors key, ComplexCursorMetadata value)
        {
            if (value.prefab != null)
            {
                return value.prefab.name;
            }

            return value.name;
        }

        protected override string GetDisplayTitle(ComplexCursors key, ComplexCursorMetadata value)
        {
            return key.ToString();
        }
    }
}
