using System;
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes.Editing;
using Appalachia.Core.Objects.Sets;

namespace Appalachia.Prototype.KOC.Lifetime.Functionality.Features.Cursors.Sets.Complex
{
    /// <inheritdoc cref="BaseComplexCursorComponentSet{TSet,TSetData,TISetData}" />
    [Serializable]
    [SmartLabelChildren]
    public sealed class ComplexCursorComponentSet : BaseComplexCursorComponentSet<ComplexCursorComponentSet,
        ComplexCursorComponentSetData, IComplexCursorComponentSetData>
    {
        public override ComponentSetSorting DesiredComponentOrder => ComponentSetSorting.Anywhere;

        /// <summary>
        ///     Defines the name of the component set.
        /// </summary>
        public override string ComponentSetName => APPASTR.Complex_Cursor;
    }
}
