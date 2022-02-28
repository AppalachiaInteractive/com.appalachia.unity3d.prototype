using System;
using Appalachia.Core.Attributes.Editing;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets.Complex
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class ComplexCursorComponentSetData : BaseComplexCursorComponentSetData<
        ComplexCursorComponentSet, ComplexCursorComponentSetData, IComplexCursorComponentSetData>
    {
    }
}
