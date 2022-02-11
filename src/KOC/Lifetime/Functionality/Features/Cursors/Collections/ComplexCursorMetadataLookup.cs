using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Metadata;
using Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.Utility.Colors;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Collections
{
    [Serializable]
    public class ComplexCursorMetadataLookup : AppaLookup<ComplexCursors, ComplexCursorMetadata,
        ComplexCursorsList, ComplexCursorMetadata.List>
    {
        protected override Color GetDisplayColor(ComplexCursors key, ComplexCursorMetadata value)
        {
            return Colors.WhiteSmokeGray96;
        }

        protected override string GetDisplaySubtitle(ComplexCursors key, ComplexCursorMetadata value)
        {
            return value.name;
        }

        protected override string GetDisplayTitle(ComplexCursors key, ComplexCursorMetadata value)
        {
            return key.ToString();
        }
    }
}
