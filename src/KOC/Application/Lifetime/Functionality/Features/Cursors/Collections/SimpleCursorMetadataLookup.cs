using System;
using Appalachia.Core.Collections;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Metadata;
using Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Model;
using Appalachia.Utility.Colors;
using UnityEngine;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Collections
{
    [Serializable]
    public class SimpleCursorMetadataLookup : AppaLookup<SimpleCursors, SimpleCursorMetadata,
        SimpleCursorsList, SimpleCursorMetadata.List>
    {
        /// <inheritdoc />
        protected override Color GetDisplayColor(SimpleCursors key, SimpleCursorMetadata value)
        {
            return Colors.WhiteSmokeGray96;
        }

        /// <inheritdoc />
        protected override string GetDisplaySubtitle(SimpleCursors key, SimpleCursorMetadata value)
        {
            if (value.texture != null)
            {
                return value.texture.name;
            }

            return value.name;
        }

        /// <inheritdoc />
        protected override string GetDisplayTitle(SimpleCursors key, SimpleCursorMetadata value)
        {
            return key.ToString();
        }
    }
}
