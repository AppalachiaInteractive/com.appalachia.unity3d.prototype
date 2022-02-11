using System;
using Appalachia.Core.Attributes.Editing;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Sets.Complex
{
    /// <inheritdoc cref="BaseComplexCursorComponentSetData{TSet,TSetData,TISetData}" />
    [Serializable]
    [SmartLabelChildren]
    public sealed class ComplexCursorComponentSetData : BaseComplexCursorComponentSetData<
        ComplexCursorComponentSet, ComplexCursorComponentSetData, IComplexCursorComponentSetData>
    {
    }
}
