using System;
using Appalachia.Core.Attributes.Editing;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Sets.Simple
{
    /// <inheritdoc cref="BaseSimpleCursorComponentSetData{TSet,TSetData,TISetData}" />
    [Serializable]
    [SmartLabelChildren]
    public sealed class SimpleCursorComponentSetData : BaseSimpleCursorComponentSetData<
        SimpleCursorComponentSet, SimpleCursorComponentSetData, ISimpleCursorComponentSetData>
    {
    }
}
