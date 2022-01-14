using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Components.Cursors.Metadata;
using Appalachia.Utility.Colors;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Components.Cursors.Collections
{
    [Serializable]
    public class SimpleCursorMetadataLookup : AppaLookup<SimpleCursors, SimpleCursorMetadata,
        SimpleCursorsList, SimpleCursorMetadataList>
    {
        protected override Color GetDisplayColor(SimpleCursors key, SimpleCursorMetadata value)
        {
            return Colors.WhiteSmokeGray96;
        }

        protected override string GetDisplaySubtitle(SimpleCursors key, SimpleCursorMetadata value)
        {
            if (value.texture != null)
            {
                return value.texture.name;
            }

            return value.name;
        }

        protected override string GetDisplayTitle(SimpleCursors key, SimpleCursorMetadata value)
        {
            return key.ToString();
        }
    }
}
