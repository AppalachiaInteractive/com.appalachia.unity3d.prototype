using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Components.Cursors.Metadata;
using Appalachia.Utility.Colors;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors.Collections
{
    [Serializable]
    public class
        AppaLookup_CursorMetadata : AppaLookup<Cursors, CursorMetadata, AppaList_Cursors,
            AppaList_CursorMetadata>
    {
        protected override Color GetDisplayColor(Cursors key, CursorMetadata value)
        {
            return Colors.WhiteSmokeGray96;
        }

        protected override string GetDisplaySubtitle(Cursors key, CursorMetadata value)
        {
            if (value.texture != null)
            {
                return value.texture.name;
            }

            return value.name;
        }

        protected override string GetDisplayTitle(Cursors key, CursorMetadata value)
        {
            return key.ToString();
        }
    }
}