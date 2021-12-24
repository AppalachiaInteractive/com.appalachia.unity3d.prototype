using System;
using Appalachia.Core.Objects.Scriptables;
using Appalachia.Prototype.KOC.Application.Components.Cursors.Collections;

namespace Appalachia.Prototype.KOC.Application.Components.Cursors.Metadata
{
    [Serializable]
    public class MainComplexCursorLookup : SingletonAppalachiaObjectLookupCollection<ComplexCursors,
        ComplexCursorMetadata, ComplexCursorsList, ComplexCursorMetadataList, ComplexCursorMetadataLookup,
        ComplexCursorLookup, MainComplexCursorLookup>
    {
    }
}
