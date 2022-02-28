using System;
using Appalachia.Core.Attributes.Editing;

namespace Appalachia.Prototype.KOC.Application.Lifetime.Functionality.Features.Cursors.Sets.Simple
{
    /// <inheritdoc />
    [Serializable]
    [SmartLabelChildren]
    public sealed class SimpleCursorComponentSetData : BaseSimpleCursorComponentSetData<
        SimpleCursorComponentSet, SimpleCursorComponentSetData, ISimpleCursorComponentSetData>
    {
    }
}
